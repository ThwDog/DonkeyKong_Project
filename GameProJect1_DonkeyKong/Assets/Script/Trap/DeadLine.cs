using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadLine : MonoBehaviour
{
    public enum _lineType
    {
        WinLine = 1, DeadLine = 2, PlayerDeadLine = 3
    }

    bool playerHasTouch = false;
    public _lineType LineType;
    private int lineTypeNum;
    [SerializeField] float timeStay = 2;

    [Header("For LVL 3")]
    [SerializeField] PlayAnimationByList playAnimation;
    private void Start()
    {
        lineTypeNum = (int)LineType;
        //Debug.Log($"Line type is {lineTypeNum} which it type of {LineType}");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (lineTypeNum == 2)
            destroyObjLine(other);
        else if (lineTypeNum == 1)
            winLine(other);
        else if (lineTypeNum == 3 && !playerHasTouch)
            playerDead(other);
    }

    private void playerDead(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerControl player = other.GetComponent<PlayerControl>();
            player.takeDamage();
            playerHasTouch = !playerHasTouch;
        }
    }

    private void destroyObjLine(Collider other)
    {
        Debug.Log("Destroy " + other.name);
        Destroy(other.gameObject);
    }

    private void winLine(Collider other) // by touch
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Win");
            
            if (playAnimation != null)
            {
                Rigidbody rb = playAnimation.gameObject.GetComponent<Rigidbody>();

                if (playAnimation.canMoveToward)
                {
                    // Debug.Log("00");
                    rb.useGravity = false;
                    rb.isKinematic = true;
                    playAnimation.moveToTarget();
                }
            }

            StartCoroutine(playWinAnimation());
        }
    }

    public IEnumerator playWinAnimation()
    {
        //play animation
        yield return new WaitForSeconds(timeStay);//wait for how long to win
        GameManager.instance.state = GameManager._state.win;
    }
}
