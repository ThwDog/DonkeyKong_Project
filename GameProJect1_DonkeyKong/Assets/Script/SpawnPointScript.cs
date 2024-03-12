using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnPointScript : MonoBehaviour
{
    public enum _spawnType
    {
        normal , enemy
    }

    [Header("Setting")]
    public GameObject spawnObj;
    [SerializeField] bool canMutiSpawn;
    [SerializeField] bool canSpawnEnemy;
    public _spawnType spawnType;
    [SerializeField][Range(0,10)] float spawnDelayTime;
    private bool canSpawn = true;

    private void Awake() 
    {
        spawnNormal();
        if(canSpawnEnemy)
            spawnEnemy(spawnObj,gameObject);
    }

    public void spawnNormal()
    {
        //if(GameObject.FindGameObjectWithTag("Player") == null)
            //Instantiate(spawned, transform.position, Quaternion.identity);
        if(spawnType == _spawnType.normal)
        {
            if(GameObject.Find(spawnObj.name) == null)
                Instantiate(spawnObj, transform.position, Quaternion.identity);
            else if(canMutiSpawn & spawnObj.GetComponent<PlayerControl>() == false)
                Instantiate(spawnObj, transform.position, Quaternion.identity);
        }
    }

    public void spawnEnemy(GameObject spawn,GameObject spawnerPosition)
    {
        if(spawnType == _spawnType.enemy && canSpawn)
        {
            StartCoroutine(spawnDelay(spawn,spawnerPosition));
            Debug.Log("Spawn Enemy");
        }
    }

    public virtual IEnumerator spawnDelay(GameObject spawn,GameObject spawnerPosition)
    {
        canSpawn = false;
        Instantiate(spawn,spawnerPosition.transform.position,Quaternion.identity);
        yield return new WaitForSecondsRealtime(spawnDelayTime);
        canSpawn = true;
    }
}
