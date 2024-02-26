using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHolder : MonoBehaviour
{
    public ItemScriptable _Item;

    private void Awake() 
    {
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
            StartCoroutine(destroy());

        }
        else if(_Item.type == ItemScriptable.Type.weapon)
        {
            //add weapon to player
            Debug.Log("have weapon");
            StartCoroutine(destroy());
        }
    }

    IEnumerator destroy()
    {
        yield return new WaitForSeconds(0.2f);
        Destroy(this.gameObject);
    }

}

