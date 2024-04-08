using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonClass<GameManager>
{
    public enum _state
    {
        win , lose , playing , waiting
    }

    public enum diff //difficulty
    {
        one , two , three , four 
    }

    public _state state;
    public diff difficulty;

    [Header("manager")]
    public int _LP = 3;
    public int score = 0;
    public int topScore = 0;
    public bool lose = false;

    private void Update() 
    {
        topScoreChange();
        winGame(ScriptSceneManager.instance);
        loseGame(_LP);
    }

    public override void Awake() 
    { 
        base.Awake();
        state = _state.playing;
        _reset();
    }

    public void _reset()
    {
        ScriptSceneManager.instance._CanPlayMusic = true;
        _LP = 3;
        score = 0;
        lose = false;
        state = _state.playing;
        difficulty = diff.one;
    }

    public void topScoreChange()
    {
        if(score > topScore)
            topScore = score;
        
        if(SaveAndLoadScore.instance.TopScoreSortList.Count.Equals(0))
            return;
        
        int top = SaveAndLoadScore.instance.TopScoreSortList[0];
        
        if(score < top) topScore = top;
        
    }

    public void IncreaseScore(int value)
    {
        score += value;
        Debug.Log($"Score Data Increase : {value}");
    }


    public void loseGame(int lp)
    {
        if(lp <= 0 && !lose)
        {
            lose = true;
            SaveAndLoadScore.instance.addToList(score);
            Debug.Log("lose");
        }
    }

    public void winGame(ScriptSceneManager sceneManager)
    {
        if(sceneManager.win && sceneManager._scene == ScriptSceneManager.scene.cutScene) // maybe when win go to main menu
        {
            if(state == _state.win)
            {
                SaveAndLoadScore.instance.addToList(score);
                Debug.Log("ID" + SaveAndLoadScore.instance.id +" : "+score);
                _reset();
                //ScriptSceneManager.instance.toMainMenu();
                Debug.Log("Win");
            }
        }
    }
}
