using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;
using TMPro;

using static leaderboard;
using static BrickProperties;
using static TwoPlayerStats;
using static SinglePlayerStats;

public class gameOverScript : MonoBehaviour
{
    // get gameobjects for use
    public GameObject endScore;
    public GameObject endBricks;
    public GameObject machineScore;
    public GameObject machineBricks;
    public GameObject playerHeader;
    public GameObject machineHeader;
    public GameObject highScoreMessage;
    public GameObject inputContainer;
    public GameObject inputName;
    public GameObject menuOptions;
    public GameObject inputButton;
    public GameObject firstMenuOption;
    private string userName;

    // private variable to hold the high score of the player
    private long playerScore;

    void Start()
    {

        // if the previous scene was single player or single AI, then use the SinglePlayerStats static variables
        // for the display and playerScore variable
        if (mainButtons.sceneName == "Single Player" || mainButtons.sceneName == "Single AI"){

            //Updates UI with user score and number of bricks destroyed
            endScore.GetComponent<TextMeshProUGUI>().text = SinglePlayerStats.playerScore.ToString();
            endBricks.GetComponent<TextMeshProUGUI>().text = SinglePlayerStats.playerBricksDestroyed.ToString();

            // define playerScore variable
            playerScore = SinglePlayerStats.playerScore;
        }

        // if the previous scene was two player, then update the game over scene to be formatted to display
        // the scores and bricks destroyed for both players
		if (mainButtons.sceneName == "Two Player"){

			machineScore.SetActive(true);
			machineBricks.SetActive(true);

			playerHeader.SetActive(true);
			machineHeader.SetActive(true);

            // get the player's score and bricks destroyed from the two player stats static variables
	        endScore.GetComponent<TextMeshProUGUI>().text = TwoPlayerStats.playerScore.ToString();
	        endBricks.GetComponent<TextMeshProUGUI>().text = TwoPlayerStats.playerBricksDestroyed.ToString();

            // get the machine's score and bricks destroyed from the two player stats static variables
	        machineScore.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = TwoPlayerStats.machineScore.ToString();
			machineBricks.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = TwoPlayerStats.machineBricksDestroyed.ToString();

            // define playerScore variable
            playerScore = TwoPlayerStats.playerScore;
		}

        // if the current scene is single player, then update the leaderboard
        if (mainButtons.sceneName == "Single Player" || mainButtons.sceneName == "Two Player")
        {
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
		            if (playerScore > highScores.highScoreEntryList[i].score)
		            {
		                showNewScorePrompts();
		                break;
		            }
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
            createScore(playerScore, userName.ToUpper());

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
