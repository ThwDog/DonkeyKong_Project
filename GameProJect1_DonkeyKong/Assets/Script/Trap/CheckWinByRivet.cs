using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckWinByRivet : DeadLine
{
    public int rivetBoomCount = 0;
    [SerializeField] int maxRivetBoom = 8;

    private void Update() 
    {
        if(rivetBoomCount == maxRivetBoom)
            StartCoroutine(playWinAnimation());
    }
}
