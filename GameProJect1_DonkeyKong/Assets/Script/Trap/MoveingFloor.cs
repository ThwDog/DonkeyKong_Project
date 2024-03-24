using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.Rendering;
using UnityEngine;

public class MoveingFloor : MonoBehaviour
{
    public enum direction
    {
        left = 1, right = 0
    }

    public direction Direction;
    [SerializeField] float pushForce;
    public bool flip;
    bool canPush = true;
    private Vector3 beforePos; //Default position
    [SerializeField] Vector3 afterPos; // position after flip
    Animator anim;

    private void Start()
    {
        beforePos = gameObject.transform.position;
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (GameManager.instance.state != GameManager._state.playing)
        {
            anim.speed = 0;
            canPush = false;
        }

        roteAnimation();
        if (flip)
        {
            if (Direction == direction.right)
                Direction = direction.left;
            else Direction = direction.right;

            // gameObject.transform.localPosition = afterPos;
            //gameObject.transform.localScale = new Vector3(-gameObject.transform.localScale.x,gameObject.transform.localScale.y,gameObject.transform.localScale.z); 
            flip = false;
        }
        else
        {
            //transform.transform.position = beforePos;
            //gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x,gameObject.transform.localScale.y,gameObject.transform.localScale.z); 
        }
    }

    public void pushPlayer(GameObject obj)
    {
        if (canPush)
        {
            CharacterController player = obj.GetComponent<CharacterController>();
            Vector3 dir = Direction == direction.right ? Vector3.right : Vector3.left;
            player.Move(dir * pushForce * Time.fixedDeltaTime);
        }
    }

    public void pushObj(GameObject obj, float speed)
    {
        if (canPush)
        {
            Rigidbody rb = obj.GetComponent<Rigidbody>();
            Vector3 dir = Direction == direction.right ? Vector3.right : Vector3.left;

            rb.AddForce(dir * (pushForce * speed) * Time.deltaTime);
        }
    }

    private void roteAnimation()
    {
        if (Direction == direction.right)
        {
            gameObject.transform.position = beforePos;
            gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
        else
        {
            gameObject.transform.localPosition = afterPos;
            gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        }
    }


}
