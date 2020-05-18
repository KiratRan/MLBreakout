using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// using to access the TextMeshProUGUI component of an object and to parse strings
using TMPro;
using System;

public class TwoPlayerController : MonoBehaviour
{

	// game object that holds the TMP prompt for the user to start
	public GameObject display;

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

	// the textmeshproUGUI object within the display
	private TextMeshProUGUI displayUGUI;

	// the circlemovement scripts within the two player balls
	private CircleMovement playerBallScript;
	private CircleMovement machineBallScript;

	// this is the prompt that will be displayed when the player needs to spawn a new ball
	private string prompt;


    // Start is called before the first frame update
    void Start()
    {
    	// define the value for displayUGUI object
        displayUGUI = display.GetComponent<TextMeshProUGUI>();

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

    	// if player hits space or clicks the mouse
        if((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0)) && Int32.Parse(playerLivesUGUI.text) > 0){

        	// clear the prompt
        	displayUGUI.text = "";

            // set buttonSpawn variable of machineBallScript to false so balls spawn constantly
            machineBallScript.buttonSpawn = false;
        }

        // if the player loses a ball, then display the prompt to the user
        if(playerBallScript.GetClearBall()){

        	displayUGUI.text = prompt;
        }

		// if player loses all of their lives, then display game over on screen for the player
        if(Int32.Parse(playerLivesUGUI.text) <= 0){

			displayUGUI.text = "Game Over";
        }
    }
}
