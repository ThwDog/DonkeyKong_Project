using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilBarrelSpawnEnemyLvl3 : SpawnPointScript
{
    [SerializeField] int maximumSpawn;
    [SerializeField] List<EnemyHolder> enemyCount;
    [SerializeField] GameObject spawnPosRight ,spawnPosLeft;

    private void Start() 
    {
        checkDiff(); 
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
            StartCoroutine(spawn());
        }
    }

    void checkDiff()
    {
        var diff = GameManager.instance.difficulty;

        switch(diff)
        {
            case GameManager.diff.one:
                maximumSpawn = 1;
                break;
            case GameManager.diff.two:
                maximumSpawn = 2;
                break;
            case GameManager.diff.three:
                maximumSpawn = 3;
                break;
            case GameManager.diff.four:
                maximumSpawn = 4;
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player");
            PlayerControl player = other.GetComponent<PlayerControl>();

            player.takeDamage();
            Debug.Log("Hit");
        }
        if(other.GetComponent<FoodTrayScript>())
        {
            Destroy(other.gameObject);
        }
    }

    IEnumerator spawn()
    {
        spawnEnemy(spawnObj,spawnPosRight);
        yield return new WaitForSecondsRealtime(1.5f);
    }

}
