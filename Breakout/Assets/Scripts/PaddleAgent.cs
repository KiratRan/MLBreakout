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
<<<<<<< HEAD
    CircleMovement ballScript;
    //BrickProperties brickScript;
    public GameObject the_walls;
    public GameObject left_wall;
    public GameObject right_wall;
    public GameObject the_bricks;
    public GameObject the_reward;
    private TextMeshProUGUI rewardScore;
    public float sens = 10.0f;
=======
   
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

>>>>>>> refs/remotes/origin/master

    private float leftWallXPos;
    private float rightWallXPos;

    private float ceilingYPos;
    private float paddleYPos;

    private float circleRadius;
    private float paddleWidth;

    private Vector3 paddleStart;

    // Start is called before the first frame update
    void Start () {
<<<<<<< HEAD
      rBody = this.gameObject.GetComponent<Rigidbody2D>();
      ballScript = my_ball.GetComponent<CircleMovement>();
      rewardScore = the_reward.GetComponent<TextMeshProUGUI>();
      //brickScript = the_bricks.GetComponent<BrickProperties>();

      paddleStart = this.transform.localPosition;

      float wallWidth = the_walls.transform.GetChild(0).gameObject.GetComponent<BoxCollider2D>().bounds.size.x;
          paddleWidth = this.GetComponent<BoxCollider2D>().bounds.size.x;

          leftWallXPos = the_walls.transform.GetChild(0).localPosition.x + wallWidth/2;
          rightWallXPos = the_walls.transform.GetChild(1).localPosition.x - wallWidth/2;
          ceilingYPos = the_walls.transform.GetChild(2).localPosition.y - the_walls.transform.GetChild(2).gameObject.GetComponent<BoxCollider2D>().bounds.size.y / 2;
          paddleYPos = this.transform.localPosition.y + this.GetComponent<BoxCollider2D>().bounds.size.y / 2;

          circleRadius = my_ball.GetComponent<CircleCollider2D>().radius;
    }
=======
>>>>>>> refs/remotes/origin/master

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
<<<<<<< HEAD
      this.rBody.velocity = Vector2.zero;
      //Debug.Log("resetBallx " + ballScript.resetBall);
      //Debug.Log("clearBallx " + ballScript.clearBall);
      //ballScript.clearBall=true;
      //ballScript.FixedUpdate();
      //gameOver();
      ballScript.ClearBall();
      ballScript.NewBall();
      //ballScript.resetBall=true;
