using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringScript : MonoBehaviour
{
    Animator anim;
    [Tooltip("For spring bounce animation")]
    [SerializeField] Animator springAnim; // for spring bounce animation
    [Header("")]
    [Range(-10,10)]public float animationSpeed;
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
            PlayerControl player = other.GetComponent<PlayerControl>();
            player.takeDamage();
            Debug.Log("Hit");
        }    
    }

    public void playVFXHitFloor()
    {
        SoundManager.instance.PlaySfx("SpringJump");
    }

    public void playVFXfall()
    {
        SoundManager.instance.PlaySfx("SpringFall");
    }

    public void BounceTriggerAnim()
    {
        springAnim.SetTrigger("Bounce");
    }
}
