using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuButtonControls : MonoBehaviour {
    public GameObject fadeObj;
    private bool isFading;
    private bool startPressed = false;
    private bool quitPressed = false;
    public void Start()
    {
        Time.timeScale = 1;
        isFading = false;
        StartCoroutine("FadeIn");
    }
    public void StartButtonClicked()
    {
        if (!isFading)
        {
            startPressed = true;
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
        if (startPressed)
        {
            SceneManager.LoadScene("Room_One", LoadSceneMode.Single);
        } else if (quitPressed)
        {
            Application.Quit();
        }
        yield break;
    }
    public void QuitButtonClicked()
    {
        if (!isFading)
        {
            quitPressed = true;
            StartCoroutine("Fade");
        }
    }
    IEnumerator FadeIn()
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
        yield break;
    }
   
}
