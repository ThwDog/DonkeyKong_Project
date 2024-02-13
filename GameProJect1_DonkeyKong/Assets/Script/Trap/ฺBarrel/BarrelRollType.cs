using System.Collections;
using System.Collections.Generic;
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
    public bool hitPlayer = false;
    private Collider[] ladderCollider;
    private bool isLadderDown;
    private bool canRnd = true;
    private bool isClimbing;
    private bool canClimbing;
    private bool isBaseGround = false;
    private Collider _collider;

    private void Start() 
    {
        _collider = GetComponent<Collider>();
        //_collider.material.bounceCombine = PhysicMaterialCombine.Minimum;
        rb = GetComponent<Rigidbody>();    
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
            rb.AddForce(Vector3.left * speed * Time.deltaTime,ForceMode.Impulse);
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
        
        ladderCollider = Physics.OverlapSphere(gameObject.transform.position - new Vector3 (0,3,0), 1,LayerMask.GetMask("Ladder"));

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
                        isClimbing = true;
                        climb();
                    }
                    break;
                default:
                    break;
            }
        }
    }

    void climb()
    {
        if(isClimbing)
        {
            StartCoroutine(ignoreCollision());
            //Transform ladderDown = ladderCollider[0].gameObject.transform.GetChild(0);
            //rb.AddForce(Vector3.Lerp(transform.position,ladderDown.position,speed * Time.deltaTime));
        }
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
            hitWall = true;
            _collider.isTrigger = true;
            hitColliderRight = true;
            yield return new WaitForSeconds(0.3f);//change it if barrel not fall
            isClimbing = false;
            hitWall = false;
            _collider.isTrigger = false;
        }
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
    }
    

}
