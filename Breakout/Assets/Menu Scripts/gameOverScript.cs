using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using TMPro;

public class gameOverScript : MonoBehaviour
{
    public GameObject endScore;
    public GameObject endBricks;

    //Updates end points
    void Start()
    {
        endScore.GetComponent<TextMeshProUGUI>().text = BrickProperties.totalPoints.ToString();
        endBricks.GetComponent<TextMeshProUGUI>().text = BrickProperties.numBricksDestroyed.ToString();
    }

    public void startAgain()
    {
        SceneManager.LoadScene("Single Player");
    }

    public void mainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void exitGame()
    {
        Application.Quit();
    }
}
