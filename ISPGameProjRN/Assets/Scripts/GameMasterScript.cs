using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMasterScript : MonoBehaviour {
    public KeyCode PauseKey;
    public GameObject PauseMenu;
    public PlayerController playerScript;
    bool isPaused;

    private void Start()
    {
        isPaused = false;
    }

    private void Update()
    {
        if(Input.GetKeyDown(PauseKey) && !isPaused)
        {
            Time.timeScale = 0;
            PauseMenu.SetActive(true);
            if(playerScript != null)
            {
                playerScript.DisableControl();
            }
            //Unpause through GUI
        }
 
    }
}
