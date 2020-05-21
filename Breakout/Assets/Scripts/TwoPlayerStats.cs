using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// using these to parse strings and access the TextMeshProUGUI component of an object
using System;
using TMPro;

// This script is used to hold the statiscits for the two player scene
public class TwoPlayerStats : MonoBehaviour
{
    // game objects for the number of bricks destroyed and the score for the player
	public GameObject playerBricksHit;
	public GameObject playerScoreObject;

	private TextMeshProUGUI playerHitsUGUI;
	private TextMeshProUGUI playerScoreUGUI;

    // game objects for the number of bricks destroyed and the score for the machine
	public GameObject machineBricksHit;
	public GameObject machineScoreObject;

	private TextMeshProUGUI machineHitsUGUI;
	private TextMeshProUGUI machineScoreUGUI;

    // public static variables so that they are available when the Game Over Scene is loaded
	public static int playerBricksDestroyed;
	public static long playerScore;
	public static int machineBricksDestroyed;
	public static long machineScore;

    // Start is called before the first frame update
    void Start()
    {
        // on start, reset the stats to 0, and get the TextMeshPro components
        playerBricksDestroyed = 0;
     	playerScore = 0;
     	machineBricksDestroyed = 0;
     	machineScore = 0;

     	playerHitsUGUI = playerBricksHit.GetComponent<TextMeshProUGUI>();
     	playerScoreUGUI = playerScoreObject.GetComponent<TextMeshProUGUI>();

     	machineHitsUGUI = machineBricksHit.GetComponent<TextMeshProUGUI>();
     	machineScoreUGUI = machineScoreObject.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        // update the static variables based off the values in their corresponding TextMeshProUGUI
    	playerBricksDestroyed = Int32.Parse(playerHitsUGUI.text);
    	playerScore = Int64.Parse(playerScoreUGUI.text);

    	machineBricksDestroyed = Int32.Parse(machineHitsUGUI.text);
    	machineScore = Int64.Parse(machineScoreUGUI.text);
    }
}
