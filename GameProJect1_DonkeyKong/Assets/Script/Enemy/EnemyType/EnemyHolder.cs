using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHolder : MonoBehaviour , IDamageable
{
    [SerializeField] Scriptable_EnemyType _EnemyType;
    private UIPopUpScore popUpScoreUI;

    public int score;//For Test

    void Start()
    {
        popUpScoreUI = FindAnyObjectByType<UIPopUpScore>();
        score = _EnemyType.setScore(GameManager.instance);
    }

    public void takeDamage()
    {
        //Play vfx
        // play score ui
        popUpScoreUI.Popup(gameObject.transform,Vector3.zero,score,1f);
        Destroy(gameObject);
    }

    public void checkType(FollowNavigation follow)//set enemy type
    {
        var type = _EnemyType.type;
        
        switch(type)
        {
            case Scriptable_EnemyType.Type.typeOne:
                follow.enemyType = FollowNavigation._enemyType.FireBall;
                break;
            case Scriptable_EnemyType.Type.typeTwo:
                follow.enemyType = FollowNavigation._enemyType.FireBallType_2;
                break;
            case Scriptable_EnemyType.Type.typeThree:
                follow.enemyType = FollowNavigation._enemyType.FireBallType_3;
                break;
        }
    }
}
