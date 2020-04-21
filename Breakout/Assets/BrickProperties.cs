using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickProperties : MonoBehaviour
{
    public GameObject myBall;
    public GameObject myDisplay;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionExit2D(Collision2D col){

    	if(col.collider.name == myBall.name){
    		// Debug.Log("Ball - Brick Collision");
    		
    		gameObject.GetComponent<SpriteRenderer>().enabled = false;
    		gameObject.GetComponent<BoxCollider2D>().enabled = false;
    		
    	}
    }
}
