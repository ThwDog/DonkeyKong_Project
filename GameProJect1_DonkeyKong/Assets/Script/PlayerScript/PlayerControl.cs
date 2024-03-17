using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController),typeof(Controller))]
public class PlayerControl : MonoBehaviour , IDamageable
{
    public static PlayerControl _instance;
    public static PlayerControl instance{get{return _instance;}}

    private Controller inputCon;
    private CharacterController player;
    private Collider playerCollider;
    private Collider[] ladderCollider;

    [Header("movement")]
    [SerializeField] int checkLadderDownHeight;
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
    [SerializeField]private float rotationSpeed;
    
    [Header("Fall System")]
    private bool isGrounded;//current ground
    private bool wasGrounded;//ever grounded
    private bool wasFalling;//ever grounded
    private float startOfFall;
    [SerializeField][Range(0,10)] float minFall;
    
    [Header("About animation and audio")]
    private bool canWalk = true;

    [Header("player")]
    internal Animator anim;
    [SerializeField]private bool _isDead = false;
    internal bool canTakeDamage = true;
    private CollectItem_Player item_Player;

    public bool isDead
    {
        get{return _isDead;}
        set{_isDead = value;}
    }

    bool isFalling // check if fall
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
        anim = GetComponentInChildren<Animator>();
        item_Player = GetComponent<CollectItem_Player>();
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
        // if(!isGrounded & player.velocity.y < 0)
        //     anim.SetBool("Fall",true);
        // else if(!canJump)
        // {
        //     anim.SetBool("Fall",false);
        // }
        // else anim.SetBool("Fall",false);

        anim.SetBool("Climb",isClimbing);
        
        if(isClimbing && inputCon.movement.y == 0)
            anim.speed = 0;
        else anim.speed = 1;
        

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
            if(inputCon.movement.x != 0 && canWalk && isGrounded)
                StartCoroutine(delayWalk());

            anim.SetFloat("Speed",inputCon.movement.x); // animation walk to idle

            Vector3 move = new Vector3(inputCon.movement.x,0,0);
            //only side move
            if(move.sqrMagnitude > 1.0f)
                move.Normalize();

            if(move != Vector3.zero && !isClimbing)
            {
                Quaternion toRotation = Quaternion.LookRotation(move,Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation,toRotation,rotationSpeed * Time.deltaTime);
            }
            
            if(!isClimbing)
                move += verticalSpeed * Vector3.up * Time.deltaTime;
            player.Move(move *speed);
        }
        climb();
    
    }

    IEnumerator delayWalk()
    {
        canWalk = false;
        SoundManager.instance.PlaySfx("Walk");
        yield return new WaitForSeconds(0.3f);
        canWalk = true;
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
                    
                    anim.SetTrigger("Jump");

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
        ladderCollider = Physics.OverlapSphere(gameObject.transform.position - new Vector3 (0,checkLadderDownHeight,0), 1,LayerMask.GetMask("Ladder"));
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

            transform.rotation = Quaternion.Euler(new Vector3(0,0,0));

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
        Gizmos.DrawSphere(gameObject.transform.position - new Vector3 (0,checkLadderDownHeight,0), 1);
    }

    IEnumerator jumpDelay()
    {
        canJump = false;
        
        SoundManager.instance.PlaySfx("Jump");
        yield return new WaitForSeconds(input_Delay);
        canJump = true;
    }

    public void dead()
    {
        if(GameManager.instance._LP <= 0) 
        {
            SoundManager.instance.StopAllMusic();
            ScriptSceneManager.instance.toMainMenu();
            //SceneManager.LoadScene("MainMenu");
            GameManager.instance._reset();
        }
        if(isDead)
            inputCon.releaseController();
        //else inputCon.gainController();
    }

    void fallDamage()
    {
        float fallDis = startOfFall - transform.position.y;
        //Debug.Log(fallDis);
        if(fallDis > minFall)
        {
            takeDamage();
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
        if(canTakeDamage)
        {
            GameManager.instance._LP--;
            SoundManager.instance.StopAllMusic();
            anim.updateMode = AnimatorUpdateMode.UnscaledTime;
            //player dead play animation wait and re scene
            item_Player.resetBeforeHaveWeapon();
            isDead = true;
            anim.SetBool("Dead",isDead);

            Debug.Log("animation finish");
            if(GameManager.instance._LP != 0 && isDead)
                StartCoroutine(revive());
            
        }
    }

    IEnumerator revive()
    {
        //play animation dead use unscaled time
        Debug.Log("Dead");
        transform.rotation = new Quaternion(0,180,0,0);// rote because animation should look like this
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(3.0f);//dead delay time
        Debug.Log("Re");
        // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameManager.instance.state = GameManager._state.lose;
        anim.updateMode = AnimatorUpdateMode.Normal;
        Time.timeScale = 1;
        isDead = false;
        yield break;
    }
}
