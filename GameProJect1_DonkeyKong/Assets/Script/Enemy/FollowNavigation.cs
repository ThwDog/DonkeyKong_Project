using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.AI;

public class FollowNavigation : MonoBehaviour
{
    public enum _enemyType
    {
        FireBall , FireDuck
    }
    public _enemyType enemyType;
    NavMeshAgent agent;
    [SerializeField][Range(0,100)] float chaseSpeed;
    private bool hitPlayer = false;
    private bool enableChasing = true;
    Transform target;

    [Header("FireDuckSetting")]
    [SerializeField] List<Transform> wayPoint;
    int wayPointIndex;
    float timeFindWayLimit;
    Vector3 waypointTarget;

    
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();   
        if(enemyType == _enemyType.FireDuck)
            duckChasingTarget(); 
    }

    private void Update() 
    {
        if(enemyType == _enemyType.FireBall)
            fireBallChasingTarget(target);
        else if(enemyType == _enemyType.FireDuck)
        {
            waypointTarget = wayPoint[wayPointIndex].position; //find next waypoint with waypointIndex
            if(Vector3.Distance(transform.position,waypointTarget) < 1)
            {
                iterateWaypointIndex();
                duckChasingTarget(); 
            }
        }
    }

    public void fireBallChasingTarget(Transform target)
    {
        agent.speed = chaseSpeed;
        if(enableChasing && !hitPlayer)
            agent.SetDestination(target.position);
    }

    #region Fire Duck Code

    void duckChasingTarget()
    {
        agent.speed = chaseSpeed;
        agent.SetDestination(waypointTarget);
        timeFindWayLimit += Time.deltaTime;
        if(timeFindWayLimit >= 15f)
            deleteWay();
    }

    private void iterateWaypointIndex()
    {
        wayPointIndex++;
        if(wayPointIndex == wayPoint.Count)
            wayPointIndex = 0;
    }   
    //if enemy can reach to way point in limit time remove current way point 
    public void deleteWay()
    {
        timeFindWayLimit = 0;
        wayPoint.Remove(wayPoint[wayPointIndex]);
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
