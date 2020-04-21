using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleMovement : MonoBehaviour
{
	public float sens = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate(){

    	gameObject.transform.Translate(Input.GetAxis("Horizontal") * Time.deltaTime * sens,0,0);
    }
}
