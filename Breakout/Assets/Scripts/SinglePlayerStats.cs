using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// using these to parse strings and access the TextMeshProUGUI component of an object
using System;
using TMPro;


// This script is used to hold the statiscits for the single player and AI player scenes
public class SinglePlayerStats : MonoBehaviour
{
    // game objects for the number of bricks destroyed and the score for the pplayer
	public GameObject bricksHit;
	public GameObject myScore;

	private TextMeshProUGUI hitsUGUI;
	private TextMeshProUGUI scoreUGUI;

    // public static variables so that they are available when the Game Over Scene is loaded
	public static int playerBricksDestroyed;
	public static long playerScore;

    // Start is called before the first frame update
    void Start()
    {
        // on start, reset the stats to 0, and get the TextMeshPro components
        playerBricksDestroyed = 0;
     	playerScore = 0;

     	hitsUGUI = bricksHit.GetComponent<TextMeshProUGUI>();
     	scoreUGUI = myScore.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        // update the static variables based off the values in their corresponding TextMeshProUGUI
    	playerBricksDestroyed = Int32.Parse(hitsUGUI.text);
    	playerScore = Int64.Parse(scoreUGUI.text);
    }
}
