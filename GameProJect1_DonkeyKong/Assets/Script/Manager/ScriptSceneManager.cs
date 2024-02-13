using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScriptSceneManager : SingletonClass<ScriptSceneManager>
{

    public enum scene
    {
        cutScene = 0,
        one  = 1, 
        two  = 2, 
        three = 3, 
        four = 4
    }

    public scene _scene;
    private string nextScene;
    private bool passLvlFour = false;

    public override void Awake() 
    { 
        base.Awake();
    }

    private void Start() 
    {
        checkCurrentScene();
    }

    private void Update() 
    {
        checkCurrentScene();

        if(GameManager.instance.state == GameManager._state.win)
        {
            if(GameObject.Find("ScoreManager") != null)
            {
                ScoreManager score = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
                score.finish();
            }
            if(_scene == scene.cutScene)
            {
                SceneManager.LoadScene("Level" + nextScene);    
                GameManager.instance.state = GameManager._state.playing;
                checkNextScene();
            }
            else
            {
                checkNextScene();
                SceneManager.LoadScene("CutScene");
                GameManager.instance.state = GameManager._state.playing;
            }
            //Debug.Log("Win");
        }
        
    }

    void checkNextScene()
    {
        var currentScene = _scene;

        switch (currentScene)
        {
            case scene.one:
                if(GameManager.instance.difficulty == GameManager.diff.one)//diff is one
                    nextScene = "04";
                if(GameManager.instance.difficulty == GameManager.diff.two)//diff is two
                    nextScene = "02";
                if(GameManager.instance.difficulty == GameManager.diff.three)//diff is three
                    nextScene = "03";
                if(GameManager.instance.difficulty == GameManager.diff.four && !passLvlFour)//diff is four
                    nextScene = "03";
                else if(GameManager.instance.difficulty == GameManager.diff.four && passLvlFour) 
                    nextScene = "02";
                break;
            case scene.two:
                if(GameManager.instance.difficulty == GameManager.diff.two)//diff is two
                    nextScene = "04";
                if(GameManager.instance.difficulty == GameManager.diff.three)//diff is three
                    nextScene = "04";
                if(GameManager.instance.difficulty == GameManager.diff.four)//diff is four
                    nextScene = "04";
                break;
            case scene.three:
                if(GameManager.instance.difficulty == GameManager.diff.three)//diff is three
                    nextScene = "02";
                if(GameManager.instance.difficulty == GameManager.diff.four)//diff is four
                {
                    passLvlFour = true;
                    nextScene = "01";
                }
                break;
            case scene.four:
                checkNextDiff();
                passLvlFour = false;
                nextScene = "01";
                break;
            default:
                break;
        }
    }

    void checkCurrentScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();

        if(currentScene.name == "Level01")
        {
            _scene = scene.one;
        }
        else if(currentScene.name == "Level02")
        {
            _scene = scene.two;
        }
        else if(currentScene.name == "Level03")
        {
            _scene = scene.three;
        }
        else if(currentScene.name == "Level04")
        {
            _scene = scene.four;
        }
        else if(currentScene.name == "CutScene")
        {
            _scene = scene.cutScene;
        }
    }

    void checkNextDiff()
    {
        var currentDiff = GameManager.instance.difficulty;

        switch (currentDiff)
        {
            case GameManager.diff.one:
                GameManager.instance.difficulty = GameManager.diff.two;
                break;
            case GameManager.diff.two:
                GameManager.instance.difficulty = GameManager.diff.three;
                break;
            case GameManager.diff.three:
                GameManager.instance.difficulty = GameManager.diff.four;
                break;
            case GameManager.diff.four:
                break;
            default:
                break;
        }
    }
}
