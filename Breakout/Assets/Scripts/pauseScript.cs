using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class pauseScript : MonoBehaviour
{
    public static bool paused = false;
    public GameObject pauseMenuUI;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (GetPauseStatus())
            {
                resumeGame();
            }
            else
            {
                pauseGame();
            }
        }

    }

    public void pauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        SetPauseStatus(true);
    }

    public void resumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        SetPauseStatus(false);
    }

    public void mainMenu()
    {
        resumeGame();
        SceneManager.LoadScene("Main Menu");
    }

    public void exitGame()
    {
        Application.Quit();
    }

    public bool GetPauseStatus()
    {
      return paused;
    }

    public void SetPauseStatus(bool p)
    {
      paused = p;
    }
}
