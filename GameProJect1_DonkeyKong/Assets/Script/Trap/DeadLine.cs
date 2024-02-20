using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadLine : MonoBehaviour
{
    public enum _lineType
    {
        WinLine = 1, DeadLine = 2
    }

    public _lineType LineType;
    private int lineTypeNum;

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
            GameManager.instance.state = GameManager._state.win;
        } 
    }
}
