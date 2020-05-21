using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// using to access the TextMeshProUGUI component of an object and to parse strings
using TMPro;
using System;

// This script is used to load the Game Over scene for the single player and AI player scenes
// the Game Over scene will be loaded when the number of lives = 0
public class SinglePlayerController : MonoBehaviour
{
	// the game objects for the lives of the player
	public GameObject playerLives;
	private TextMeshProUGUI playerLivesUGUI;

    // Start is called before the first frame update
    void Start()
    {

        // get the TMPro UGUI game objects for the player's lives
        playerLivesUGUI = playerLives.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
    	// if the player loses their lives, then load the game over scene
         if(Int32.Parse(playerLivesUGUI.text) <= 0){

			UnityEngine.SceneManagement.SceneManager.LoadScene("Game Over");
        }
    }
}
