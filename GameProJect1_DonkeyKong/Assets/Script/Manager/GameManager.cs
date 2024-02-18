using System.Collections;
using System.Collections.Generic;
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
        loseGame(_LP);
        winGame(ScriptSceneManager.instance);
    }

    public override void Awake() 
    { 
        base.Awake();
        state = _state.playing;
        _reset();
    }

    public void _reset()
    {
        _LP = 3;
        score = 0;
        state = _state.playing;
    }

    public void topScoreChange()
    {
        int top = 0;

        if(SaveAndLoadScore.instance.saveScore != null)
        {
            if(score > topScore)
            {
                topScore = score;
            }
            foreach(var _topScore in SaveAndLoadScore.instance.saveScore)
            {
                if(top <  _topScore.score)
                    top = _topScore.score;
                if(score < top)
                {
                    topScore = top;
                }   
            }
        }
    }

    public void IncreaseScore(int value,string form)
    {
        score += value;
        Debug.Log($"Score Increase : {value} From {form}");
    }

    public void DecreaseScore(int value,string form)
    {
        score -= value;
        Debug.Log($"Score Decrease : {value} From {form}");
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
        if(sceneManager.win && sceneManager._scene == ScriptSceneManager.scene.cutScene)
        {
            sceneManager.win = false;
            
            SaveAndLoadScore.instance.addToList(score);
            Debug.Log("Win");
        }
    }
}
