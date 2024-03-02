using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using System;

public class UIManager : MonoBehaviour
{
    [Header("Game UI")]
    [SerializeField] TMP_Text score_Text;
    [SerializeField] TMP_Text topScore_Text;
    [SerializeField] TMP_Text bonusScore_Text;
    [SerializeField] TMP_Text level_Text;
    [SerializeField] TMP_Text lp;//just for show live point
    ScoreManager scoreManager;
    [Header("Main Menu UI")]
    [SerializeField] GameObject startBlinkingText;
    [SerializeField] TMP_Text topFiveScoreRank_Text;
    [SerializeField][Range(0,100)] float blinkingTime;
    public bool canUpdateText = true;

    private void Start() 
    {
        if(GameObject.Find("ScoreManager") != null)
            scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>(); 
        if(startBlinkingText)
            StartCoroutine(textBlinking(blinkingTime));
        
    }

    private void Update() 
    {
        updateText();
        if(topFiveScoreRank_Text)
            showTopFive();
    }

    private void showTopFive()
    {
        topFiveScoreRank_Text.text = "";

        int rank = 1;
        string rankDis = "";
        string script = "";
        int inList = 0;
        if(SaveAndLoadScore.instance.TopScoreSortList.Count.Equals(0))
            return;
            
        if(SaveAndLoadScore.instance.TopScoreSortList.Count < 5)
            inList = SaveAndLoadScore.instance.TopScoreSortList.Count;
        else inList = 5;

        for(int i = 0;i < inList;i++)
        {
            switch(rank)
            {
                case 1:
                    rankDis = "1st";
                    break;
                case 2:
                    rankDis = "2nd";
                    break;
                case 3:
                    rankDis = "3rd";
                    break;
                case 4:
                    rankDis = "4th";
                    break;
                case 5:
                    rankDis = "5th";
                    break;
            }
            script += $"{rankDis} {SaveAndLoadScore.instance.TopScoreSortList[i]} \n" ;
            rank++;
        }
        topFiveScoreRank_Text.text = script;
    }

    int checkDiff()
    {
        var _diff = GameManager.instance.difficulty;
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

        if(FindObjectOfType<CollectItem_Player>() && canUpdateText)
        {
            CollectItem_Player player = FindObjectOfType<CollectItem_Player>();
            int scoreSum = GameManager.instance.score + player.playerScore;
            score_Text.text = scoreSum.ToString();
        }
        else if(!canUpdateText)
        {
            score_Text.text = GameManager.instance.score.ToString();
        }
        else
            score_Text.text = GameManager.instance.score.ToString();

        topScore_Text.text = "HIGH SCORE \n" + GameManager.instance.topScore.ToString();
        if(bonusScore_Text != null)
            bonusScore_Text.text = scoreManager._CurrentBonusScore.ToString();
        
        level_Text.text = level.ToString();
        lp.text = GameManager.instance._LP.ToString();
    }

    IEnumerator textBlinking(float time)
    {
        while(startBlinkingText)
        {
            startBlinkingText.SetActive(false);
            yield return new WaitForSeconds(time);
            startBlinkingText.SetActive(true);
            yield return new WaitForSeconds(time);
        }
    }
}
