using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
using MLAgents.Sensors;

public class PaddleAgent : Agent
{
    Rigidbody2D rBody;
    Rigidbody2D b;
    public GameObject my_ball;
    CircleMovement ballScript;
    public GameObject the_walls;
    public GameObject the_bricks;


    // Start is called before the first frame update
    void Start () {
      rBody = this.gameObject.GetComponent<Rigidbody2D>();
      ballScript = my_ball.GetComponent<CircleMovement>();
    }

    public override void OnEpisodeBegin()
    {
      this.rBody.velocity = Vector2.zero;
      ballScript.NewBall();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
      //Distance Ball to Paddle
      sensor.AddObservation(Vector2.Distance(this.transform.position,my_ball.transform.position));
      //Debug.Log("Observation" + Vector2.Distance(this.transform.position,my_ball.transform.position));

      //Direction of Ball
      sensor.AddObservation(my_ball.transform.position);
      sensor.AddObservation(this.transform.position);

      //Distance Wall to Paddle
      sensor.AddObservation(Vector2.Distance(this.transform.position,the_walls.transform.position));


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
        //Debug.Log("ActionX" + controlSignal.x);
        //rBody.AddForce(controlSignal * speed);
        rBody.MovePosition(rBody.position + controlSignal * Time.fixedDeltaTime);

        // Rewards
        float distanceToTarget = Vector2.Distance(this.transform.position,my_ball.transform.position);

        // Reached target
        /*
        if (distanceToTarget < 1.42f)
        {
            SetReward(0.3f);
        }*/

        if (ballScript.paddleCollision==true)
        {
          SetReward(1.0f);

        }
        // Ball Fell
        if (my_ball.transform.position.y < this.transform.position.y)
        {
            SetReward(-10.0f);
            EndEpisode();
        }
        // Ball Moving
        if (my_ball.transform.position.y > this.transform.position.y)
        {
            //SetReward(1.0f);
            //EndEpisode();
        }
        // Broke Brick TODO:get brick break function
        if (my_ball.transform.position.y > 8)
        {
            SetReward(10.0f);
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
