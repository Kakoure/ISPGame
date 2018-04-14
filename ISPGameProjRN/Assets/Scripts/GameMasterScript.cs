using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMasterScript : MonoBehaviour {
    public KeyCode PauseKey;
    public GameObject PauseMenu;
    public PlayerController playerScript;
    bool isPaused;
    bool openingScene;
    bool isFading = false;
    public GameObject fadeObj;

    private void Start()
    {
        Time.timeScale = 1;
        isPaused = false;
        openingScene = true;
        Color c = fadeObj.GetComponent<Image>().color;
        c.a = 1f;
        fadeObj.GetComponent<Image>().color = c;
        if (playerScript != null)
        {
            playerScript.DisableControl();
            StartCoroutine("FadeOut");
        }
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
    IEnumerator FadeIn()
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
        yield break;
    }
    IEnumerator FadeOut()
    {
        Color c = fadeObj.GetComponent<Image>().color;
        isFading = true;
        for (float f = 1f; f > 0f; f -= .02f)
        {
            c.a = f;
            fadeObj.GetComponent<Image>().color = c;
            yield return null;
        }
        isFading = false;
        if (openingScene)
        {
            playerScript.Invoke("EnableControl", .5f);
            Debug.Log("InControl");
            openingScene = false;
        }
        yield break;
    }
}
