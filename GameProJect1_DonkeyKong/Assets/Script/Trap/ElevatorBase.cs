using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorBase : MonoBehaviour
{
    public bool playerHasTouch = false;

    private void OnCollisionEnter(Collision other) 
    {
        if(other.gameObject.CompareTag("Player"))
        {
            playerHasTouch = true;    
        }
    }
}
