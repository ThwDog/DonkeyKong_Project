using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RivetScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("Boom");
            Destroy(this.gameObject);        
        }  
    }

}
