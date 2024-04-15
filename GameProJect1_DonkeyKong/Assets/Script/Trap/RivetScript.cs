using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RivetScript : MonoBehaviour
{

    private void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Player"))
        {
            CheckWinByRivet rivet = FindAnyObjectByType<CheckWinByRivet>();
            Debug.Log("Boom");
            rivet.rivetBoomCount++;
            gameObject.SetActive(false);
            // Destroy(this.gameObject);        
        }  
    }

}