=======

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
>>>>>>> refs/remotes/origin/master
    }


    // This function collects abservations to send to the model for evaluation
    public override void CollectObservations(VectorSensor sensor)
    {
<<<<<<< HEAD
      //Distance Ball to Paddle
      /*
      sensor.AddObservation(Vector2.Distance(this.transform.position,my_ball.transform.position));
      //Debug.Log("Observation" + Vector2.Distance(this.transform.position,my_ball.transform.position));

      //Direction of Ball
      var heading = my_ball.transform.position - this.transform.position;
      var distance = heading.magnitude;
      var direction = heading / distance; // This is now the normalized direction.
      sensor.AddObservation(direction);
      //Debug.Log("Observation" + direction);
      sensor.AddObservation(my_ball.transform.position);
      sensor.AddObservation(this.transform.position);
      //Debug.Log("Observation" + my_ball.transform.position);
      //Debug.Log("Observation" + this.transform.position);

      //Distance Wall to Paddle
      sensor.AddObservation(Vector2.Distance(this.transform.position,left_wall.transform.position));
      sensor.AddObservation(Vector2.Distance(this.transform.position,right_wall.transform.position));
      //Debug.Log("Observation Left Wall" + Vector2.Distance(this.transform.position,left_wall.transform.position));
      //Debug.Log("Observation Right Wall" + Vector2.Distance(this.transform.position,right_wall.transform.position));

      // Target and Agent positions
      //sensor.AddObservation(my_ball.position);
      //sensor.AddObservation(this.transform.position);

      // Agent velocity
      //sensor.AddObservation(rBody.velocity.x);
      //sensor.AddObservation(rBody.velocity.y);

      // Target velocity
      //sensor.AddObservation(my_ball.velocity.x);
      //sensor.AddObservation(my_ball.velocity.y);
      */

      float normalPosBall = Normalize(rightWallXPos, leftWallXPos, circleRadius, my_ball.transform.localPosition.x);
  sensor.AddObservation(normalPosBall);

      // calculate the normalized y position of the ball and add observation
      normalPosBall = Normalize(ceilingYPos, paddleYPos, circleRadius, my_ball.transform.localPosition.y);
      sensor.AddObservation(normalPosBall);

      // calculate the normalized x position of the paddle
      float normalPosPaddle = Normalize(rightWallXPos, leftWallXPos, paddleWidth/2, this.transform.localPosition.x);
      sensor.AddObservation(normalPosPaddle);
=======

        // calculate the normalized x position of the ball and add observation
        float normalPosBall = Normalize(rightWallXPos, leftWallXPos, circleRadius, my_ball.transform.localPosition.x);
		sensor.AddObservation(normalPosBall);

        // calculate the normalized y position of the ball and add observation
        normalPosBall = Normalize(ceilingYPos, paddleYPos, circleRadius, my_ball.transform.localPosition.y);
        sensor.AddObservation(normalPosBall);

        // calculate the normalized x position of the paddle
        float normalPosPaddle = Normalize(rightWallXPos, leftWallXPos, paddleWidth/2, gameObject.transform.localPosition.x);
        sensor.AddObservation(normalPosPaddle);
>>>>>>> refs/remotes/origin/master
    }

    // This function gets the action from the model, and adds rewards/penalties if
    // any
    public override void OnActionReceived(float[] vectorAction)
    {
<<<<<<< HEAD
        // Actions, size = 2
        Vector2 controlSignal = Vector2.zero;
        //controlSignal.x = vectorAction[0];
        controlSignal.x = vectorAction[0] * Time.deltaTime * sens;
        //Debug.Log("ActionX" + controlSignal.x);
        //rBody.AddForce(controlSignal);
        //rBody.MovePosition(rBody.position + controlSignal * Time.fixedDeltaTime * 6);
        //rBody.MovePosition(rBody.position + controlSignal * speed * Time.fixedDeltaTime);
        //rBody.MovePosition(rBody.position + controlSignal);
        rBody.transform.Translate(controlSignal);

        //rBody.transform.Translate(Input.GetAxis("Horizontal") * Time.deltaTime * sens,0,0);
        //Debug.Log("Action Log" + vectorAction[0]);

        // Rewards
        //float distanceToTarget = Vector2.Distance(this.transform.position,my_ball.transform.position);

        // Reached target
        /*
        if (distanceToTarget < 1.42f)
        {
            SetReward(0.3f);
        }*/
        //AddReward(0.05f);
        //IncreaseTMProUGUIText(rewardScore, 0.05);
        if (ballScript.paddleCollision==true)
        {
          //SetReward(1.0f);
          AddReward(0.30f);
          //IncreaseTMProUGUIText(rewardScore, 300);
=======
    	// create a controlSignal vector and set x value to the first value in the vectorAction array(modified for time and sensitivity)
        Vector3 controlSignal = new Vector3(0,0,0);
        controlSignal.x = vectorAction[0] * Time.deltaTime* sens;

        // move the paddle based off the controlSignal
        gameObject.transform.Translate(controlSignal);

        // give reward for the paddle hitting the ball
        if (ballScript.GetPaddleCollision())
        {
          AddReward(0.05f);
>>>>>>> refs/remotes/origin/master
        }
        // Ball Fell; give large penalty
        if (ballScript.GetClearBall())
        {
<<<<<<< HEAD
            //SetReward(-10.0f);
            AddReward(-1.0f);
            //IncreaseTMProUGUIText(rewardScore, -1000);
            EndEpisode();
=======
            AddReward(-1.0f);
>>>>>>> refs/remotes/origin/master
        }
        // Ball Moving; give small reward
        if (my_ball.transform.localPosition.y > gameObject.transform.localPosition.y)
        {
            AddReward(0.01f);
<<<<<<< HEAD
            //IncreaseTMProUGUIText(rewardScore, 1);
            //EndEpisode();
=======
>>>>>>> refs/remotes/origin/master
        }

        // give rewards based off change in score(bricks destroyed)
        int prevScore = curScore;
        curScore = Int32.Parse(scoreUGUI.text);
        if (curScore != prevScore)
        {
<<<<<<< HEAD
            Debug.Log("Ball Fall Bug");
            AddReward(-1.0f);
            //IncreaseTMProUGUIText(rewardScore, -1000);
            EndEpisode();
        }

        if (my_ball.transform.position.y > 45)
        {
            Debug.Log("Ball Roof Bug");
            //AddReward(-100.0f);
            //IncreaseTMProUGUIText(rewardScore, -100);
            EndEpisode();
        }
        // Broke Brick TODO:get brick break function
        //if (my_ball.transform.position.y > 8)

        if (ballScript.brickCollision==true)
        {
            AddReward(0.30f);
            //IncreaseTMProUGUIText(rewardScore, 300);
            //AddReward(10.0f);
            //IncreaseTMProUGUIText(rewardScore, 10);
            //EndEpisode();
        }
=======
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
>>>>>>> refs/remotes/origin/master

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
      //action[0] = Input.GetAxis("Horizontal");
      //Debug.Log("Action Log0" + action[0]);
      return action;
    }

<<<<<<< HEAD
    private float Normalize(float maxPos, float minPos, float dim, float objPos){

        maxPos -= dim;
        minPos += dim;

        return (objPos - minPos) / (maxPos - minPos);

    }
=======
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

>>>>>>> refs/remotes/origin/master
}
