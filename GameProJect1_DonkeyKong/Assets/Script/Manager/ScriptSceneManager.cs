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
        four = 4,
        mainMenu = 5 
    }

    public scene _scene;
    private string nextScene;
    private bool passLvlFour = false;
    public bool win = false;

    [Header("About animation and music")]
    private bool canPlayMusic = true;

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
        playMusicOnScene();

        if(GameManager.instance.state == GameManager._state.win)
        {
            canPlayMusic = true;
            SoundManager.instance.StopAllMusic();
            if(GameObject.Find("ScoreManager") != null)
            {
                ScoreManager score = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
                score.finish();
            }
            // if(_scene == scene.cutScene && !win)
            // {   
            //     SceneManager.LoadScene("Level" + nextScene);    
            //     GameManager.instance.state = GameManager._state.playing;
            //     checkNextScene();
            // }
            if(_scene != scene.cutScene)
            {
                checkNextScene();
                SceneManager.LoadScene("CutScene");
                GameManager.instance.state = GameManager._state.playing;
            }
        }

        if(_scene == scene.mainMenu)
        {
            if(Input.anyKey)
            {
                checkNextScene();
                SceneManager.LoadScene("Level" + nextScene);    
            }
        }

        if(_scene == scene.cutScene)
        {
            if(Input.anyKey)
            {
                if(win)
                {
                    GameManager.instance.state = GameManager._state.win;  
                    toMainMenu();
                }
                else
                {
                    SceneManager.LoadScene("Level" + nextScene); 
                    GameManager.instance.state = GameManager._state.playing;
                    checkNextScene();
                }
            }
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
                if(GameManager.instance.difficulty == GameManager.diff.four)//diff is four
                {
                    win = true;
                }
                else
                {
                    checkNextDiff();
                    passLvlFour = false;
                    nextScene = "01";
                }
                break;
            case scene.cutScene:
                break;
            case scene.mainMenu:
                nextScene = "01";
                break;
            default:
                break;
        }
    }

    public void toMainMenu()
    {
        Debug.Log("ToMain");
        SceneManager.LoadScene("MainMenu");
    }

    void checkCurrentScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();

        switch(currentScene.name)
        {
            case "Level01":
                _scene = scene.one;
                break;
            case "Level02":
                _scene = scene.two;
                break;
            case "Level03":
                _scene = scene.three;
                break;
            case "Level04":
                _scene = scene.four;
                break;
            case "CutScene":
                _scene = scene.cutScene;
                break;
            case "MainMenu":
                _scene = scene.mainMenu;
                break;
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

    void playMusicOnScene()
    {
        var currentScene = _scene;
        switch(currentScene)
        {
            case scene.one:
                if(canPlayMusic)
                {
                    SoundManager.instance.PlayMusic("State01BGM");
                    canPlayMusic = false;
                }
                break;
            case scene.two:
                
                break;
            case scene.three:
                if(canPlayMusic)
                {
                    SoundManager.instance.PlayMusic("State03BGM");
                    canPlayMusic = false;
                }
                break;
            case scene.four:
                if(canPlayMusic)
                {
                    SoundManager.instance.PlayMusic("State04BGM");
                    canPlayMusic = false;
                }
                break;
            case scene.cutScene:
                break;
        }
    }
}
