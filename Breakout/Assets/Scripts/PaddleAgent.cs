using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
using MLAgents.Sensors;

// using these to parse strings and access the TextMeshProUGUI component of an object
using System;
using TMPro;

public class PaddleAgent : Agent
{
    Rigidbody2D rBody;
    Rigidbody2D b;
    public GameObject my_ball;
    CircleMovement ballScript;
    public GameObject the_walls;
    public GameObject the_bricks;
    
    public GameObject myLives;
    public GameObject myScore;

    private TextMeshProUGUI scoreUGUI;
    private TextMeshProUGUI livesUGUI;

    private int curScore;
	private int curLives;

    private float leftWall;
    private float rightWall;

    private Vector3 paddleStart;


    // Start is called before the first frame update
    void Start () {
		rBody = this.gameObject.GetComponent<Rigidbody2D>();
		ballScript = my_ball.GetComponent<CircleMovement>();

		paddleStart = this.transform.localPosition;
		leftWall = the_walls.transform.GetChild(0).position.x + the_walls.transform.GetChild(0).gameObject.GetComponent<BoxCollider2D>().bounds.size.x / 2;
		rightWall = the_walls.transform.GetChild(1).position.x - the_walls.transform.GetChild(1).gameObject.GetComponent<BoxCollider2D>().bounds.size.x / 2;

		livesUGUI = myLives.GetComponent<TextMeshProUGUI>();
		scoreUGUI = myScore.GetComponent<TextMeshProUGUI>();

		curScore = 0;
		curLives = 3;
	}

    public override void OnEpisodeBegin()
    {
    	// reset paddle position, and set velocity to 0
		this.rBody.velocity = Vector2.zero;
		this.transform.localPosition = paddleStart;

		// reset the bricks and score
		ballScript.ResetBricks();

		int val = 3;
		livesUGUI.text = val.ToString();
		val = 0;
		scoreUGUI.text = val.ToString();

    }

    public override void CollectObservations(VectorSensor sensor)
    {
		//Distance Ball to Paddle
		// sensor.AddObservation(Vector2.Distance(this.transform.position,my_ball.transform.position));
		//Debug.Log("Observation" + Vector2.Distance(this.transform.position,my_ball.transform.position));

		//location of ball and location of paddle
		sensor.AddObservation(my_ball.transform.localPosition);
		sensor.AddObservation(this.transform.localPosition);

		// get the width of the paddle and the location of the paddle
		float paddleWidth = this.GetComponent<BoxCollider2D>().bounds.size.x;
		float paddleXLoc = this.transform.position.x;

		//Distance from left wall to paddle
		sensor.AddObservation(paddleXLoc - (paddleWidth / 2) - leftWall);

		// distance from right wall to paddle
		sensor.AddObservation(rightWall - (paddleXLoc + (paddleWidth / 2)));


		// Target and Agent positions
		//sensor.AddObservation(my_ball.position);
		//sensor.AddObservation(this.transform.position);

		// Agent velocity
		//sensor.AddObservation(rBody.velocity.x);
		//sensor.AddObservation(rBody.velocity.y);

		// Target velocity
		//sensor.AddObservation(my_ball.velocity.x);
		//sensor.AddObservation(my_ball.velocity.y);
    }

    public float speed = 10;
    public override void OnActionReceived(float[] vectorAction)
    {
        // Actions, size = 2
        Vector2 controlSignal = Vector2.zero;
        controlSignal.x = vectorAction[0];
        // Debug.Log("ActionX" + controlSignal.x);
        // rBody.AddForce(controlSignal * speed);
        rBody.MovePosition(rBody.position + speed* controlSignal * Time.fixedDeltaTime);



        // give reward for the paddle hitting the ball
        if (ballScript.paddleCollision==true)
        {
          AddReward(0.25f);
        }
        // Ball Fell; give large penalty
        if (my_ball.transform.position.y < this.transform.position.y)
        {
            AddReward(-10.0f);
        }
        // Ball Moving; give small reward
        if (my_ball.transform.position.y > this.transform.position.y)
        {
            AddReward(0.25f);
        }

        int prevScore = curScore;
        curScore = Int32.Parse(scoreUGUI.text);
        // Broke Brick TODO:get brick break function
        if (curScore != prevScore)
        {
        	float scoreDiff = curScore - prevScore;
            AddReward(scoreDiff);
        }

        // end episode when lose all 3 lives, or all bricks are destroyed
        if(curScore % 448 == 0 && curScore != 0){
        	EndEpisode();
        }

        curLives = Int32.Parse(livesUGUI.text);
        if(curLives == 0){
        	EndEpisode();
        }

    }

    public override float[] Heuristic()
    {
      var action = new float[1];
      action[0] = Input.GetAxis("Horizontal");
      return action;
    }

}
