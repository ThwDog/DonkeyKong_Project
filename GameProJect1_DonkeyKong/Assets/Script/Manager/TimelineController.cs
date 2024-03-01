using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class TimelineController : MonoBehaviour
{
    [SerializeField] PlayableDirector startCutScene; //receive bool that check if startCutScene Had play then dont play
    [SerializeField] PlayableDirector endCutScene;

    private void Awake() 
    {
        //if(!GameManager.instance.CutSceneHasPlay) //if win or lose cutSceneHas play is false
        startCutScene.Play();
        // CutSceneHasPlay = true;
    }

    //when 
    public void endedScene()
    {
        if(GameManager.instance.state == GameManager._state.win)
            endCutScene.Play();
    }

    // go to summary score scene
    public void toCutScene()
    {
        setTimeToOne();
        SceneManager.LoadScene("CutScene");
        ScriptSceneManager.instance._CanPlayMusic = true;
        GameManager.instance.state = GameManager._state.playing;
    }

    public void setTimeToZero()
    {
        Time.timeScale = 0;
    }

    public void setTimeToOne()
    {
        Time.timeScale = 1;
    }
}
