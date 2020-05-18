using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;
using TMPro;

using static leaderboard;
using static BrickProperties;

public class gameOverScript : MonoBehaviour
{
    public GameObject endScore;
    public GameObject endBricks;
    public GameObject highScoreMessage;
    public GameObject inputContainer;
    public GameObject inputName;
    public GameObject menuOptions;
    public GameObject inputButton;
    public GameObject firstMenuOption;
    private string userName;

   
    void Start()
    {
        //Hides high-score message
        highScoreMessage.SetActive(false);
        inputContainer.SetActive(false);

        //Updates UI with user score and number of bricks destroyed
        endScore.GetComponent<TextMeshProUGUI>().text = totalPoints.ToString();
        endBricks.GetComponent<TextMeshProUGUI>().text = numBricksDestroyed.ToString();

        //Grabs list of high-scores
        highScoreList highScores = getAndSortHighScores();

        //List isn't full
        if (highScores.highScoreEntryList.Count < 15)
        {
            showNewScorePrompts();
        }
        //List is full
        else
        {
            for (int i = 0; i < highScores.highScoreEntryList.Count; i++)
            {
                //Creates new high-score and breaks loop to prevent multiple entries
                if ((int)totalPoints > highScores.highScoreEntryList[i].score)
                {
                    showNewScorePrompts();
                    break;
                }
            }
        }
    }

    //Allows user to press enter instead of just clicking to submit high-score
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return) && inputButton.activeSelf == true)
        {
            enterNewHighScore();
        }
    }

    public void startAgain()
    {
        if (mainButtons.sceneName == "")
        {
            SceneManager.LoadScene("Main Menu");
        }
        else
        {
            SceneManager.LoadScene(mainButtons.sceneName);
        }
    }

    public void mainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void exitGame()
    {
        Application.Quit();
    }

    //Manipulates visibility of UI elements for new high-score
    private void showNewScorePrompts()
    {
        highScoreMessage.SetActive(true);
        inputContainer.SetActive(true);
        inputButton.SetActive(true);
        menuOptions.SetActive(false);

        //Changes focus to textbox automatically
        EventSystem.current.SetSelectedGameObject(inputContainer);
    }

    //Validates and enters user new high-score
    public void enterNewHighScore()
    {
        //Grabs user info
        userName = inputName.GetComponent<TextMeshProUGUI>().text;

        //For some reason a length of 4 corresponds to 3 entered characters. Counting \0?
        if (userName.Length == 4 && userName.Contains(" ") == false)
        {
            createScore((int)totalPoints, userName.ToUpper());

            //Manipulates visibility of UI elements
            inputContainer.SetActive(false);
            inputButton.SetActive(false);
            menuOptions.SetActive(true);

            //Sets selected event to first menu option. Required for keyboard movement
            EventSystem.current.SetSelectedGameObject(firstMenuOption);
        }
        //Handles invalid input
        else
        {
            //Changes text of submit button to inform user
            inputButton.GetComponentInChildren<TextMeshProUGUI>().text = "Error: Enter 3 Letter Name";
            
            //Changes focus to textbox
            EventSystem.current.SetSelectedGameObject(inputContainer);
        }
    }
}
