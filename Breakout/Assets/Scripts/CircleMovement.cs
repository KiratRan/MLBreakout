using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// using these to parse strings and access the TextMeshProUGUI component of an object
using System;
using TMPro;

public class CircleMovement : MonoBehaviour
{
	// this is the paddle that the ball will interact with; set in the inspector of the ball object in Unity
	public GameObject myPaddle;

    // this is the game object that has the TextMeshProUGUI component that displays the number of lives
    // the player has left; This is set in Unity inspector for the ball
    public GameObject myLives;

    // this is the game object that contains all of the bricks that this ball is going to interact with
    public GameObject myBricks;

    // this is the game object that contains the score that this ball is going to interact with
    public GameObject myScore;

    // this is a variable to have balls spawn in constantly if it is set to true, or have balls only
    // spawn in when the player presses spacebar or clicks on the left mouse
    public bool infiniteBalls = false;



    // this is the TextMeshProUGUI component that will be used to update the number of lives;
    // it will automatically be set when start() is run
    private TextMeshProUGUI livesUGUI;

    // this is the TextMeshProUGUI component of the muScore gameObject that will be used to tell
    // if all of the bricks have been destroyed
    private TextMeshProUGUI scoreUGUI;

	// create variables for accessing components of the ball; these components will be set in
    // the start() function and can then be used to modify these components of the ball during runtime
	private Rigidbody2D rb;
	private SpriteRenderer sr;
	private CircleCollider2D cc2d;



	// these are constants used to update the speed of the ball after it hits certain bricks
	private const float speedTwo = 1.3f;
	private const float speedThree = 1.6f;
	private const float speedFour = 2.0f;

	// this is used to increase the speed of the ball; public variable that can be changed
	// when all of the bricks are destroyed in a level
	private float speedFactor = 1.0f;
	// this is used to increase the speed of the ball after 4 and 12 bricks have been destroyed for the current ball
	private int numBricks = 0;
	// this is used to update the speed of the ball in the FixedUpdate() function
	private bool updateSpeed = false;

	// starting y-value of the ball; can be changed if needed
	private const float startingYPos = -1.25f;

	// the minimum and maximum x-values that the ball can start in; can be changed if needed
	private const float minX = -1.0f;
	private const float maxX = 1.0f;

	// this is used to keep track if the ball needs to be reset to a starting position and then give the
    // ball a velocity; this is used when the player hits space/clicks to start playing with a new ball
	private bool resetBall = false;

	// this is used to keep track if the ball made contact with the floor and needs to become invisible,
    // not collide with objects, and lose its velocity
	private bool clearBall = false;

	// this is used to tell if the ball collided with the paddle and change the ball's
	// velocity after the collision
	public bool paddleCollision = false;

	// this is used to determine if the velocity of teh ball needs to be updated after it hits a wall;
	// if the angle of the ball hitting the wall is less than minWallAngle, then the speed needs to be updated
	// to prevent infinite wall bouncing
	private bool wallCollision = false;



	// these are the angles used to calculate the ball's updated velocity after it hits the paddle;
	// the ninetyAngle should be set at 90 degrees for current calculations to function correctly;
    // the maxAngle is the maximum angle from 90 degrees that the ball can bounce off from the paddle(can be updated to test different angles)

    // they are both displayed in radians since, the Mathf class in unity uses radians
	private const float maxAngle = 1.0f/3.5f*Mathf.PI;
	private const float ninetyAngle = 1.0f/2.0f*Mathf.PI;

	// this defines the minimum angle that the ball can bounce off the wall at
	// this will prevent the ball from bouncing back and forth between walls infinitely
	private const float minWallAngle = 1.0f/36.0f*Mathf.PI;




	// speed will be the magnitude of velocity in both the x and y directions
	private float speed = 2.0f;

	// xDirection and yDirection are used to determine the starting velocity of the ball
	// xDirection will randomly vary from -1 and 1, while yDirection will remain constant
	private int xDirection = 1;
	private int yDirection = -1;

