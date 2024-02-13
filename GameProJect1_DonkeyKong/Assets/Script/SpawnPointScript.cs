using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointScript : MonoBehaviour
{
    public enum _spawnType
    {
        normal , enemy
    }

    public GameObject spawned;
    [SerializeField] bool canMutiSpawn;
    public _spawnType spawnType;

    private void Awake() 
    {
        spawnNormal();
        
    }

    public void spawnNormal()
    {
        //if(GameObject.FindGameObjectWithTag("Player") == null)
            //Instantiate(spawned, transform.position, Quaternion.identity);
        if(spawnType == _spawnType.normal)
        {
            if(GameObject.Find(spawned.name) == null)
                Instantiate(spawned, transform.position, Quaternion.identity);
            else if(canMutiSpawn & spawned.GetComponent<PlayerControl>() == false)
                Instantiate(spawned, transform.position, Quaternion.identity);
        }
    }

    public void spawnEnemy()
    {
        if(spawnType == _spawnType.enemy)
        {
            Debug.Log("Spawn Enemy");
        }
    }
}
