using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectItem_Player : MonoBehaviour
{
    [SerializeField]List<GameObject> weapon;
    public bool haveWeapon;
    [SerializeField]float holdSec;//How long player can hold weapon
    private Controller control;
    private PlayerControl player;

    [SerializeField] GameObject hammer;//Just For show Pls Delete 

    private void Start() 
    {
        control = GetComponent<Controller>();    
        player = GetComponent<PlayerControl>();    
    }

    private void Update() 
    {
        havingWeapon();
        StartCoroutine(_HolderWeapon(holdSec));
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Item"))
        {
            ItemHolder item = other.GetComponent<ItemHolder>();
            if(item._Item.type == ItemScriptable.Type.weapon && !haveWeapon)
            {
                weapon.Add(item._Item.obj);
                haveWeapon = true;
                hammer.SetActive(true);//Just For show Pls Delete 
                item.checkType();
            }
            else if(item._Item.type == ItemScriptable.Type.collectScore)
            {
                item.checkType();
            }
        }
    }

    IEnumerator _HolderWeapon(float holdSec)
    {
        if(haveWeapon)
        {
            yield return new WaitForSeconds(holdSec);
            hammer.SetActive(false);//Just For show Pls Delete 
            haveWeapon = false;
            weapon.Clear();
        }
    }

    public void havingWeapon()
    {
        if(haveWeapon)
            player.canClimbing = false;
        else player.canClimbing = true;
    }
}
