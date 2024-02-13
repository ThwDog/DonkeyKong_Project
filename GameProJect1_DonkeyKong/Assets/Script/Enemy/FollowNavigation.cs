using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.AI;

public class FollowNavigation : MonoBehaviour
{
    NavMeshAgent enemy;
    Transform target;
    [SerializeField][Range(0,100)] float chaseSpeed;
    private bool hitPlayer = false;
    private bool enableChasing = true;

    
    void Start()
    {
        enemy = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();    
    }

    private void Update() 
    {
        chasingTarget(target);
    }

    public void chasingTarget(Transform target)
    {
        enemy.speed = chaseSpeed;
        if(enableChasing && !hitPlayer)
            enemy.SetDestination(target.position);
    }


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
