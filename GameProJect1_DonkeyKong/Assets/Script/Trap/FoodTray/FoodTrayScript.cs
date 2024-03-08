using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodTrayScript : MonoBehaviour
{
    [SerializeField] float speed;

    private void OnCollisionStay(Collision other)
    {
        if(other.gameObject.GetComponent<MoveingFloor>())
        {
            MoveingFloor moveingFloor = other.gameObject.GetComponent<MoveingFloor>();
            Debug.Log("Move");
            moveingFloor.pushObj(gameObject,speed);
        }    
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("Player");
            PlayerControl player = other.GetComponent<PlayerControl>();

            player.takeDamage();
            Debug.Log("Hit");
        } 
    }
}
