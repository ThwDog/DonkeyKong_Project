using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHolder : MonoBehaviour
{
    public ItemScriptable _Item;
    private UIPopUpScore popUpScoreUI;

    private void Start() 
    {
        popUpScoreUI = FindAnyObjectByType<UIPopUpScore>();
    }

    public void checkType(CollectItem_Player player)
    {
        if(_Item.type == ItemScriptable.Type.collectScore)
        {
            CollectaleItem_Score collectaleItem = (CollectaleItem_Score)_Item;
            collectaleItem.setScore(GameManager.instance);
            //add score to score manager
            //GameManager.instance.IncreaseScore(collectaleItem.score,$"Collect {collectaleItem._name}");
            player.IncreaseScore(collectaleItem.score,$"Collect {collectaleItem._name}");
            StartCoroutine(destroy(collectaleItem.score));

        }
        else if(_Item.type == ItemScriptable.Type.weapon)
        {
            //add weapon to player
            Debug.Log("have weapon");
            StartCoroutine(destroy(0));
        }
    }

    IEnumerator destroy(int score)
    {
        if(score != 0)
            popUpScoreUI.Popup(gameObject.transform,Vector3.zero,score,1f);
        yield return new WaitForSeconds(0.2f);
        gameObject.SetActive(false);
        //Destroy(this.gameObject);
    }

}

