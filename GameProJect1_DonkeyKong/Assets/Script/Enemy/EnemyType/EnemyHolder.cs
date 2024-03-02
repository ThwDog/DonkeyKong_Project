using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHolder : MonoBehaviour , IDamageable
{
    [SerializeField] Scriptable_EnemyType _EnemyType;
    private UIPopUpScore popUpScoreUI;

    public int score = 100;//For Test

    void Start()
    {
        popUpScoreUI = FindAnyObjectByType<UIPopUpScore>();
    }

    public void takeDamage()
    {
        //Play vfx
        // play score ui
        popUpScoreUI.Popup(gameObject.transform,Vector3.zero,score,1f);
        Destroy(gameObject);
    }
}
