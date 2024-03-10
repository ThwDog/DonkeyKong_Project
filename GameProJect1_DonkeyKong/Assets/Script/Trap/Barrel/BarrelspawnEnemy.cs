using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelspawnEnemy : SpawnPointScript
{
    [Header("Barrel Spawn")]
    [SerializeField] private LayerMask layerMask01;
    [SerializeField] private LayerMask layerMask02;

    void OnCollisionEnter(Collision other)
    {
        GameObject objHitToSpawn = other.gameObject;
        var hitLayerMask = 1 << other.gameObject.layer; // need to shift(<<) layer of game obj before use it because layer mask is use by bit mask
        //Debug.Log(Convert.ToString(layerMask01,2).PadLeft(32,'0')); 

        // if(objHitToSpawn.CompareTag("Enemy") && objHitToSpawn.layer == 9 || objHitToSpawn.CompareTag("Enemy") && objHitToSpawn.layer == 11) 
        if (objHitToSpawn.CompareTag("Enemy") && hitLayerMask == layerMask01 || objHitToSpawn.CompareTag("Enemy") && hitLayerMask == layerMask02)
        {
            spawnEnemy(spawnObj,gameObject);
            Destroy(objHitToSpawn);
        }

    }
}
