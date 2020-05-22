using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// using to access the TextMeshProUGUI component of an object and to parse strings
using TMPro;
using System;

// This script is used to load the Game Over scene for the two player scene
// the Game Over scene will be loaded when the number of lives for both players = 0
public class TwoPlayerController : MonoBehaviour
{

	// game object that holds the TMP prompt for the user to start
	public GameObject display;

	// the textmeshproUGUI object within the display
	private TextMeshProUGUI displayUGUI;

	// game object that holds the TMP prompt for the user to start
	public GameObject secondPrompt;

	// the textmeshproUGUI object within the display
	private TextMeshProUGUI secondPromptUGUI;

	// the game objects for the two balls in the scene
	public GameObject playerBall;
	public GameObject machineBall;

	// the game objects for the lives of the two players
	public GameObject playerLives;
	public GameObject machineLives;

	private TextMeshProUGUI playerLivesUGUI;
	private TextMeshProUGUI machineLivesUGUI;

	// the game objects for the scores of the two players
	public GameObject playerScore;
	public GameObject machineScore;

	private TextMeshProUGUI playerScoreUGUI;
	private TextMeshProUGUI machineScoreUGUI;

	// the circlemovement scripts within the two player balls
	private CircleMovement playerBallScript;
	private CircleMovement machineBallScript;

	// this is the prompt that will be displayed when the player needs to spawn a new ball
	private string prompt;


    // Start is called before the first frame update
    void Start()
    {
    	// define the value for displayUGUI, and secondPromptUGUI objects
        displayUGUI = display.GetComponent<TextMeshProUGUI>();
        secondPromptUGUI = secondPrompt.GetComponent<TextMeshProUGUI>();

        // get the prompt from the display
        prompt = displayUGUI.text;

        // define the values for the playerBallScript and machineBallScript
        playerBallScript = playerBall.GetComponent<CircleMovement>();
        machineBallScript = machineBall.GetComponent<CircleMovement>();

        // get the TMPro UGUI game objects for the player's lives and scores
        playerLivesUGUI = playerLives.GetComponent<TextMeshProUGUI>();
        machineLivesUGUI = machineLives.GetComponent<TextMeshProUGUI>();

        playerScoreUGUI = playerScore.GetComponent<TextMeshProUGUI>();
        machineScoreUGUI = machineScore.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
    	// if both players have 0 lives, then load the game over scene
        if(Int32.Parse(playerLivesUGUI.text) <= 0 && Int32.Parse(machineLivesUGUI.text) <= 0){

            UnityEngine.SceneManagement.SceneManager.LoadScene("Game Over");
        }

    	// if player hits space or clicks the mouse
        if((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0)) && Int32.Parse(playerLivesUGUI.text) > 0){

        	// clear the prompt
        	displayUGUI.text = "";

            // set buttonSpawn variable of machineBallScript to false so balls spawn constantly
            machineBallScript.buttonSpawn = false;
        }

        // if the player loses a ball, then display the prompt to the user again
        if(playerBallScript.GetClearBall() && Int32.Parse(playerLivesUGUI.text) > 0){

        	displayUGUI.text = prompt;
        }

        // if machine loses all of their lives before the player and the machine has a lower score
        if(Int32.Parse(machineLivesUGUI.text) <= 0 && Int64.Parse(machineScoreUGUI.text) < Int64.Parse(playerScoreUGUI.text)){

        	// display that the user wins
			secondPromptUGUI.text = "Player Wins!";
        }

		// if player loses all of their lives, then display game over on screen for the player
        if(Int32.Parse(playerLivesUGUI.text) <= 0){

			displayUGUI.text = "Game Over";

			// if the player has lost all of their lives, and their score is less than the machine
			if(Int64.Parse(machineScoreUGUI.text) > Int64.Parse(playerScoreUGUI.text)){

				// display prompts to the user
				displayUGUI.text = "Press Space to Quit";
				secondPromptUGUI.text = "Machine Wins!";

				// if the player hits space or clicks, then load game over scene
				if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0)){

					UnityEngine.SceneManagement.SceneManager.LoadScene("Game Over");
				}
			}
        }
    }
}
