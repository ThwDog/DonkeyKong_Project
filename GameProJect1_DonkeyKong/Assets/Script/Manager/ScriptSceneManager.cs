using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScriptSceneManager : SingletonClass<ScriptSceneManager>
{

    public enum scene
    {
        cutScene = 0,
        one = 1,
        two = 2,
        three = 3,
        four = 4,
        mainMenu = 5
    }

    private int howManySceneYouPass = 0; // for active kong ui in ui manager 
    public scene _scene;
    internal string nextScene;
    internal string previousScene;
    private bool passLvlFour = false;
    public bool win = false;

    [Header("About animation and music")]
    private bool canPlayMusic = true;
    public bool _CanPlayMusic
    {
        get
        {
            if (GameManager.instance.state == GameManager._state.lose || GameManager.instance.state == GameManager._state.win)
                return false;
            return canPlayMusic;
        }
        set
        {
            canPlayMusic = value;
        }
    }

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

        // when state win
        if (GameManager.instance.state == GameManager._state.win)
        {
            

            // add score in player 
            if (FindObjectOfType<CollectItem_Player>())
            {
                CollectItem_Player player = FindObjectOfType<CollectItem_Player>();
                GameManager.instance.IncreaseScore(player.playerScore);
            }

            if (FindAnyObjectByType<UIManager>())
            {
                UIManager ui = FindAnyObjectByType<UIManager>();
                ui.canUpdateText = false;
            }

            SoundManager.instance.StopAllMusic();

            if (GameObject.Find("ScoreManager") != null)
            {
                ScoreManager score = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
                score.finish();
            }

            if (_scene != scene.cutScene)
            {
                if(_scene == scene.four) // for clear ui index that use to active kong img ui
                    resetPassScene();
                else howManySceneYouPass++;

                checkNextScene();
                // Play Ended Scene
                if (FindAnyObjectByType<TimelineController>()) // make sure that have time line in scene
                {
                    Debug.Log("endScene");
                    TimelineController timeline = FindAnyObjectByType<TimelineController>();
                    timeline.endedScene(); // use when player win then play time line animation
                    GameManager.instance.state = GameManager._state.playing;
                }
                else
                {
                    SceneManager.LoadScene("CutScene");
                    _CanPlayMusic = true;
                    GameManager.instance.state = GameManager._state.playing;
                }
            }
        }
        else if (GameManager.instance.state == GameManager._state.lose)
        {
            if (_scene != scene.cutScene)
            {
                SceneManager.LoadScene("CutScene");
                _CanPlayMusic = true;
                GameManager.instance.state = GameManager._state.lose;
            }
        }

        if (_scene == scene.mainMenu)
        {
            win = false;
            if (Input.anyKey)
            {
                checkNextScene();
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                GameManager.instance.state = GameManager._state.playing;
                //SceneManager.LoadScene("Level" + nextScene);    
            }
        }

        if (_scene == scene.cutScene)
        {
            if (Input.anyKey)
            {
                if (win)
                {
                    GameManager.instance.state = GameManager._state.win;
                    toMainMenu();
                }
                else
                {
                    // if losing then load same scene else load next Scene
                    string scene = GameManager.instance.state == GameManager._state.lose? previousScene : nextScene;
                    Debug.Log("LoadScene " + scene);
                    SceneManager.LoadScene("Level" + scene);
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
                if (GameManager.instance.difficulty == GameManager.diff.one)//diff is one
                    nextScene = "04";
                if (GameManager.instance.difficulty == GameManager.diff.two)//diff is two
                    nextScene = "02";
                if (GameManager.instance.difficulty == GameManager.diff.three)//diff is three
                    nextScene = "03";
                if (GameManager.instance.difficulty == GameManager.diff.four && !passLvlFour)//diff is four
                    nextScene = "03";
                else if (GameManager.instance.difficulty == GameManager.diff.four && passLvlFour)
                    nextScene = "02";
                break;
            case scene.two:
                if (GameManager.instance.difficulty == GameManager.diff.two)//diff is two
                    nextScene = "04";
                if (GameManager.instance.difficulty == GameManager.diff.three)//diff is three
                    nextScene = "04";
                if (GameManager.instance.difficulty == GameManager.diff.four)//diff is four
                    nextScene = "04";
                break;
            case scene.three:
                if (GameManager.instance.difficulty == GameManager.diff.three)//diff is three
                    nextScene = "02";
                if (GameManager.instance.difficulty == GameManager.diff.four)//diff is four
                {
                    passLvlFour = true;
                    nextScene = "01";
                }
                break;
            case scene.four:
                if (GameManager.instance.difficulty == GameManager.diff.four)//diff is four
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

        switch (currentScene.name)
        {
            case "Level01":
                previousScene = "01";
                _scene = scene.one;
                break;
            case "Level02":
                previousScene = "02";
                _scene = scene.two;
                break;
            case "Level03":
                previousScene = "03";
                _scene = scene.three;
                break;
            case "Level04":
                previousScene = "04";
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
        switch (currentScene)
        {
            case scene.one:
                if (_CanPlayMusic)
                {
                    SoundManager.instance.PlayMusic("State01BGM");
                    _CanPlayMusic = false;
                }
                break;
            case scene.two:
                if (_CanPlayMusic)
                {
                    _CanPlayMusic = false;
                }
                break;
            case scene.three:
                if (_CanPlayMusic)
                {
                    SoundManager.instance.PlayMusic("State03BGM");
                    _CanPlayMusic = false;
                }
                break;
            case scene.four:
                if (_CanPlayMusic)
                {
                    SoundManager.instance.PlayMusic("State04BGM");
                    _CanPlayMusic = false;
                }
                break;
            case scene.cutScene:
                if (_CanPlayMusic)
                {
                    SoundManager.instance.PlaySfx("HowHighCanUGet");
                    _CanPlayMusic = false;
                }
                break;
            case scene.mainMenu:
                break;
            default:
                break;
        }
    }

    // public void reviveToSameScene()
    // {
    //     SceneManager.LoadScene("Level" + previousScene);
    //     GameManager.instance.state = GameManager._state.playing;
    // }

    public void kongImgUi(GameObject[] ui)
    {
        for (int i = 0; i <= howManySceneYouPass; i++)
        {
            if(ui.Length < i) // need to check again if it bug 
                break;

            ui[i].SetActive(true);
        }
    }

    public void resetPassScene()
    {
        howManySceneYouPass = 0;
    }
}
