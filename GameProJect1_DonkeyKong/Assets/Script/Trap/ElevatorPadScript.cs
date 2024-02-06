using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorPadScript : MonoBehaviour
{
    ElevatorScript elevator;
    public float speed;
    [SerializeField]Rigidbody rb;
    [HideInInspector]public Transform target;
    public bool playerStay = false;

    private void Awake() 
    {
        rb = GetComponent<Rigidbody>();     
        elevator = GameObject.Find("Elevator").GetComponent<ElevatorScript>();     

    }

    private void FixedUpdate() 
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        if(!elevator.start)
            speed = 0;
    }

    private void OnCollisionEnter(Collision other) 
    {
        if(other.gameObject.CompareTag("Elevator"))
        {
            //Debug.Log("pad End");
            //elevator.padCount.Remove(gameObject);
            Destroy(gameObject);    
        }
    }

    void OnCollisionStay(Collision other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            playerStay = true;
        }
    }
}
