using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class pauseScript : MonoBehaviour
{
    public static bool paused = false;
    public GameObject pauseMenuUI;
    public GameObject resumeSelection;
    private GameObject settingsUI;
    public static GameObject pauseUI;
    public static GameObject settingsButton;
    
    //Grabs UI elements and then hides before game begins
    private void Awake()
    {
        settingsUI = GameObject.Find("Settings Panel");
        pauseUI = GameObject.Find("Pause Panel");
        settingsButton = GameObject.Find("Settings");
        settingsUI.SetActive(false);
        pauseUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (paused)
            {
                //Prevents spam from resuming game while settings menu is open
                if (settingsUI.activeSelf == false)
                {
                    resumeGame();
                }
            }
            else
            {
                pauseGame();
            }
        }

    }

    void pauseGame()
    {
        pauseMenuUI.SetActive(true);
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(resumeSelection);
        Time.timeScale = 0f;
        paused = true;
    }

    public void resumeGame()
    {
        pauseMenuUI.SetActive(false);
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
        Time.timeScale = 1f;
        paused = false;
    }

    public void settings()
    {
        pauseMenuUI.SetActive(false);
        settingsUI.SetActive(true);
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(MixerController.sliderReference);
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
}
