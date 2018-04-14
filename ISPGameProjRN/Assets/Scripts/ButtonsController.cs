using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonsController : MonoBehaviour {
    public GameObject PauseMenu;
    public GameObject fadeObj;
    public PlayerController playerScript;
    private bool isFading = false;
    private bool quitPressed = false;
    private bool restartPressed = false;


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
        if (!isFading)
        {
            quitPressed = true;
            StartCoroutine("Fade");
        }
    }
    public void RestartButtonClicked()
    {
        if (!isFading)
        {
            restartPressed = true;
            StartCoroutine("Fade");
        }
    }
    IEnumerator Fade()
    {
        Color c = fadeObj.GetComponent<Image>().color;
        isFading = true;
        for (float f = 0f; f < 1f; f += .02f)
        {
            c.a = f;
            fadeObj.GetComponent<Image>().color = c;
            yield return null;
        }
        isFading = false;
        if (restartPressed)
        {
            SceneManager.LoadScene("Room_One", LoadSceneMode.Single);
        }
        else if (quitPressed)
        {
            SceneManager.LoadScene("Main_Menu", LoadSceneMode.Single);
        }
        yield break;
    }
}
