using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointScript : MonoBehaviour
{
    public enum _spawnType
    {
        normal , enemy
    }

    [Header("Setting")]
    public GameObject spawned;
    [SerializeField] bool canMutiSpawn;
    public _spawnType spawnType;
    [SerializeField][Range(0,10)] float spawnDelayTime;
    private bool canSpawn = true;

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

    public void spawnEnemy(GameObject spawn)
    {
        if(spawnType == _spawnType.enemy && canSpawn)
        {
            StartCoroutine(spawnDelay(spawn));
            Debug.Log("Spawn Enemy");
        }
    }

    IEnumerator spawnDelay(GameObject spawn)
    {
        canSpawn = false;
        Instantiate(spawn,transform.position,Quaternion.identity);
        yield return new WaitForSeconds(spawnDelayTime);
        canSpawn = true;
    }
}
