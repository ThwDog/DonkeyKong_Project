using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScriptLvl4 : SpawnPointScript
{
    [Header("level 04")]
    [SerializeField] GameObject[] setSpawnPoint;
    [SerializeField] int maximumSpawn;
    [SerializeField] int spawnPointInt;
    [SerializeField] List<EnemyHolder> enemyCount;

    private void Start()
    {
        spawnPointInt = Random.Range(0,setSpawnPoint.Length);
    }

    private void Update() 
    {
        foreach(EnemyHolder _enemy in FindObjectsOfType<EnemyHolder>())
        {
            if(enemyCount.Contains(_enemy))
                continue;
            enemyCount.Add(_enemy);
        }

        if(enemyCount.Count < maximumSpawn)
        {
            spawnEnemy(spawnObj,setSpawnPoint[spawnPointInt]);
        }
    }

    public override IEnumerator spawnDelay(GameObject spawn, GameObject spawnerPosition)
    {
        spawnPointInt = Random.Range(0,setSpawnPoint.Length);
        return base.spawnDelay(spawn, spawnerPosition);
    }
}
