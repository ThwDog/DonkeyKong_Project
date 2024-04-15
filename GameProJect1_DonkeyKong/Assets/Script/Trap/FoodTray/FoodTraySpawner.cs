using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class FoodTraySpawner : SpawnPointScript
{
    [Header("Spawner Setting")]
    [SerializeField] GameObject[] normalSpawner ,leftRightSpawner;
    [SerializeField] bool spawnerIsLeft = true;
    private int normalSpawnIndex = 0;


    [SerializeField] GameObject foodTray;

    [Header("Moving Floor")]
    [SerializeField] MoveingFloor[] movingFloor;
    [SerializeField] MoveingFloor topMovingFloor , downMovingFloor;
    [SerializeField] [Range(0,10)] float topFloorDelay;

    [Header("")]
    private bool _canSpawn = true; 

    private void Start() 
    {
        patternMovingFloor();
    }

    private void Update() 
    {
        normalSpawn();
        leftAndRightSpawn();
    }

    void normalSpawn()
    {
        if(normalSpawnIndex > normalSpawner.Length - 1)
            normalSpawnIndex = 0;
        else 
            spawnEnemy(foodTray,normalSpawner[normalSpawnIndex]);
    }

    void leftAndRightSpawn()
    {
        
        if(spawnerIsLeft && _canSpawn)
            StartCoroutine(leftNRightSpawn(foodTray,leftRightSpawner[0]));
        else if(!spawnerIsLeft && _canSpawn)
            StartCoroutine(leftNRightSpawn(foodTray,leftRightSpawner[1]));
    }

    public IEnumerator leftNRightSpawn(GameObject spawn,GameObject spawnerPosition)
    {
        _canSpawn = false;
        Instantiate(spawn,spawnerPosition.transform.position,Quaternion.identity);
        yield return new WaitForSecondsRealtime(2.5f);
        _canSpawn = true;
    }

    public override IEnumerator spawnDelay(GameObject spawn, GameObject spawnerPosition)
    {
        normalSpawnIndex++;
        return base.spawnDelay(spawn, spawnerPosition);
    }

    void patternMovingFloor()
    {
        StartCoroutine(topMovingFloorFlip());    
        StartCoroutine(switchLeftAndRight());
        Invoke("_normalMovingSwitch",8f);   
    }
    
    // pattern top floor moving
    IEnumerator topMovingFloorFlip()
    {
        //Debug.Log("Flip");
        topMovingFloor.flip = !topMovingFloor.flip;
        yield return new WaitForSeconds(topFloorDelay);
        StartCoroutine(topMovingFloorFlip());    
    }

    // pattern down floor moving
    IEnumerator switchLeftAndRight()
    {
        yield return new WaitForSecondsRealtime(8f);
        spawnerIsLeft = !spawnerIsLeft;
        downMovingFloor.flip = !downMovingFloor.flip;
        StartCoroutine(switchLeftAndRight());    
    }

    void _normalMovingSwitch()
    {
        StartCoroutine(normalMovingSwitch());
    }

    IEnumerator normalMovingSwitch()
    {
        yield return new WaitForSecondsRealtime(4.5f);
        if(normalSpawnIndex > normalSpawner.Length - 1)
            normalSpawnIndex = 0;
        movingFloor[normalSpawnIndex].flip = !movingFloor[normalSpawnIndex].flip;

        StartCoroutine(normalMovingSwitch());    
    }
}

