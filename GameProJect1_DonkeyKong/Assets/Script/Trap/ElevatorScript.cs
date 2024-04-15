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
    [SerializeField] Vector3 spawnPosi;
    //public List<GameObject> padCount;

    bool hasSpawn = false;
    PlayerControl player;
    ElevatorBase _base;

    private void Awake() 
    {
        _base = GetComponentInChildren<ElevatorBase>();    
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
    }

    private void Update() 
    {
        if(_base.playerHasTouch || GameManager.instance.state != GameManager._state.playing)
        {
            start = false;
            player.takeDamage();
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
        //Vector3 _dis = new Vector3(1,1,0); 
        if(!hasSpawn)
        {
            // Vector3 pos = inst == _inst.down ? spawnPosi : -spawnPosi;

            ElevatorPadScript pad = Instantiate(padOBJ,spawner.transform.position + spawnPosi,padOBJ.gameObject.transform.rotation);
            

            pad.transform.parent = gameObject.transform;
            pad.speed = padSpeed;
            //pad.target = target.transform;

            pad.target = new Vector3(target.transform.position.x + spawnPosi.x,target.transform.position.y,target.transform.position.z);
            hasSpawn = true;
            //padCount.Add(pad.gameObject);
            yield return new WaitForSeconds(2.5f);
            hasSpawn = false;
        }

        yield return new WaitForSeconds(coolDown);
    } 

    private void setSpawnerPosition(GameObject start,GameObject end)
    {
        Vector3 _dis = new Vector3(0,dis,0); 
        if(inst == _inst.down)
            end.transform.position = start.transform.position + _dis; 
        else if(inst == _inst.top)
            end.transform.position = start.transform.position - _dis; 
    }

    private void OnDrawGizmos() 
    {
        Gizmos.color = Color.red;
        Vector3 _dis = new Vector3(0,dis,0); 
        if(inst == _inst.down)
            Gizmos.DrawLine(down.transform.position,down.transform.position + _dis);
        else if(inst == _inst.top)
            Gizmos.DrawLine(up.transform.position,up.transform.position - _dis);
    }

}
