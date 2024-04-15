using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiTranformToTarget : MonoBehaviour
{
    internal bool hasUser = false;
    private Camera cam;

    private void Start() 
    {
    }

    void Update()
    {

             
    }

    public void set(Transform target ,Vector3 offset)
    {
        cam = Camera.main;    

        Vector3 pos = cam.WorldToScreenPoint(target.position + offset);

        if(transform.position != pos)
            transform.position = pos;   
    }
}
