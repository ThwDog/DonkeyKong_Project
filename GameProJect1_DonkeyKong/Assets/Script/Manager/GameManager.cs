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

    private void Update() 
    {
        topScoreChange();
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
        if(score > topScore)
        {
            topScore = score;
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
}
