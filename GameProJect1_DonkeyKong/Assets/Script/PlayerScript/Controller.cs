using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    protected static Controller _instance;
    public static Controller instance{ get{return _instance;}}
    [Header("Controller")]
    public bool blockController;
    public KeyCode jumpKey;

    protected Vector2 p_Movement;
    protected bool p_jump;

    public Vector2 movement
    {
        get
        {
            if(blockController)
                return Vector2.zero;
            return  p_Movement;
        }
    }

    public bool jump
    {
        get{return p_jump && !blockController;}
    }


    private void Awake() 
    {
        if(_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }    
        else _instance =this;
    }
    private void Update() 
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        
        p_jump = Input.GetKey(jumpKey);
        p_Movement = new Vector2(horizontal,vertical);
    }

    public void releaseController()
    {
        blockController = true;
    }

    public void gainController()
    {
        blockController = false;
    }
}
