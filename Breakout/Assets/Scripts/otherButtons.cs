using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class otherButtons : MonoBehaviour
{
    [SerializeField] public menuIndexer mi;
    [SerializeField] public Animator ani;
    [SerializeField] public int current;
    public GameObject creditsPanel;
    public GameObject settingsPanel;
    public GameObject creditsReturn;
    public GameObject musicSlider;

    public void loadScene()
    {
        switch (current)
        {
            case 0:
                SceneManager.LoadScene("Leaderboard");
                break;
            case 1:
                panelChange(settingsPanel, musicSlider);
                break;
            case 2:
                panelChange(creditsPanel, creditsReturn);
                break;
            case 3:
                SceneManager.LoadScene("Main Menu");
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (mi.index == current)
        {
            ani.SetBool("selected", true);

            if (Input.GetAxis("Submit") == 1)
            {
                ani.SetBool("pressed", true);
            }
            else if (ani.GetBool("pressed") == true)
            {
                ani.SetBool("pressed", false);
            }
        }
        else
        {
            ani.SetBool("selected", false);
        }
    }

    /* Shows/hides panel depending on whether or not panel's currently visible. 
    Time requried to prevent double input from return button and other-menu option. */
    public void panelChange(GameObject panel, GameObject selectedObject)
    {
        if (panel.activeSelf == true)
        {
            panel.SetActive(false);
            Time.timeScale = 1f;
        }
        else
        {
            panel.SetActive(true);
            EventSystem.current.SetSelectedGameObject(selectedObject);
            Time.timeScale = 0f;
        }
    }
}
