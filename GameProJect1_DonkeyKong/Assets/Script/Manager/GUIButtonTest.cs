using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GUIButtonTest : MonoBehaviour
{
    public bool ShowGui;
    PlayerControl player;
    bool _pause = false;

    private void Update() 
    {
        if(Input.GetKeyDown(KeyCode.F1) && !ShowGui)
            ShowGui = true;
        else if(Input.GetKeyDown(KeyCode.F1) && ShowGui)
            ShowGui = false;
        if(GameObject.FindGameObjectWithTag("Player") != null)
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();  
        else
            player = null;
    }

    void OnGUI() 
    {
        GUILayout.BeginArea(new Rect(450,1000,150,150));
        if(ShowGui)
        {
            if (GUILayout.Button("Win"))
                GameManager.instance.state = GameManager._state.win;
            if (GUILayout.Button("Exit Game"))
                Application.Quit();
            
            

            string pauseMenu = _pause? "Resume":"Pause"; 

            if(GUILayout.Button(pauseMenu))
            {
                if(!_pause)
                {
                    _pause = true;
                    Time.timeScale = 0f;
                }
                else
                {
                    _pause = false;
                    Time.timeScale = 1;
                }
            }

            if(player != null)
            {
                string takeDamage = player.canTakeDamage? "Can't Take Damage" : "Can Take Damage";
                if (GUILayout.Button(takeDamage))
                {
                    if(player.canTakeDamage)
                        player.canTakeDamage = false;
                    else if(!player.canTakeDamage)
                        player.canTakeDamage = true;
                }
            }
        }
        GUILayout.EndArea();
    }
}
