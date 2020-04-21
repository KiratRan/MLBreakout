using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleMovement : MonoBehaviour
{
	// this is used to increase the speed of the ball; public variable that can be changed
	// when all of the bricks are destroyed in a level
	public float speedFactor = 2.0f;

	// this is the paddle that the ball will interact with; set in the inspector of the ball object
	public GameObject myPaddle;

	// create variables for accessing components of the ball
	private Rigidbody2D rb;
	private SpriteRenderer sr;
	private CircleCollider2D cc2d;

	// starting y-value of the ball
	private float startingYPos = -1.25f;

	// the minimum and maximum x-values that the ball can start in
	private float minX = -2.0f;
	private float maxX = 2.0f;

	// this is used to keep track if the ball needs to be reset to a starting position
	private bool resetBall = false;

	// this is used to keep track if the ball made contact with the floor and needs to reset
	private bool clearBall = false;

	// this is used to tell if the ball collided with the paddle and to update the ball's
	// velocity in the FixedUpdate() function if it has
	private bool paddleCollision = false;


	// these are the angles used to calculate the ball's updated velocity after it hits the paddle
	// the least angle is 
	private const float leastAngle = 1.0f/3.5f*Mathf.PI;
	private const float greatestAngle = 1.0f/2.0f*Mathf.PI;



	// these are used to set the speed of the ball in the X and Y directions
	// speed will be the magnitude of velocity in both the x and y directions
	private float speed = 1.0f;

	// xDirection and yDirection are used to determine the starting velocity of the ball
	// xDirection will randomly vary from -1 and 1, while yDirection will remain constant
	private int xDirection = 1;
	private int yDirection = -1;

	private const int left = -1;
	private const int right = 1;

	// this is the new velocity of the ball in the x and y directions after it makes contact with 
	// the paddle
	private float newXVel;
	private float newYVel;



    // Start is called before the first frame update
    void Start()
    {
    	// get the rigidBody2D component of the ball
    	rb = gameObject.GetComponent<Rigidbody2D>();

        // get the spriterenderer component of the ball and do not render the ball, so that it isn't visible on screen
        sr = gameObject.GetComponent<SpriteRenderer>();
        sr.enabled = false;
        // get the circular collider 2d component and do not let the ball have a collider
        cc2d = gameObject.GetComponent<CircleCollider2D>();
        cc2d.enabled = false;
    }

    // Update is called once per frame and should be used to get button entry
    void Update()
    {
    	// check if the space bar or mouse 1 button have been pressed down. if either one has been pressed and the ball is currently
    	// not being rendered, then set resetBall variable to true; properties of ball will be changed in FixedUpdate()
        if((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0)) && !gameObject.GetComponent<SpriteRenderer>().enabled){

        	resetBall = true;
        }

    }

    // FixedUpdate is called once per frame and should be used to update rigidbody values like speed
    void FixedUpdate(){

    	// see if resetBall variable is true
    	if(resetBall){

    		// if it is true, then call the ResetBall function and set resetBall variable to false
    		ResetBall();
    		resetBall = false;
    	}

    	// see if clearBall bool is true
    	if(clearBall){

    		// if it is, then call ClearBall function and set clearBall to false
    		ClearBall();
    		clearBall = false;
    	}

    	if(paddleCollision){

    		rb.velocity = new Vector2(newXVel, newYVel);

    		paddleCollision = false;
    	}

    }

    void OnCollisionEnter2D(Collision2D col){

    	if(col.collider.name == myPaddle.name){
    		paddleCollision = true;

    		// get the location of the ball
    		Vector2 ballLocation;
    		ballLocation = CurrentPosition(gameObject);

    		// get the location of the paddle
    		Vector2 paddleLocation;
    		paddleLocation = CurrentPosition(myPaddle);

			// Debug.Log("Location of ball: " + ballLocation.x +" "+ ballLocation.y);
    		// Debug.Log("Location of paddle: " + paddleLocation.x +" "+ paddleLocation.y);

    		// get the velocity of the ball
    		Vector2 ballVelocity = new Vector2();
    		ballVelocity = rb.velocity;

    		// calculate the magnitude of the velocity of the ball using pythagorean theorum
    		float ballVelMag = Mathf.Sqrt(Mathf.Pow(ballVelocity.x, 2) + Mathf.Pow(ballVelocity.y, 2));

    		// Debug.Log("Magnitude of Velocity of Ball: " + " "+ ballVelMag);


    		// calculate the distance from the center of the ball to the center of the paddle
    		float ballToPaddle = ballLocation.x - paddleLocation.x;


    		float paddleWidth = myPaddle.GetComponent<BoxCollider2D>().bounds.size.x;

    		// Debug.Log("Paddle Width: " + " "+ paddleWidth);

    		if(ballToPaddle >= 0){
    			
    			float myVar = ballToPaddle*2/paddleWidth;

    			float angle = greatestAngle - (leastAngle*myVar);

    			newXVel = Mathf.Cos(angle)* ballVelMag;
    			newYVel = Mathf.Sin(angle)* ballVelMag;
    		}

    		else{
    			ballToPaddle = paddleLocation.x - ballLocation.x;

    			float myVar = ballToPaddle*2/paddleWidth;

    			float angle = greatestAngle - (leastAngle*myVar);

    			newXVel = Mathf.Cos(angle)* ballVelMag* left;
    			newYVel = Mathf.Sin(angle)* ballVelMag;
    		}
    	}

    	else if(col.collider.name == "Floor"){
    		clearBall = true;
    	}
    }





    void ClearBall(){

	    // stop rendering the ball, and remove its collider
    	sr.enabled = false;
    	cc2d.enabled = false;

    	rb.velocity = new Vector2(0.0f, 0.0f);

    	clearBall = false;
    }

    /*void BallPaddleCollision(){

    	float myVar = ballToPaddle*2/paddleWidth;

		float angle = greatestAngle - (leastAngle*myVar);

		newXVel = Mathf.Cos(angle)* ballVelMag;
		newYVel = Mathf.Sin(angle)* ballVelMag;    	
    }*/


    // This function can be called to enable the sprite renderer and the circle collider for the ball 
    // as well as reset the position and velocity of the ball.
    void ResetBall(){

	    // get a random float between -2, and 2 for the starting position
    	float startingXPos = Random.Range(minX, maxX);
    	// give the ball its new position
    	NewPosition(gameObject, startingXPos, startingYPos);

	    // render the ball, and add the collider
    	sr.enabled = true;
    	cc2d.enabled = true;

    	// determine of the ball will start moving down and to the left or down and to the right
    	int num = Random.Range(0, 2);

    	// if num == 0, then ball will start moving to the left
    	if(num == 0){
    		xDirection = left;
    	}
    	// else the ball will move to the right
    	else{
    		xDirection = right;
    	}

    	// update the velocity of the ball
    	rb.velocity = new Vector2(speed*speedFactor*xDirection, speed*speedFactor*yDirection);
    }

	// This function can be used to update the 2D position of a GameObject.
	// The function takes in a GameObject, a float for the x position, and a float for the 
	// y position. The function does not return anything.
	void NewPosition(GameObject ball, float xPos, float yPos){

		ball.transform.position = new Vector2(xPos, yPos);
	}

	Vector2 CurrentPosition(GameObject obj){

		Vector2 pos = new Vector2();

		pos = obj.transform.position;

		return pos;
	}

}



