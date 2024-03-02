using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIPopUpScore : MonoBehaviour
{
    [SerializeField] UiTranformToTarget popupText;

    public void Popup(Transform pos,Vector3 offset,int score,float destroyTime)
    {
        if(FindAnyObjectByType<UiTranformToTarget>())
        {
            UiTranformToTarget _popUpUi = FindAnyObjectByType<UiTranformToTarget>();
            if(_popUpUi.gameObject.activeSelf == false)
            {
                _popUpUi.gameObject.SetActive(true);
                TMP_Text _Text = _popUpUi.GetComponent<TMP_Text>();

                _Text.text = score.ToString();
                _popUpUi.set(pos,offset);
                StartCoroutine(destroyObj(destroyTime,_popUpUi.gameObject));
            }
        }
        else 
        {
            UiTranformToTarget popUpUi = Instantiate(popupText);
            TMP_Text _text = popUpUi.GetComponent<TMP_Text>();

            popUpUi.transform.SetParent(gameObject.transform);
            popUpUi.gameObject.SetActive(true);

            _text.text = score.ToString();
            popUpUi.set(pos,offset);
            StartCoroutine(destroyObj(destroyTime,popUpUi.gameObject));
        }
    }

    IEnumerator destroyObj(float destroyTime,GameObject ui)
    {
        yield return new WaitForSeconds(destroyTime);
        //Destroy(ui);
        if(ui.activeSelf)
            ui.SetActive(false);
        yield break;
    }

    
}
