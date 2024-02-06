using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorScript : MonoBehaviour
{
    public enum _inst//select to instantiate top or down
    {
        top , down
    }
    public _inst inst;

    [Header("setting")]
    [SerializeField] GameObject up;
    [SerializeField] GameObject down;
    [SerializeField] ElevatorPadScript padOBJ;
    [SerializeField] int maxPad;
    [SerializeField] float coolDown;
    public bool start;
    [Tooltip("distance Of pad")]
    [SerializeField][Range(0,100)] float dis;//distance Form each pad
    [SerializeField] float padSpeed;
    //public List<GameObject> padCount;

    bool hasSpawn = false;
    PlayerControl player;
    ElevatorBase _base;
    GameManager gameManager;

    private void Awake() 
    {
        _base = GetComponentInChildren<ElevatorBase>();    
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void Update() 
    {
        if(_base.playerHasTouch || gameManager.state != GameManager._state.playing)
        {
            start = false;
            player.isDead = true;
        }
        else start = true;
        
    }



    private void FixedUpdate()
    {
        spawnpad();
    }

    void spawnpad()
    {
        
        if(inst == _inst.top)
        {
            setSpawnerPosition(up,down);
            if(start)
                StartCoroutine(spawn(up,down));
        }
        else if(inst == _inst.down)
        {
            setSpawnerPosition(down,up);
            if(start)
                StartCoroutine(spawn(down,up));
        }
        
    }

    private IEnumerator spawn(GameObject spawner,GameObject target)
    {

        Vector3 _dis = new Vector3(0,1,0); 
        if(!hasSpawn)
        {
            ElevatorPadScript pad = Instantiate(padOBJ,spawner.transform.position + _dis, Quaternion.identity);
            pad.transform.parent = gameObject.transform;
            pad.speed = padSpeed;
            pad.target = target.transform;
            hasSpawn = true;
            //padCount.Add(pad.gameObject);
            yield return new WaitForSeconds(1);
            hasSpawn = false;
        }

        yield return new WaitForSeconds(coolDown);
    } 

    private void setSpawnerPosition(GameObject start,GameObject end)
    {
        Vector3 _dis = new Vector3(0,dis,0); 
        end.transform.position = start.transform.position + _dis; 
    }

    private void OnDrawGizmos() 
    {
        Gizmos.color = Color.red;
        Vector3 _dis = new Vector3(0,dis,0); 
        Gizmos.DrawLine(down.transform.position,down.transform.position + _dis);
    }

}
