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
    [Header("About anim and audio")]
    private bool canPlayMusic = true;
    Coroutine hammerHoldCoroutine;
    [SerializeField] GameObject hammer;//Just For show Pls Delete 
    [Header("")]
    public int playerScore = 0;

    private void Start() 
    {
        playerScore = 0;
        control = GetComponent<Controller>();    
        player = GetComponent<PlayerControl>();    
    }

    private void Update() 
    {
        hammerHoldCoroutine = StartCoroutine(_HolderWeapon(holdSec));
        player.anim.SetBool("HaveHammer",haveWeapon);
        if(Input.GetKeyDown(KeyCode.Z) && haveWeapon)
        {
            haveWeapon = false;
            SoundManager.instance.StopSfx("Hammer");
            StopCoroutine(hammerHoldCoroutine);
            resetBeforeHaveWeapon();
        }
        havingWeapon();
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

                item.checkType(this);
            }
            else if(item._Item.type == ItemScriptable.Type.collectScore)
            {
                item.checkType(this);
            }
        }
    }

    IEnumerator _HolderWeapon(float holdSec)
    {
        if(haveWeapon)
        {
            if(canPlayMusic)
            {
                SoundManager.instance.PlaySfx("Hammer");
                canPlayMusic = false;
            }
            control.releaseJumpKey();
            yield return new WaitForSeconds(holdSec);
            resetBeforeHaveWeapon();
        }
    }

    public void resetBeforeHaveWeapon()
    {
        control.gainJumpKey();
        hammer.SetActive(false);//Just For show Pls Delete 
        canPlayMusic = true;
        haveWeapon = false;
        weapon.Clear();
    }

    public void havingWeapon()
    {
        if(haveWeapon)
            player.canClimbing = false;
        else
            player.canClimbing = true;
    }

    public void IncreaseScore(int value,string form)
    {
        playerScore += value;
        Debug.Log($"Player Score Increase : {value} From {form}");
    }

}
