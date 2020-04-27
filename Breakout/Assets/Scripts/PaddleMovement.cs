using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleMovement : MonoBehaviour
{
    // public variable that can be changed by the user to set the sensitivity of the paddle
    // should be able to change in settings/controls
	public float sens = 6.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // used to update the physics of the paddle
    void FixedUpdate(){

        // update the x position of the paddle using the transform.Translate property
        // get the horizontal axis input, multiply by delta time so that the speed change is fixed, and multiply by the sensitivity
        // no change in y or z axes
    	gameObject.transform.Translate(Input.GetAxis("Horizontal") * Time.deltaTime * sens,0,0);
    }
}