    // constant values to change x velocity to either the left or the right
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

        // get the TextMeshProUGUI component of the lives game object
        livesUGUI = myLives.GetComponent<TextMeshProUGUI>();

        scoreUGUI = myScore.GetComponent<TextMeshProUGUI>();

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

    	// if player needs to spawn a ball, then enter
    	if(!infiniteBalls){

			//Load game over screen if lives reach 0, or somehow less than
	        if(Int32.Parse(livesUGUI.text) <= 0)
	        {
	            UnityEngine.SceneManagement.SceneManager.LoadScene("Game Over");
	        }

	    	// check if the space bar or mouse 1 button have been pressed down. if either one has been pressed then enter
	        if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0)){

				if(pauseScript.paused == false){

		            // check to see if the ball is currently rendered(in play). if it is, then do not add a new ball
		            if(!gameObject.GetComponent<SpriteRenderer>().enabled){

		                    // setting resetBall here to true so that FixedUpdate() can change the physics of the ball
		                    resetBall = true;
		            }
		        }
	        }
	    }

	    // else a ball will spawn in indefinitely
	    else{

	    	// check to see if the ball is currently rendered(in play). if it is, then do not add a new ball
            if(!gameObject.GetComponent<SpriteRenderer>().enabled){

                    // setting resetBall here to true so that FixedUpdate() can change the physics of the ball
                    resetBall = true;
            }

	    }
    }


    // FixedUpdate is called once per frame and should be used to update Physics (rigidbody values like speed)
    void FixedUpdate(){

    	// see if resetBall variable is true
    	if(resetBall){

    		// if it is true, then call the NewBall function and set resetBall variable to false
    		NewBall();
    		resetBall = false;
    	}

    	// see if clearBall variable is true
    	else if(clearBall){

    		// if it is, then call ClearBall function and set clearBall to false
    		ClearBall();
    		clearBall = false;
    	}

        // see if paddleCollision variable is true; if it is, then update the velocity
        // using the newXVel and newYVel values already calculated and set paddleCollision to false
    	else if(paddleCollision){

    		rb.velocity = new Vector2(newXVel, newYVel);
    		paddleCollision = false;
    	}
    	// if the ball needs to update the speed based on the bricks hit to increase the difficulty enter
    	else if(updateSpeed){

    		// update the velocity and reset updateSpeed to false
    		rb.velocity = new Vector2(newXVel, newYVel);
    		updateSpeed = false;
    	}

    	// the ball hit the wall with an angle less than minWallAngle; the velocity of the ball is
    	// updated so the ball does not bounce infinitely
    	else if(wallCollision){

    		rb.velocity = new Vector2(newXVel, newYVel);
    		wallCollision = false;
    	}
    }


    // this function is called when the ball collides with another object
    void OnCollisionEnter2D(Collision2D col){

        // if the name of the colliding object matches the name of our paddle, then enter
    	if(col.collider.name == myPaddle.name){

    		// get the location of the ball
    		Vector2 ballLocation;
    		ballLocation = CurrentPosition(gameObject);

    		// get the location of the paddle
    		Vector2 paddleLocation;
    		paddleLocation = CurrentPosition(myPaddle);

            // use the box collider component of the paddle to get its height
            float paddleHeight = myPaddle.GetComponent<BoxCollider2D>().bounds.size.y;

            // get the velocity of the ball
            Vector2 ballVelocity = new Vector2();
            ballVelocity = rb.velocity;

            // get the current score value as a long from the scoreUGUI
            long curVal = Int64.Parse(scoreUGUI.text);

            // if player reached max score for a set of bricks, then reset there are no more bricks and they need to be
            // reset
            if(curVal % 448 == 0 && curVal != 0){
				// call the ResetBricks() function
                ResetBricks();
            }


            // if the center of the ball is lower than the top surface of the paddle, then ball should be unaffected
            // and will not have unique bounce off the top surface of the paddle
            if(ballLocation.y < (paddleLocation.y + paddleHeight / 2)){

                return;
            }

            // set paddleCollision to true so that FixedUpdate() will change the velocity of the ball
            paddleCollision = true;

    		// calculate the magnitude of the velocity of the ball using pythagorean theorum
    		float ballVelMag = PythagoreanC(ballVelocity.x, ballVelocity.y);

    		// calculate the distance from the center of the ball to the center of the paddle
    		float ballToPaddle = ballLocation.x - paddleLocation.x;

            // make sure that max ballToPaddle value is 1, so that ball does not bounce more than the
            // expected angle to the right
            if(ballToPaddle > 1){
                ballToPaddle = 1;
            }
            // similarly make sure that the min ballToPaddle value is -1, so that ball does not bounce
            // more than the expected angle to the left
            else if(ballToPaddle < -1){
                ballToPaddle = -1;
            }

            // get the width of the paddle by accessing the BoxCollider2D component and getting the size of its bounds
            // in the x direction
    		float paddleWidth = myPaddle.GetComponent<BoxCollider2D>().bounds.size.x;

            // call the BallPaddleCollision() function to update the newXVel and newYVel parameters;
            // these will later be used in the FixedUpdate() function to change the velocity of the ball
            BallPaddleCollision(ballToPaddle, paddleWidth, ballVelMag, ref newXVel, ref newYVel);
    	}

        // if the ball collides with the floor then set clear ball to true and update lives
    	else if(col.collider.name == "Floor"){
    		clearBall = true;

            // reduce the value in the livesUGUI text field by one using function
            IncreaseTMProUGUIText(livesUGUI, -1);
    	}

    	// if the ball collides with a brick, determine if the speed needs to be updated
    	else if(col.collider.tag == "Brick" || col.collider.tag == "RedOrange Brick"){

    		// if speedFactor is less than 2, then check if it needs to be updated; if speedFactor is already 2 then
    		// it cannot go any higher
    		if(speedFactor < 2.0f){

    			// increment the number of bricks hit by this ball
    			numBricks++;

    			// if the ball hit a red or orange brick, then increase speedFactor to 2, and
    			// set newXVel and newYVel to be updated in FixedUpdate()
    			if(col.collider.tag == "RedOrange Brick"){
    				speedFactor = speedFour;
    				updateSpeed = true;
    			}

    			// if the ball has hit 4 bricks, then increase speedFactor to 1.3, and
    			// set newXVel and newYVel to be updated in FixedUpdate()
    			else if(numBricks == 4){

    				speedFactor = speedTwo;
    				updateSpeed = true;
    			}

    			// if the ball has hit 12 bricks, then increase speedFactor to 1.6, and
    			// set newXVel and newYVel to be updated in FixedUpdate()
    			else if(numBricks == 12){
    				speedFactor = speedThree;
    				updateSpeed = true;
    			}

    			UpdateSpeedByFactor(rb.velocity, speedFactor, ref newXVel, ref newYVel);
    		}
    	}

    	// determine if the ball hit off the wall at an angle less than minWallAngle radians
    	else if(col.collider.tag == "Wall"){

    		// get the magnitude of the speed of the ball
    		float curSpeed = rb.velocity.magnitude;

    		// calculate the minimum speed that the ball is allowed to move in the y-direction
    		float minYSpeed = curSpeed * Mathf.Sin(minWallAngle);

    		// if the y velocity is less than the minimum allowed y speed, and greater than or equal to 0
    		if(rb.velocity.y < minYSpeed && rb.velocity.y >= 0){

    			// set wall collision variable to true so that the speed is updated in the FixedUpdate function
    			wallCollision = true;

    			// add half of the minimum speed to the y velocity
    			newYVel = rb.velocity.y + 0.5f*minYSpeed;

    			// use the new y velocity to calculate the new x velocity
    			newXVel = Mathf.Sqrt((curSpeed * curSpeed) - (newYVel * newYVel));

    			// if x velocity is negative, make newXVel negative
    			if(rb.velocity.x <= 0){
    				newXVel *= left;
    			}
    		}

    		// if the y velocity is between - minimum y speed and 0
    		else if(rb.velocity.y > (minYSpeed*-1) && rb.velocity.y < 0){

				// set wall collision variable to true so that the speed is updated in the FixedUpdate function
    			wallCollision = true;

    			// add half of the minimum speed to the y velocity
    			newYVel = rb.velocity.y - 0.5f*minYSpeed;

    			// use the newYVel and trigonometry to update the newXVel to maintain the same magnitude of speed
    			newXVel = Mathf.Sqrt((curSpeed * curSpeed) - (newYVel * newYVel));

    			// if x velocity is negative, make newXVel negative
    			if(rb.velocity.x <= 0){
    				newXVel *= left;
    			}
    		}
    	}
    }




    // This function is used to make the ball invisible to the player, make sure that it does not collide with anything,
    // and to reset its velocity to 0
    void ClearBall(){

	    // stop rendering the ball, and remove its collider
    	sr.enabled = false;
    	cc2d.enabled = false;

        // set the velocity to 0
    	rb.velocity = new Vector2(0.0f, 0.0f);

        // set the clearBall variable to false
    	clearBall = false;
    }

    // This function is used to calculate the new x and y velocity of the ball when it hits the paddle.
    // If the ball hits on the left side of the paddle, then it will bounce off to the left. If it
    // hits on the right side of the paddle it will bounce off to the right. If it hits in the middle, then
    // the ball will bounce off straight up. The function takes in a float for the distance from the center of the ball
    // to the center of the paddle, a float for the width of the paddle, a float for the magnitude of the velocity of the ball
    // before hitting the paddle, a reference to a float to contain the new x velocity, and a reference to a float to contain
    // the new y velocity. The function does not return anything, but the values of the newXVal and newYVal parameters
    // will be updated. This function uses linear algebra and trigonometry to calculate the new x and y velocities of the ball.
    void BallPaddleCollision(float ballToPaddle, float paddleWidth, float ballVelMag, ref float newXVel, ref float newYVel){

        // myVar is used to calculate the angle that the ball will bounce off the paddle;
        // closer to the paddle will result in myVar equalling 0, and farther from the paddle will result in myVar
        // equalling 1; myVar is a ratio used to calculate the resulting angle of deflection
        float myVar = ballToPaddle*2/paddleWidth;

        // using myVar ratio to calculate the new angle that the ball is moving in relative to the paddle
        float angle = ninetyAngle - (maxAngle*myVar);

        // use the angle and trigonometry to calculate the new x and y velocities of the ball
        newXVel = Mathf.Cos(angle)* ballVelMag;
        newYVel = Mathf.Sin(angle)* ballVelMag;
    }


    // This function can be called to enable the sprite renderer and the circle collider for the ball
    // as well as reset the position and velocity of the ball. This is specific to the ball object.
    public void NewBall(){

    	// get the current score value as a long from the scoreUGUI
        long curVal = Int64.Parse(scoreUGUI.text);

	    // if player reached max score for a set of bricks, then reset there are no more bricks and they need to be
        // reset
        if(curVal % 448 == 0 && curVal != 0){
        	// call the ResetBricks() function
            ResetBricks();
		}

		// reset the variables for the ball's speedFactor
		numBricks = 0;
		speedFactor = 1.0f;

	    // get a random float between -2, and 2 for the starting position
    	float startingXPos = UnityEngine.Random.Range(minX, maxX);
    	// give the ball its new position
    	NewPosition(gameObject, startingXPos, startingYPos);

	    // render the ball, and add the collider
    	sr.enabled = true;
    	cc2d.enabled = true;

    	// determine of the ball will start moving down and to the left or down and to the right
    	int num = UnityEngine.Random.Range(0, 2);

    	// if num == 0, then ball will start moving to the left
    	if(num == 0){
    		xDirection = left;
    	}
    	// else the ball will move to the right
    	else{
    		xDirection = right;
    	}

    	// update the velocity of the ball
    	rb.velocity = new Vector2(speed*xDirection, speed*yDirection);
    }


    // This function is used to take the current speed of the ball and increase the speed based on the original speed
    // of the ball. If the original speed of the ball(when it was first put into play) is 2 and the myFactor variable
    // is 2, then the new speed will be 4. The function takes in a Vector2 object that holds the current speed of the ball
    // in the x and y direstions, a float for the factor to increase the speed by, a reference to a float to hold the
    // new x velocity, and a reference to a float that will hold the new y velocity. The function does not return anything,
    // but the reference variables newXSpeed and newYSpeed will be updated to hold the new speed in the x and y diections respectively.
    void UpdateSpeedByFactor(Vector2 speedVec, float myFactor, ref float newXSpeed, ref float newYSpeed){

    	// xDirect and yDirect will hold the direction the ball is travelling in; either 1 or -1
    	int xDirect = 1;
    	int yDirect = 1;

    	// if x or y velocities are in the negative directions, then update xDirect and yDirect accordingly
    	if(speedVec.x < 0){

    		xDirect = -1;
    	}
    	if(speedVec.y < 0){

    		yDirect = -1;
    	}

    	// calculate the new x and y velocities
    	newXSpeed = xDirect * speed * myFactor;
    	newYSpeed = yDirect * speed * myFactor;
    }


	// This function can be used to update the 2D position of a GameObject.
	// The function takes in a GameObject, a float for the x position, and a float for the
	// y position. The function does not return anything.
	void NewPosition(GameObject obj, float xPos, float yPos){

		obj.transform.position = new Vector2(xPos, yPos);
	}


    // This function can be used to get the 2D position of a GameObject.
    // the function takes in a gameobject as a variable and returns a Vector2
    // that contains the 2D location of the game object.
	Vector2 CurrentPosition(GameObject obj){

        // create new vector
		Vector2 pos = new Vector2();

        // set value of pos to the position of the game object
		pos = obj.transform.position;

        // return pos
		return pos;
	}


    // This is a function to update the integer value of the text in a TextMeshProUGUI. The
    // function takes in a TextMeshProUGUI component, and the integer value to increase the
    // value of the text in the TextMeshProUGUI. The TextMeshProUGUI must already have an integer value
    // in the text field. THe function does not return anything.
    void IncreaseTMProUGUIText(TextMeshProUGUI textUGUI, int change){

        // get the current text value as a string from the textmeshprougui
        string curVal = textUGUI.text;

        // calculate the new score by converting the string from the text to a long and adding the
        // points variable to it
        long newVal = Int64.Parse(curVal) + (long)change;

        // update the text for the textmeshprougui with the new score
        textUGUI.text = newVal.ToString();
    }

    // This function uses the Unity Math class to solve for the "c" value in the pythagorean theorum.
    // The function takes in a float for the length of side a, and a float for the length of side b,
    // and returns a float for the value of side c.
    float PythagoreanC(float a, float b){

        return Mathf.Sqrt(Mathf.Pow(a, 2) + Mathf.Pow(b, 2));
    }

    // This function will reset and display all of the bricks that this ball is going to interact with.
    // The function does not have any arguments and does not return anything.
    void ResetBricks(){

    	// get the BoxCollider2D and SpriteRenderers of all of the Bricks
        Component [] brickColliders = myBricks.GetComponentsInChildren(typeof(BoxCollider2D));
        Component [] brickRenderers = myBricks.GetComponentsInChildren(typeof(SpriteRenderer));

        // for each brick, enable the SpriteRenderer and the BoxCollider
        for(int i = 0; i < brickColliders.Length;i++){

            ((BoxCollider2D)brickColliders[i]).enabled = true;
            ((SpriteRenderer)brickRenderers[i]).enabled = true;
		}

	}

}
