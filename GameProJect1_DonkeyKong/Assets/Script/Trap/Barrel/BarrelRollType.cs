using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class BarrelRollType : MonoBehaviour
{
    Rigidbody rb;
    bool hitColliderRight = false;
    bool hitWall = false;
    bool playerAbove = false;
    [Header("Setting")]
    [SerializeField] float roteSpeed;
    public bool hitPlayer = false;
    [SerializeField]private Collider[] ladderCollider;
    private bool isLadderDown;
    private bool canRnd = true;
    private bool isClimbing;
    private bool canClimbing;
    private bool isBaseGround = false;
    private Collider _collider;
    [Header("VFX")]
    VFXScript vfx;
    [SerializeField] string VfxScriptObjName;
    bool isGrounded;
    bool onAir;

    private void Start() 
    {
        _collider = GetComponent<Collider>();
        //_collider.material.bounceCombine = PhysicMaterialCombine.Minimum;
        rb = GetComponent<Rigidbody>();    

        vfx = GameObject.Find(VfxScriptObjName).GetComponent<VFXScript>();
    }

    private void Update() 
    {
        //cheack if player on above floor
        RaycastHit[] hit;
        Ray ray = new Ray(transform.position, Vector3.up);
        hit = Physics.RaycastAll(ray);

        foreach(RaycastHit obj in hit)
        {
            if(obj.transform.gameObject.CompareTag("Player"))
            {
                playerAbove = true;
            } 
        }    

        if(!isGrounded)
            onAir = true;
        if(isGrounded && onAir)
        {
            vfx.playParticle(gameObject.transform,new Vector3(0,-3.5f,0.5f));
            onAir = false;
        }
    }

    public void _NormalRoll(float speed)
    {
        Collider[] wall = FindObjectsOfType(typeof(Collider)) as Collider[];
        foreach (Collider ignore in wall)
        {
            if(ignore.gameObject.layer == 10)
            {
                Physics.IgnoreCollision(_collider,ignore,playerAbove);
            }
        }

        Vector3 dir = hitColliderRight? Vector3.left : Vector3.right;
        float _roteSpeed = hitColliderRight? roteSpeed : -roteSpeed;
        _roteSpeed = hitPlayer? 0 : _roteSpeed;

        barrelRolling(_roteSpeed);

        float currentSpeed = hitPlayer || hitWall? 0 : speed;
        //Physics.IgnoreLayerCollision(9,10,playerAbove); 

        rb.AddForce(dir * currentSpeed * Time.deltaTime,ForceMode.Impulse);
    }

    IEnumerator DecreadSpeed()//Decread Speed When hit the wall 
    {
        hitWall = true;
        //Debug.Log("HitWall");
        yield return new WaitForSeconds(1.5f);
        hitWall = false;
    }

    public void _Opening(float speed)
    {
        if(isBaseGround)
        {
            transform.localRotation = new Quaternion(transform.rotation.x,0,0,transform.rotation.w);
            rb.AddForce(Vector3.left * speed * Time.deltaTime,ForceMode.Impulse);
        }
    }

    public void _CrossType(List<Transform> goDir,float speed)
    { 
        if(rb.useGravity)
            rb.useGravity = false;
        if(transform.position == goDir[0].position)
            goDir.Remove(goDir[0]);
        if(goDir.Count != 0)
            transform.position = Vector3.MoveTowards(transform.position,goDir[0].position,speed * Time.deltaTime);
        else Destroy(gameObject);
        
    }

    public void _RollingOnLadder(float speed)
    {
        if(!isClimbing)
            _NormalRoll(speed);
        else
            rb.velocity = new Vector3(0,-transform.position.y / 3,0);
        
        ladderCollider = Physics.OverlapSphere(gameObject.transform.position - new Vector3 (0,3,0), 1,LayerMask.GetMask("Ladder"));//ladder must in layer ladder 

        if(ladderCollider.Length >= 1)
        {
            isLadderDown = true;
            canClimbing = true;
        }
        else
        {
            isLadderDown = false;
            canClimbing = false;
        }
        if(isLadderDown && canRnd)
        {
            int rnd;
            StartCoroutine(randomNum());
            //rnd = Random.Range(0,2);
            rnd = 1;
            switch(rnd)
            {
                case 0:
                    //Debug.Log("0");
                    break;
                case 1:
                    //Debug.Log("1");
                    if(ladderCollider[0].gameObject.transform.childCount > 0 && canClimbing && !isClimbing)
                    {    
                        //isClimbing = true;
                        Invoke("climb",0.1f);
                        //climb();
                    }
                    break;
                default:
                    break;
            }
        }
    }

    void climb()
    {
        // if(isClimbing)
        // {
        //     StartCoroutine(ignoreCollision());
        // }
        isClimbing = true;
        StartCoroutine(ignoreCollision());
    }

    IEnumerator randomNum()
    {
        
        canRnd = false;
        yield return new WaitForSeconds(1f);
        canRnd = true;
    }

    IEnumerator ignoreCollision()
    {
        if(ladderCollider[0].gameObject.transform.GetChild(0) == true)
        {
            // play animation
            transform.localRotation = Quaternion.Euler(0,0,90);
            hitWall = true;
            _collider.isTrigger = true;
            hitColliderRight = true;
            Invoke("resetModel",0.7f);
            yield return new WaitForSeconds(0.3f);//change it if barrel not fall
            isClimbing = false;
            hitWall = false;
            _collider.isTrigger = false;
        }
    }

    void resetModel()
    {
        transform.localRotation = Quaternion.Euler(0,90,90);
    }


    private void OnCollisionEnter(Collision other) 
    {
        if(other.gameObject.name == "ColliderRight")
        {
            StartCoroutine(DecreadSpeed());
            _collider.material.bounciness = 0.5f;
            hitColliderRight = true;
        } 
        else if(other.gameObject.name == "ColliderLeft")
        {
            StartCoroutine(DecreadSpeed());
            _collider.material.bounciness = 0.5f;
            hitColliderRight = false;
        }    
        if(other.gameObject.CompareTag("Player"))
        {
            PlayerControl player = other.gameObject.GetComponent<PlayerControl>();
            if(!hitPlayer)
            {
                hitPlayer = true;
                Debug.Log("Rolling on Player");
                //add animation and effect
                player.takeDamage();
            }
        }
        if(other.gameObject.layer == 8)
        {
            isBaseGround = true;
        }
        if(other.gameObject.layer == 8 || other.gameObject.layer == 6)
            isGrounded = true;
        else isGrounded = false;
    }
    
    void barrelRolling(float speed)
    {
        transform.Rotate(0,speed *Time.deltaTime,0,Space.Self);
    }
}
