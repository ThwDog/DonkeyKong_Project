using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayAnimationByList : MonoBehaviour
{
    public enum _type
    {
        Pauline, Kong , Other
    }
    public _type type;

    public AnimationList[] animationList; // play animation that in list
    private Animator anim;

    public bool canPlay = true;
    [Header("Setting For Kong")]
    [SerializeField] float speed;
    private int indexList = 0;
    [SerializeField] bool canMoveTo;
    [Header("For kong LVL 3")]
    bool objHasMove = false;
    [SerializeField] GameObject target;
    [SerializeField] float moveToSpeed;
    public bool canMoveToward;


    void Start()
    {
        anim = GetComponent<Animator>();
        StartCoroutine(playAnimation());
    }

    private void Update()
    {
        // if(canMoveTo && GameManager.instance.state == GameManager._state.win)
        // {
        //     Vector3 _target = new Vector3(target.transform.position.x,0,0);

        //     transform.position = Vector3.MoveTowards(transform.position,_target,1000);
        // }
    }

    IEnumerator playAnimation()
    {
        if (!canPlay)
            yield break;
        if (indexList >= animationList.Length)
            indexList = 0;

        anim.SetTrigger(animationList[indexList].AnimationName);
        yield return new WaitForSeconds(animationList[indexList].howLongToPlay);
        indexList++;
        StartCoroutine(playAnimation());
    }

    private void OnCollisionStay(Collision other) // if in lvl 3
    {
        if (type == _type.Kong)
        {
            if (other.gameObject.GetComponent<MoveingFloor>())
            {
                MoveingFloor moveingFloor = other.gameObject.GetComponent<MoveingFloor>();
                //Debug.Log("Move");
                moveingFloor.pushObj(gameObject, speed);
            }
        }
    }

    public void PlayWinAnimation(string animationName)// use in time Line
    {
        anim.SetTrigger(animationName);
    }

    public void setAnimationToZero()
    {
        anim.speed = 0;
    }

    public void setAnimationToOne()
    {
        anim.speed = 1;
    }

    public void moveToTarget()
    {
        if(!objHasMove)
        {
            StartCoroutine(_moveToTarget());
        }
        // Vector3 _target = new Vector3(target.gameObject.transform.position.x, transform.position.y, transform.position.z);

        // while (transform.position != _target)
        // {
        //     Debug.Log("Move to");
        //     transform.position = Vector3.MoveTowards(transform.position, _target, moveToSpeed * Time.deltaTime);
        // }
        // transform.position = _target;
    }

    IEnumerator _moveToTarget()
    {
        Vector3 _target = new Vector3(target.gameObject.transform.position.x, transform.position.y, transform.position.z);
        objHasMove = true;
        while (transform.position != _target)
        {
            Debug.Log("Move to");
            transform.position = Vector3.MoveTowards(transform.position, _target, moveToSpeed * Time.deltaTime);
            yield return null;
        }
    }

}
