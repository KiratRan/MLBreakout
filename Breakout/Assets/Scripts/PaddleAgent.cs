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
    //BrickProperties brickScript;
    public GameObject the_walls;
    public GameObject left_wall;
    public GameObject right_wall;
    public GameObject the_bricks;

    public GameObject the_reward;
    private TextMeshProUGUI rewardScore;
    public float sens = 6.0f;



    // Start is called before the first frame update
    void Start () {

      rBody = this.gameObject.GetComponent<Rigidbody2D>();
      ballScript = my_ball.GetComponent<CircleMovement>();
      rewardScore = the_reward.GetComponent<TextMeshProUGUI>();
      //brickScript = the_bricks.GetComponent<BrickProperties>();
    }

    void IncreaseTMProUGUIText(TextMeshProUGUI textUGUI, double change){

        // get the current text value as a string from the textmeshprougui
        string curVal = textUGUI.text;

        // calculate the new score by converting the string from the text to a long and adding the
        // points variable to it
        long newVal = Int64.Parse(curVal) + (long)change;

        // update the text for the textmeshprougui with the new score
        textUGUI.text = newVal.ToString();
    }

    public override void OnEpisodeBegin()
    {
      this.rBody.velocity = Vector2.zero;
      //Debug.Log("resetBallx " + ballScript.resetBall);
      //Debug.Log("clearBallx " + ballScript.clearBall);
      //ballScript.clearBall=true;
      //ballScript.FixedUpdate();
      ballScript.ClearBall();
      ballScript.NewBall();
      //ballScript.resetBall=true;

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


      //Direction of Ball
      var heading = my_ball.transform.position - this.transform.position;
      var distance = heading.magnitude;
      var direction = heading / distance; // This is now the normalized direction.
      sensor.AddObservation(direction);
      sensor.AddObservation(my_ball.transform.position);
      sensor.AddObservation(this.transform.position);

      //Distance Wall to Paddle
      sensor.AddObservation(Vector2.Distance(this.transform.position,left_wall.transform.position));
      sensor.AddObservation(Vector2.Distance(this.transform.position,right_wall.transform.position));


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

    public float speed = 0.1f;
    public override void OnActionReceived(float[] vectorAction)
    {
        // Actions, size = 2
        Vector2 controlSignal = Vector2.zero;
        controlSignal.x = vectorAction[0];

        //Debug.Log("ActionX" + controlSignal.x);
        //rBody.AddForce(controlSignal * speed);
        //rBody.MovePosition(rBody.position + controlSignal * Time.fixedDeltaTime);
        //rBody.MovePosition(rBody.position + controlSignal * speed * Time.fixedDeltaTime);
        rBody.MovePosition(rBody.position + controlSignal * speed);

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
          AddReward(30.0f);
          IncreaseTMProUGUIText(rewardScore, 30);
        }
        // Ball Fell
        //if (my_ball.transform.position.y == -0.005)
        if (ballScript.clearBall==true)
        //if (my_ball.transform.position.y < this.transform.position.y)
        {
            //SetReward(-10.0f);
            AddReward(-100.0f);
            IncreaseTMProUGUIText(rewardScore, -100);
            EndEpisode();
        }
        // Ball Moving

        if (my_ball.transform.position.y > this.transform.position.y)
        {
            AddReward(1.0f);
            IncreaseTMProUGUIText(rewardScore, 1);
            //EndEpisode();
        }

        if (my_ball.transform.position.y < -10)
        {
            Debug.Log("Ball Fall Bug");
            AddReward(-100.0f);
            IncreaseTMProUGUIText(rewardScore, -100);
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
            AddReward(30.0f);
            IncreaseTMProUGUIText(rewardScore, 30);
            //AddReward(10.0f);
            //IncreaseTMProUGUIText(rewardScore, 10);
            //EndEpisode();

        }

    }

    public override float[] Heuristic()
    {
      var action = new float[1];
      action[0] = Input.GetAxis("Horizontal");
      return action;
    }

}
