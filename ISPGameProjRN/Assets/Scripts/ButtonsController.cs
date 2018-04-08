using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonsController : MonoBehaviour {
    public GameObject PauseMenu;
    public PlayerController playerScript;

	public void ResumeButtonClicked()
    {
        Time.timeScale = 1;
        PauseMenu.SetActive(false);
        if(playerScript != null) {
            playerScript.Invoke("EnableControl", .05f);
                
        } 
    }

    public void QuitButtonClicked()
    {
        Application.Quit();
    }
}
