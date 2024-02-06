using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private GameManager gameManager;
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


    private void Awake() 
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();    
        bonusScoreSet();
        startDecread = true;

    }
    

    private void Update() 
    {
        //finish();
        if(startDecread & gameManager.state == GameManager._state.playing)
        {
            StartCoroutine(rateOfLoss());
        }
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
        if(gameManager.state == GameManager._state.win && !hasSentScore)
        {
            gameManager.score += _CurrentBonusScore;
            hasSentScore = true;
        }
    }

    public void bonusScoreSet()
    {
        var _diff = gameManager.difficulty;

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
