using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelspawnEnemy : SpawnPointScript
{
    void OnCollisionEnter(Collision other)
    {
        GameObject barrel = other.gameObject;
        if(barrel.CompareTag("Enemy") && barrel.layer == 9 || barrel.CompareTag("Enemy") && barrel.layer == 11) 
        {
            spawnEnemy(spawned);
            Destroy(barrel);
        }   
    }


}
