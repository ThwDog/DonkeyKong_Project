using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class FollowNavigation : MonoBehaviour
{
    public enum _enemyType
    {
        FireBall   //normal fire ball
        , FireBallType_2 // fire ball that know dir
        ,FireBallType_3 // fire ball that need to set dir or fire duck
    }
    public _enemyType enemyType;
    NavMeshAgent agent;
    [SerializeField][Range(0,100)] float chaseSpeed;
    private bool hitPlayer = false;
    private bool enableChasing = true;
    Transform target;

    [Header("FireDuckSetting")]
    [SerializeField] List<Transform> wayPoint; //for collected way point
    private int preRandomIndex; //for collect previous random index 
    int wayPointIndex;
    float timeFindWayLimit;
    Vector3 waypointTarget;

    private void Awake() 
    {
        EnemyHolder enemy = GetComponent<EnemyHolder>();
        enemy.checkType(this);
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();   
        if(enemyType == _enemyType.FireBallType_3)
        {
            getDir();
            randomWaypointIndex();
            chasingWayPointTarget(); 
        }
        else if(enemyType == _enemyType.FireBallType_2)
        {
            chasingWayPointTarget(); 
        }
    }

    private void Update() 
    {
        if(enemyType == _enemyType.FireBall)
            fireBallChasingTarget(target);
        else if(enemyType == _enemyType.FireBallType_2)
        {
            if(Vector3.Distance(transform.position,waypointTarget) < 2f)
            {
                //waypointTarget = wayPoint[wayPointIndex].position; //find next waypoint with waypointIndex
                iterateWaypointIndex();
                chasingWayPointTarget(); 
            }
        }
        else if(enemyType == _enemyType.FireBallType_3)
        {
            if(Vector3.Distance(transform.position,waypointTarget) < 2f)
            {
                randomWaypointIndex();
                chasingWayPointTarget(); 
            }
        }
        
    }

    public void fireBallChasingTarget(Transform target)
    {
        agent.speed = chaseSpeed;
        if(enableChasing && !hitPlayer)
            agent.SetDestination(target.position);
    }

    #region Fire Duck and fine type 2 Code

    void chasingWayPointTarget()
    {
        waypointTarget = wayPoint[wayPointIndex].position; //find next waypoint with waypointIndex
        agent.speed = chaseSpeed;
        agent.SetDestination(waypointTarget);

        timeFindWayLimit += Time.deltaTime; // if enemy cant reach to way point in 15min then delete that way point
        //Debug.Log(timeFindWayLimit);
        if(timeFindWayLimit >= 15f)
            deleteWay();
    }

    private void iterateWaypointIndex()//for enemy that really know dir 
    {
        wayPointIndex++;
        if(wayPointIndex == wayPoint.Count)
            wayPointIndex = 0;
    }   

    private void randomWaypointIndex() // for enemy that need to random waypoint 
    {
        int index = Random.Range(0,wayPoint.Count - 1);

        if(preRandomIndex == index)
            return;

        preRandomIndex = index;
        wayPointIndex = index;
    }

    //if enemy can reach to way point in limit time remove current way point 
    public void deleteWay()
    {
        timeFindWayLimit = 0;
        wayPoint.Remove(wayPoint[wayPointIndex]);
        Debug.Log("Delete Way");
    }

    void getDir()//for fire Duck
    {
        GameObject[] obj = GameObject.FindGameObjectsWithTag("SetDir"); 

        foreach(var dir in obj)
        {
            wayPoint.Add(dir.transform);
        }

    }

    #endregion

    private void OnCollisionEnter(Collision other) 
    {
        if(other.gameObject.CompareTag("Player") && !hitPlayer)
        {
            hitPlayer = true;
            enableChasing = false; 
            Debug.Log("Enemy atk Player");
            PlayerControl player = other.gameObject.GetComponent<PlayerControl>();
            player.takeDamage();
        }    
    }

}
