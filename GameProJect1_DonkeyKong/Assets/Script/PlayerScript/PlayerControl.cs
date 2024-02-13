using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

[RequireComponent(typeof(CharacterController),typeof(Controller))]
public class PlayerControl : MonoBehaviour , IDestoryable , IDamageable
{
    public static PlayerControl _instance;
    public static PlayerControl instance{get{return _instance;}}

    private Controller inputCon;
    private CharacterController player;
    private Collider playerCollider;
    private Collider[] ladderCollider;

    [Header("movement")]
    [SerializeField]float speed;
    [SerializeField]float climbSpeed;
    private float verticalSpeed;
    [SerializeField]float j_force;
    [SerializeField]private float gravityValue = 20f;
    private float current_GValue;
    [SerializeField]private float stickingGravityPro = 20f;
    [SerializeField][Range(-10.0f,10.0f)]float input_Delay;
    private bool canJump = true;
    private bool isClimbing = false;
    [HideInInspector]public bool canClimbing = true;
    [SerializeField]private bool isLadder = false;
    [SerializeField]private bool isLadderDown = false;
    
    [Header("Fall System")]
    private bool isGrounded;//current ground
    private bool wasGrounded;//ever grounded
    private bool wasFalling;//ever grounded
    private float startOfFall;
    [SerializeField][Range(0,10)] float minFall;
    


    [Header("player")]
    [SerializeField]private bool _isDead = false;
    internal bool canTakeDamage = true;
    public bool isDead
    {
        get{return _isDead;}
        set{_isDead = value;}
    }

    bool isFalling
    {
        get{return !isGrounded & player.velocity.y < 0;}
    }

    bool ground()
    {
        float distToGround = playerCollider.bounds.extents.y;
        return Physics.Raycast(transform.position, Vector3.down,distToGround + 0.1f);
    }

    private void Awake() 
    {
        player = GetComponent<CharacterController>();
        playerCollider = GetComponent<Collider>();
        inputCon = Controller.instance;   
        //Cursor.visible = false;
    }

    private void Start() 
    {
        current_GValue = gravityValue;
    }

    private void Update() 
    {
        if(!canTakeDamage)
            isDead = false;
        dead();
        LockPlayersZPos();
    }

    private void FixedUpdate() 
    {
        //checkRay();
        checkGround();
        move();
        isGrounded = ground();
        if(!wasFalling & isFalling & !isLadder)
            startOfFall = transform.position.y;
        if(!wasGrounded & isGrounded & !isLadder)
            fallDamage();
        wasGrounded = isGrounded;
        wasFalling = isFalling;
    }


    private void move()
    {   
        if(isLadder && inputCon.movement.y > 0f && canClimbing)
            isClimbing = true;
        if(!isLadderDown || !isLadder || isLadder & isGrounded)
        {
            Vector3 move = new Vector3(inputCon.movement.x,0,0);
            //only side move
            if(move.sqrMagnitude > 1.0f)
                move.Normalize();
            if(!isClimbing)
                move += verticalSpeed * Vector3.up * Time.deltaTime;
            player.Move(move *speed);
        }
        climb();
    
    }

    /*void checkRay()
    {
        float distToGround = playerCollider.bounds.extents.y;
        isGrounded = Physics.Raycast(transform.position, Vector3.down,distToGround + 0.1f);
    }*/

    void LockPlayersZPos()
    {
        if (transform.position.z != -7.31f)
        {
            transform.SetPositionAndRotation(new Vector3(transform.position.x, transform.position.y, -7.31f), transform.rotation);
        }
    }

    void checkGround()
    {
        if(!isClimbing)
        {    
            current_GValue = gravityValue;
            if (isGrounded)
            {
                verticalSpeed = -current_GValue * stickingGravityPro;
                if (inputCon.jump && isGrounded && canJump)
                {
                    verticalSpeed = j_force;
                    StartCoroutine(jumpDelay());
                }
            }
            else
            {
                verticalSpeed -= current_GValue;
            }
        }
        else current_GValue = 0;
    }

    

    private void climb()
    {  
        //ladderCollider = Physics.OverlapBox(gameObject.transform.position - new Vector3 (0,2,0), transform.localScale / 2, Quaternion.identity,LayerMask.GetMask("Ladder"));
        ladderCollider = Physics.OverlapSphere(gameObject.transform.position - new Vector3 (0,3,0), 1,LayerMask.GetMask("Ladder"));
        if(ladderCollider.Length >= 1)
        {    
            isLadderDown = true;
            if(isLadderDown && inputCon.movement.y < 0 && canClimbing)
            {
                isClimbing = true;
                isGrounded = false;
            }
            else if(!isLadder)
            {
                isClimbing = false;
            }
        }
        else  
        {
            isLadderDown = false;
            isGrounded = true;            
        }

        if(isClimbing & isLadder || isLadderDown & isClimbing)
        {
            Physics.IgnoreLayerCollision(3,6,!isGrounded);
            Vector3 move = new Vector3(0,inputCon.movement.y,0);
            move.x = 0;
            player.Move(move * climbSpeed);
        } 
    }

    private void OnDrawGizmos() 
    {
        Gizmos.color = Color.red;
        //Use the same vars you use to draw your Overlap SPhere to draw your Wire Sphere.
        //Gizmos.DrawCube (gameObject.transform.position - new Vector3 (0,2,0), transform.localScale);
        Gizmos.DrawSphere(gameObject.transform.position - new Vector3 (0,3,0), 1);
    }

    IEnumerator jumpDelay()
    {
        canJump = false;
        yield return new WaitForSeconds(input_Delay);
        canJump = true;
    }

    public void dead()
    {
        if(isDead)
            inputCon.releaseController();
        //else inputCon.gainController();
    }

    public void des(bool destroy)
    {
        if(destroy)
        {
            Debug.Log("player Dead");
        }
    }

    void fallDamage()
    {
        float fallDis = startOfFall - transform.position.y;
        if(fallDis > minFall)
        {
            GameManager.instance._LP--;
            Debug.Log("Fall Damage");
        }
    }

    
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Ladder"))
        {
            isLadder = true;
        }
        
    }


    private void OnTriggerExit(Collider other) 
    {
        if(other.CompareTag("Ladder"))
        {
            isLadder = false;
            isClimbing = false;
        }
    }
 
    private void OnControllerColliderHit(ControllerColliderHit hit) 
    {
        if(hit.gameObject.GetComponent<MoveingFloor>())
        {
            MoveingFloor moveingFloor = hit.gameObject.GetComponent<MoveingFloor>();
            moveingFloor.pushPlayer(this.gameObject);
        }    
    }

    public void takeDamage()
    {
        GameManager.instance._LP--;
        //player dead play animation wait and re scene
        isDead = true;
    }


}
