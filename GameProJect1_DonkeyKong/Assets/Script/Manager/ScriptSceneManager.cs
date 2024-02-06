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
                nextScene = "02";
                break;
            case scene.two:
                nextScene = "03";
                break;
            case scene.three:
                nextScene = "04";
                break;
            case scene.four:
                checkNextDiff();
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
            //Debug.Log("Scene One");
            _scene = scene.one;
        }
        else if(currentScene.name == "Level02")
        {
            //Debug.Log("Scene two");
            _scene = scene.two;
        }
        else if(currentScene.name == "Level03")
        {
            //Debug.Log("Scene Three");
            _scene = scene.three;
        }
        else if(currentScene.name == "Level04")
        {
            //Debug.Log("Scene four");
            _scene = scene.four;
        }
        else if(currentScene.name == "CutScene")
        {
            //Debug.Log("Scene four");
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
