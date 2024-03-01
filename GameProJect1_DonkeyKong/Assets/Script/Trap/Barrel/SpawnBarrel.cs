using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBarrel : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField] List<GameObject> barrelType;
    [SerializeField][Range(1,10)] float spawnRate;
    [SerializeField][Range(1,10)] float animationStartDelay;
    bool canSpawn = true;
    bool startSpawner;
    private float rndNum;
    private int nextBarrelNum = 1;
    Coroutine spawner;
    private PlayerControl player;

    private void OnEnable() 
    {
        EventsBus.Subscribe(GameManager._state.playing,startSpawn);    
 
    }

    private void OnDisable() 
    {
        EventsBus.UnSubscribe(GameManager._state.playing,startSpawn);    
    }

    public void startSpawn()
    {
        //fix start time 
        Invoke("startFirstSpawn",animationStartDelay);
        //StartCoroutine(startSpawnDelay());
    }

    void startFirstSpawn()
    {
        Instantiate(barrelType[0],transform.position,barrelType[0].transform.rotation);
        StartCoroutine(delay());
    }

    IEnumerator delay()
    {
        yield return new WaitForSeconds(spawnRate);
        startSpawner = true;
        yield break;
    }

    private void Start() 
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
    }

    private void Update() 
    {
        if(canSpawn && startSpawner)
            spawner = StartCoroutine(delaySpawn(barrelType[nextBarrelNum]));
        else if(!startSpawner || player.isDead)
        {
            if(spawner != null) 
                StopCoroutine(spawner);
            else return;
        }
        nextBarrel();
    }

    IEnumerator delaySpawn(GameObject barrel)
    {
        //Debug.Log(barrel.name);
        Instantiate(barrel,transform.position,barrel.transform.rotation);
        rndNum = Random.Range(0f,1f);
        canSpawn = false;
        yield return new WaitForSeconds(spawnRate);
        canSpawn = true;
    }

    void nextBarrel()
    {
        if(rndNum > 0.9)
        {
            nextBarrelNum = 3;
        }
        else if(rndNum > 0.2)
        {
            nextBarrelNum = 2;
        }
        else if(rndNum > 0.1)
        {
            nextBarrelNum = 1;
        }

    }
}
