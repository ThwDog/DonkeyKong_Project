using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadLine : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.layer != 8 || other.gameObject.layer != 6 || other.gameObject.layer != 10)
        {
            Debug.Log("Destroy " + other.name);
            Destroy(other.gameObject);   
        } 
    }
}
