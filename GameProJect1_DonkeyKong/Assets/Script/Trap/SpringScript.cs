using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringScript : MonoBehaviour
{
    Animator anim;
    [Range(1,10)]public float animationSpeed;
    [SerializeField]private bool _activate;
    public bool activate
    {
        get{return _activate;}
        set
        { 
            _activate = value;

        }
    }

    private void Awake() 
    {
        anim =  GetComponent<Animator>();
    }

    private void Update() 
    {
        anim.SetBool("Start",activate);
        anim.speed = animationSpeed;
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("Hit");
        }    
    }
}
