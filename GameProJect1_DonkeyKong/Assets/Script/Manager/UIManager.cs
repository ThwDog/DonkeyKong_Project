using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class UIManager : MonoBehaviour
{
    [Header("Game UI")]
    [SerializeField] TMP_Text score_Text;
    [SerializeField] TMP_Text topScore_Text;
    [SerializeField] TMP_Text bonusScore_Text;
    [SerializeField] TMP_Text level_Text;
    [SerializeField] TMP_Text lp;//just for show live point
    GameManager gm;
    ScoreManager scoreManager;

    private void Awake()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>(); 
        
    }

    private void Start() 
    {
        if(GameObject.Find("ScoreManager") != null)
            scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>(); 
    }

    private void Update() 
    {
        updateText();
    }

    int checkDiff()
    {
        var _diff = gm.difficulty;
        int level;

        switch(_diff)
        {
            case GameManager.diff.one: 
                level = 1;
                return level;
            case GameManager.diff.two:
                level = 2;
                return level;
            case GameManager.diff.three:
                level = 3;
                return level;
            case GameManager.diff.four:
                level = 4;
                return level;
            default:
                Debug.Log("Error");
                break;
        }
        return 0;
    }

    void updateText()
    {
        int level = checkDiff();

        score_Text.text = gm.score.ToString();    
        topScore_Text.text = "HIGH SCORE \n" + gm.topScore.ToString();
        if(bonusScore_Text != null)
            bonusScore_Text.text = scoreManager._CurrentBonusScore.ToString();
        
        level_Text.text = level.ToString();
        lp.text = gm._LP.ToString();
    }
}
