using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadLine : MonoBehaviour
{
    public enum _lineType
    {
        WinLine = 1, DeadLine = 2 , PlayerDeadLine = 3
    }

    public _lineType LineType;
    private int lineTypeNum;
    [SerializeField] float timeStay = 2;

    private void Start()
    {
        lineTypeNum = (int)LineType;
        //Debug.Log($"Line type is {lineTypeNum} which it type of {LineType}");
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(lineTypeNum == 2)
            destroyObjLine(other);
        else if(lineTypeNum == 1)
            winLine(other);
        else if(lineTypeNum == 3)
            playerDead(other);
    }

    private void playerDead(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            PlayerControl player = other.GetComponent<PlayerControl>();
            player.takeDamage();
        } 
    }

    private void destroyObjLine(Collider other)
    {
        if(other.gameObject.layer != 8 || other.gameObject.layer != 6 || other.gameObject.layer != 10)
        {
            Debug.Log("Destroy " + other.name);
            Destroy(other.gameObject);   
        } 
    }

    private void winLine(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Win");
            StartCoroutine(playWinAnimation());
        } 
    }

    IEnumerator playWinAnimation()
    {
        //play animation
        yield return new WaitForSeconds(timeStay);
        GameManager.instance.state = GameManager._state.win;
    }
}
