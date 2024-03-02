using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class BarrelHolder : MonoBehaviour , IDamageable
{
    [SerializeField] BarrelTypeScriptable barrelType;
    BarrelRollType rollType;
    [Header("Only for Cross Type")]
    [Tooltip("For cross barrel Type")]
    [SerializeField] private List<Transform> setDirection;
    private bool playerHaveCross = false; 
    public int score = 100;
    UIPopUpScore popUpScoreUI;


    private void Start() 
    {
        popUpScoreUI = FindAnyObjectByType<UIPopUpScore>();
        rollType = GetComponent<BarrelRollType>();
        getDir();
    }

    private void Update() 
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, Vector3.up);

        if (Physics.Raycast(ray,out hit,3))
            {
                if (hit.collider.gameObject.CompareTag("Player") && !playerHaveCross) 
                {
                    playerHaveCross = true;
                    CollectItem_Player player = hit.collider.GetComponent<CollectItem_Player>();
                    // instantiate ui score
                    popUpScoreUI.Popup(gameObject.transform,Vector3.zero,score,1f);
                    player.IncreaseScore(score,"Jump ON");          
                    //GameManager.instance.IncreaseScore(score,"Jump ON");
                }
            }   
    }

    private void FixedUpdate() 
    {
        checkBarrelType();
    }

    
    public void checkBarrelType()
    {
        var type = barrelType._type;
        switch (type)
        {
            case BarrelTypeScriptable.type.NormalType:
                rollType._NormalRoll(barrelType.speed);
                break;
            case BarrelTypeScriptable.type.OpeningType:
                rollType._Opening(barrelType.speed);
                break;
            case BarrelTypeScriptable.type.CrossType:
                rollType._CrossType(setDirection,barrelType.speed);
                break;
            case BarrelTypeScriptable.type.CanUseLadderType:
                rollType._RollingOnLadder(barrelType.speed);
                break;
        }
    }

    void getDir()
    {
        GameObject[] obj = GameObject.FindGameObjectsWithTag("SetDir"); 

        foreach(var dir in obj)
        {
            setDirection.Add(dir.transform);
        }
        //setDirection.AddRange(GameObject.FindGameObjectsWithTag("SetDir"));
    }

    private void OnDrawGizmos() 
    {
        if(barrelType._type == BarrelTypeScriptable.type.CanUseLadderType)
        {
            Gizmos.color = Color.red;
            //Use the same vars you use to draw your Overlap SPhere to draw your Wire Sphere.
            //Gizmos.DrawCube (gameObject.transform.position - new Vector3 (0,2,0), transform.localScale);
            Gizmos.DrawSphere(gameObject.transform.position - new Vector3 (0,3,0), 1);
        }
    }

    public void takeDamage()
    {
        //Play vfx
        // play score ui
        popUpScoreUI.Popup(gameObject.transform,Vector3.zero,score,1f);
        Destroy(gameObject);
    }
}
