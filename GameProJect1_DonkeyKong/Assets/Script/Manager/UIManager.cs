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
    // [SerializeField] TMP_Text lp;//just for show live point
    [SerializeField] GameObject[] lpArray; 
    ScoreManager scoreManager;
    [Header("Main Menu UI")]
    [SerializeField] GameObject startBlinkingText;
    [SerializeField] TMP_Text topFiveScoreRank_Text;
    [SerializeField][Range(0,100)] float blinkingTime;
    [Header("Cut Scene")]
    [SerializeField] GameObject[] kongUi; 
    public bool canUpdateText = true;
    bool isUIUpdate =false;

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
        if(kongUi.Length > 0 && !isUIUpdate)
        {
            ScriptSceneManager.instance.kongImgUi(kongUi);
            isUIUpdate = !isUIUpdate;
        }
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

        if(lpArray != null)
            imageLP();
        if(kongUi != null)
            imageKong();
        // lp.text = GameManager.instance._LP.ToString();
    }

    // check from scriptSceneManager and GameManager that what lvl and what lvlState then set image active
    private void imageKong()
    {
        // if player died then only open image by previousScene else active image by nextScene
        string scene = GameManager.instance.state == GameManager._state.lose? ScriptSceneManager.instance.previousScene : ScriptSceneManager.instance.nextScene;
        int sceneNum = Convert.ToInt32(scene);
        // Debug.Log(sceneNum);

        switch(GameManager.instance.difficulty)
        {
            case GameManager.diff.one:
                // for(int i = sceneNum;i < kongUi.Length;i++)
                //     kongUi[i].SetActive(true);
                break;
            case GameManager.diff.two:
                break;
            case GameManager.diff.three:
                break;
            case GameManager.diff.four:
                break;
        }
    }

    private void imageLP() //if player lose lp then image set active are false
    {
        if(GameManager.instance._LP < lpArray.Length)
            for(int i = GameManager.instance._LP;i < lpArray.Length;i++)
                lpArray[i].SetActive(false);
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
