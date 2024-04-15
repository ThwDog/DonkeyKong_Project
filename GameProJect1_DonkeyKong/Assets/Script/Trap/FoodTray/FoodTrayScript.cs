using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodTrayScript : MonoBehaviour, IDamageable
{
    [SerializeField] float speed;
    public int score = 800;
    private UIPopUpScore popUpScoreUI;

    private void Start() 
    {
        popUpScoreUI = FindAnyObjectByType<UIPopUpScore>();
    }

    private void OnCollisionStay(Collision other)
    {
        if(other.gameObject.GetComponent<MoveingFloor>())
        {
            MoveingFloor moveingFloor = other.gameObject.GetComponent<MoveingFloor>();
            //Debug.Log("Move");
            moveingFloor.pushObj(gameObject,speed);
        }    
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("Player");
            PlayerControl player = other.GetComponent<PlayerControl>();

            player.takeDamage();
            Debug.Log("Hit");
        } 
    }

    public void takeDamage()
    {
        popUpScoreUI.Popup(gameObject.transform,Vector3.zero,score,1f);
        Destroy(gameObject);
    }
}
