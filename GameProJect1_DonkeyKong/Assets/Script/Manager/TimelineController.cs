using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class TimelineController : MonoBehaviour
{
    [SerializeField] PlayableDirector endCutScene;

    public void endedScene()
    {
        if(endCutScene)
        {
            if(GameManager.instance.state == GameManager._state.win)
                endCutScene.Play();
        }
    }

    // go to summary score scene
    public void ToCutScene()
    {
        SetTimeToOne();
        SceneManager.LoadScene("CutScene");
        ScriptSceneManager.instance._CanPlayMusic = true;
        GameManager.instance.state = GameManager._state.playing;
    }

    public void ToNextLevel()
    {
        SceneManager.LoadScene("Level" + ScriptSceneManager.instance.nextScene);    
    }

    public void SetTimeToZero()
    {
        Time.timeScale = 0;
    }

    private void SetTimeToOne()
    {
        Time.timeScale = 1;
    }
}
