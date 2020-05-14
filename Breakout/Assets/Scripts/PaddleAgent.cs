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
	// define game object for the ball that this paddle plays the game with
    public GameObject my_ball;
   
   	// the left wall, right wall, and ceiling game objects that the paddle is confined by
    public GameObject leftWall;
    public GameObject rightWall;
    public GameObject ceiling;
    
    // the gameobjects that hold the lives and score textmeshproUGUI's 
    public GameObject myLives;
    public GameObject myScore;

    // this is a public boolean that determines if the agent is setup for training or
    // setup for regular play
    public bool training;

	// define variable for the rigid body of the paddle
    private Rigidbody2D rBody;

    // the script for the ball
    private CircleMovement ballScript;

    // the TextMeshProUGUI that holds the values for the paddle's lives and score
    private TextMeshProUGUI scoreUGUI;
    private TextMeshProUGUI livesUGUI;

    // the current score and current lives
    private int curScore;
	private int curLives;

	// the x position of the left wall and x position of the right wall after the width of the walls are taken into account
    private float leftWallXPos;
    private float rightWallXPos;

    // the y position of the ceiling and y position of the paddle after the height of these objects are taken into account
    private float ceilingYPos;
    private float paddleYPos;

    // the radius of the ball, and width of the paddle
    private float circleRadius;
    private float paddleWidth;

    // the local position coordinates of the paddle; used to reset the position
    private Vector3 paddleStart;

    // sensitivity of the paddle movements
    private float sens = 10.0f;


    // Awake() is called once and before Start(); required when using multiple scenes and reloading a scene
	// that uses an agent
    void Awake(){

    	// set the Academy so that it requires manual stepping; this is needed to reload scenes
    	Academy.Instance.AutomaticSteppingEnabled = false;
    }



    // Start is called before the first frame update
    void Start () {

    	// get the rigidbody of the paddle
		rBody = gameObject.GetComponent<Rigidbody2D>();

		// get the circle movement script of the ball
		ballScript = my_ball.GetComponent<CircleMovement>();

		// get the starting position of the paddle
		paddleStart = gameObject.transform.localPosition;

		// get the width of the walls
		float wallWidth = leftWall.GetComponent<BoxCollider2D>().bounds.size.x;

		// get the width of the paddle
        paddleWidth = gameObject.GetComponent<BoxCollider2D>().bounds.size.x;

        // define leftWallXPos and rightWallXPos so that they can be used in observations
        leftWallXPos = leftWall.transform.localPosition.x + wallWidth/2;
        rightWallXPos = rightWall.transform.localPosition.x - wallWidth/2;

        // define ceilingYPos and paddleYPos so that they can be used in observations
        ceilingYPos = ceiling.transform.localPosition.y - ceiling.GetComponent<BoxCollider2D>().bounds.size.y / 2;
        paddleYPos = gameObject.transform.localPosition.y + gameObject.GetComponent<BoxCollider2D>().bounds.size.y / 2;


        // get the radius of the ball
        circleRadius = my_ball.GetComponent<CircleCollider2D>().radius;
		
		// get the textmeshproUGUI components of the lives and scores game objects
		livesUGUI = myLives.GetComponent<TextMeshProUGUI>();
		scoreUGUI = myScore.GetComponent<TextMeshProUGUI>();

		// set starting values for the score and lives
		curScore = 0;

		if(training){
			curLives = 1;
		}

		else{
			curLives = 3;
		}
		

	}

	// call FixedUpdate() to manually step in the academy; required when using multiple scenes and reloading a scene
	// that uses an agent
	void FixedUpdate(){

		Academy.Instance.EnvironmentStep();
	}

	// reset the training environment
    public override void OnEpisodeBegin()
    {

    	// if this.rBody is set to NULL, then call Start() function again to set the variables;
    	// this is used when reloading a scene
		if (this.rBody == null){
    		Start();
    	}

    	// reset paddle position, and set velocity to 0
		this.rBody.velocity = Vector2.zero;
		gameObject.transform.localPosition = paddleStart;

		// if training, clear the ball, spawn a new one, and reset the bricks when the episode begins
		if(training){
			// reset the ball
			this.ballScript.ClearBall();
			this.ballScript.NewBall();

			// reset the bricks
			this.ballScript.ResetBricks();
		}

		// set val to 1 for training and to 3 when setting up for the game
		int val;
		if(training){
			val = 1;
		}
		else{
			val = 3;
		}

		this.livesUGUI.text = val.ToString();
		val = 0;
		this.scoreUGUI.text = val.ToString();
    }


    // This function collects abservations to send to the model for evaluation
    public override void CollectObservations(VectorSensor sensor)
    {

        // calculate the normalized x position of the ball and add observation
        float normalPosBall = Normalize(rightWallXPos, leftWallXPos, circleRadius, my_ball.transform.localPosition.x);
		sensor.AddObservation(normalPosBall);

        // calculate the normalized y position of the ball and add observation
        normalPosBall = Normalize(ceilingYPos, paddleYPos, circleRadius, my_ball.transform.localPosition.y);
        sensor.AddObservation(normalPosBall);

        // calculate the normalized x position of the paddle
        float normalPosPaddle = Normalize(rightWallXPos, leftWallXPos, paddleWidth/2, gameObject.transform.localPosition.x);
        sensor.AddObservation(normalPosPaddle);
    }

    // This function gets the action from the model, and adds rewards/penalties if
    // any
    public override void OnActionReceived(float[] vectorAction)
    {
    	// create a controlSignal vector and set x value to the first value in the vectorAction array(modified for time and sensitivity)
        Vector3 controlSignal = new Vector3(0,0,0);
        controlSignal.x = vectorAction[0] * Time.deltaTime* sens;

        // move the paddle based off the controlSignal
        gameObject.transform.Translate(controlSignal);

        // give reward for the paddle hitting the ball
        if (ballScript.GetPaddleCollision())
        {
          AddReward(0.05f);
        }
        // Ball Fell; give large penalty
        if (ballScript.GetClearBall())
        {
            AddReward(-1.0f);
        }
        // Ball Moving; give small reward
        if (my_ball.transform.localPosition.y > gameObject.transform.localPosition.y)
        {
            AddReward(0.01f);
        }

        // give rewards based off change in score(bricks destroyed)
        int prevScore = curScore;
        curScore = Int32.Parse(scoreUGUI.text);
        if (curScore != prevScore)
        {
        	float scoreDiff = curScore - prevScore;
            AddReward(scoreDiff/ 10.0f);
        }

        // add penalty for taking another step
        AddReward(-0.01f);

        // end the episode of all of the bricks are destroyed and the agent is training
        if(training){

		    // end episode when lose a live, or all bricks are destroyed
		    if(curScore % 448 == 0 && curScore != 0){
		    	EndEpisode();
		    }
		}

        // if the lives reaches 0, then end the training episode
        curLives = Int32.Parse(livesUGUI.text);
        if(curLives == 0){
        	EndEpisode();
        }
    }

    // This function is to manually control the paddle, instead of the model
    public override float[] Heuristic()
    {
      var action = new float[1];
      action[0] = Input.GetAxis("Horizontal");
      return action;
    }

    // This function is used to normalize a position of an object in one dimension based off its minimum and maximum possible values,
    // and the objects dimension in the one direction. The function takes in the maximum possible possition in this dimension as a float,
    //  the minimum possible position in this dimension as a float, the dimension of the object as a float, 
    // and the object's current position in the dimension. The function returns a float that is between 0, and 1. The closer to 0,
    // the closer the object is to the minimum position. The closer to 1, the closer the object is to the maximum position.
    private float Normalize(float maxPos, float minPos, float dim, float objPos){

    	// update maxPos and minPos based off the object's height/width
        maxPos -= dim;
        minPos += dim;

        // calculate and return the normalized value
        return (objPos - minPos) / (maxPos - minPos);

    }

}
