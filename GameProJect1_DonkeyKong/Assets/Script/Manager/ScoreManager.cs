using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [Header("ScoreManager")]
    [SerializeField]private int _DefaultBonusScore;
    public int _CurrentBonusScore
    {
        get{return _DefaultBonusScore;}
        set{_DefaultBonusScore = value;}
    }
    [SerializeField]private float decreadRateTime;//seconds
    [SerializeField]private int decreadRatePoint;//seconds
    public bool startDecread;
    private bool hasSentScore = false;

    private void OnEnable() 
    {
        EventsBus.Subscribe(GameManager._state.playing,decreadScoreByTime);    
    }

    private void OnDisable() 
    {
        EventsBus.UnSubscribe(GameManager._state.playing,decreadScoreByTime);    
    }


    private void Start() 
    {
        bonusScoreSet();
        startDecread = true;
        
        EventsBus.publish(GameManager._state.playing);
    }
    

    private void Update() 
    {
        //finish();
        if(startDecread & GameManager.instance.state == GameManager._state.playing)
        {
            StartCoroutine(rateOfLoss());
        }
    }

    public void decreadScoreByTime()
    {
        StartCoroutine(rateOfLoss());
    }

    IEnumerator rateOfLoss()
    {
        startDecread = false;
        yield return new WaitForSeconds(decreadRateTime);
        _CurrentBonusScore -= decreadRatePoint;
        //Debug.Log(_CurrentBonusScore);
        yield return new WaitForSeconds(decreadRateTime);
        startDecread = true;
    }

    public void finish()
    {
        if(GameManager.instance.state == GameManager._state.win && !hasSentScore)
        {
            GameManager.instance.score += _CurrentBonusScore;
            hasSentScore = true;
        }
    }

    public void bonusScoreSet()
    {
        var _diff = GameManager.instance.difficulty;

        switch(_diff)
        {
            case GameManager.diff.one: 
                _CurrentBonusScore = 5000;
                decreadRateTime = 2f;
                decreadRatePoint = 120;
                break;
            case GameManager.diff.two:
                _CurrentBonusScore = 6000;
                decreadRateTime = 1.667f;
                decreadRatePoint = 100;
                break;
            case GameManager.diff.three:
                _CurrentBonusScore = 7000;
                decreadRateTime = 1.33f;
                decreadRatePoint = 80;
                break;
            case GameManager.diff.four:
                _CurrentBonusScore = 8000;
                decreadRateTime = 1f;
                decreadRatePoint = 60;
                break;
            default:
                Debug.Log("Error");
                break;
        }
    }
}
