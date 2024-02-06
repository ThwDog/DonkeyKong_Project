using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveingFloor : MonoBehaviour
{
    public enum direction
    {
        left = 1, right = 0
    }

    public direction Direction;
    [SerializeField] float pushForce;

    public void pushPlayer(GameObject obj)
    {
        CharacterController player = obj.GetComponent<CharacterController>();
        Vector3 dir = Direction == direction.right? Vector3.right : Vector3.left;
        player.Move(dir * pushForce * Time.fixedDeltaTime);
    }

    
}
