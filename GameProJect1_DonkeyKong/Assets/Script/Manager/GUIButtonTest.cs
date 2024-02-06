using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GUIButtonTest : MonoBehaviour
{
    public bool ShowGui;
    PlayerControl player;

    private void Update() 
    {
        if(GameObject.FindGameObjectWithTag("Player") != null)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
        }    
        else
        {
            player = null;
        }
        
    }

    void OnGUI() 
    {
        
        if(ShowGui)
        {
            if (GUILayout.Button("Win"))
                GameManager.instance.state = GameManager._state.win;
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
    }
}
