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
    private Vector3 beforePos; //Default position
    [SerializeField] Vector3 afterPos; // position after flip

    private void Start() 
    {
        beforePos = gameObject.transform.position;     
    }

    private void Update() 
    {
        roteAnimation();
        if(flip)
        {
            if(Direction == direction.right)
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
        CharacterController player = obj.GetComponent<CharacterController>();
        Vector3 dir = Direction == direction.right? Vector3.right : Vector3.left;
        player.Move(dir * pushForce * Time.fixedDeltaTime);
    }

    public void pushObj(GameObject obj , float speed)
    {
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        Vector3 dir = Direction == direction.right? Vector3.right : Vector3.left;
        
        rb.AddForce(dir * (pushForce * speed) * Time.deltaTime);
    }

    private void roteAnimation()
    {
        if(Direction == direction.right)
        {
            gameObject.transform.position = beforePos;
            gameObject.transform.rotation = Quaternion.Euler(new Vector3(0,0,0));
        }
        else
        {
            gameObject.transform.localPosition = afterPos;
            gameObject.transform.rotation = Quaternion.Euler(new Vector3(0,180,0));
        }
    }
}
