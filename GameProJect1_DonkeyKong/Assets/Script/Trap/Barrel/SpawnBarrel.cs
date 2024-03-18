using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBarrel : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField] List<GameObject> barrelType;
    [SerializeField][Range(1f,10f)] float spawnRate;
    [SerializeField][Range(0f,10f)] float animationSpawnDelay;
    [SerializeField] Transform spawnNormalPosition , spawnStartPosition;
    [Header("Show Barrel model")]
    [SerializeField] private GameObject brownBarrel , blueBarrel; // use for set active when play animation
    bool canSpawn = true;
    bool startSpawner;
    private float rndNum;
    private int nextBarrelNum = 1;
    Coroutine spawner;
    private PlayerControl player;
    private Animator anim;

    private void OnEnable() 
    {
        anim = GetComponentInChildren<Animator>();
        EventsBus.Subscribe(GameManager._state.playing,startSpawn);    
 
    }

    private void OnDisable() 
    {
        EventsBus.UnSubscribe(GameManager._state.playing,startSpawn);    
    }

    public void startSpawn()
    {
        //fix start time 
        anim.SetTrigger("Barrel01");
        Invoke("activeOpenBarrel",0.2f);
        Invoke("startFirstSpawn",0.5f);
    }

    void activeOpenBarrel()
    {
        blueBarrel.SetActive(true);
    }

    void startFirstSpawn()
    {
        //kongAnimation(nextBarrelNum); //add animation
        blueBarrel.SetActive(false);
        Instantiate(barrelType[0],spawnStartPosition.position,barrelType[0].transform.rotation);
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
        {
            anim.speed = 0.95f;

            kongAnimation(nextBarrelNum); //add animation
            brownBarrel.SetActive(true);

            spawner = StartCoroutine(delaySpawn(barrelType[nextBarrelNum]));
        }
        else if(!startSpawner || player.isDead || GameManager.instance.state != GameManager._state.playing)
        {
            if(spawner != null) 
                StopCoroutine(spawner);
            else return;
        }
        nextBarrel();
    }

    IEnumerator delaySpawn(GameObject barrel)
    {
        // add animation
        canSpawn = false;
        yield return new WaitForSeconds(animationSpawnDelay); // delay between animation and spawn

        brownBarrel.SetActive(false);

        Instantiate(barrel,spawnNormalPosition.position,barrel.transform.rotation);
        rndNum = Random.Range(0f,1f);
        yield return new WaitForSecondsRealtime(spawnRate);
        anim.speed = 1f;
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
    
    // use switch for select animation
    void kongAnimation(int barrelType)
    {
        switch(barrelType)
        {
            case 1:
                anim.SetTrigger("Barrel02");
                // Debug.Log("Play animation 1");
                break;
            case 2:
                anim.SetTrigger("Barrel02");
                // Debug.Log("Play animation 2");
                break;
            case 3:
                anim.SetTrigger("Barrel01");
                // Debug.Log("Play animation 3");
                break;
            default:
                break;
        }
    }
}
