using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class AttackPoint : MonoBehaviour
{
    bool right = true;
    CollectItem_Player collectItem;
    Controller controller;

    private void Awake() 
    {
        controller = GetComponentInParent<Controller>();    
        collectItem = GetComponentInParent<CollectItem_Player>();  
    }

    private void Update() 
    {
        filp();
        if(controller.movement.x > 0f)
            right = true;  
        else if(controller.movement.x < 0f) 
            right = false;
    }

    void filp()
    {
        if(right)
            transform.rotation = new Quaternion(0,0,0,0);
        else 
            transform.rotation = new Quaternion(0,180,0,0);
    }
    void OnTriggerEnter(Collider other)
    {
        if(collectItem.haveWeapon && other.CompareTag("Enemy"))
        {
            Debug.Log("Hit Enemy");
            if(other.gameObject.GetComponent<EnemyHolder>())
            {
                EnemyHolder enemy = other.GetComponent<EnemyHolder>();
                GameManager.instance.IncreaseScore(enemy.score,"Kill Enemy");
                Destroy(other.gameObject);
            }
            if(other.gameObject.GetComponent<BarrelHolder>())
            {
                BarrelHolder barrel = other.GetComponent<BarrelHolder>();
                GameManager.instance.IncreaseScore(barrel.score,"hit barrel");
                Destroy(other.gameObject);
            }
        }
    }
}