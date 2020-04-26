using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menuIndexer : MonoBehaviour
{
    [SerializeField] public int index;
    [SerializeField] public bool keyDown;
    [SerializeField] public int maxIndex;


    // Update is called once per frame
    void Update()
    {
        if(Input.GetAxis("Vertical") != 0)
        {
            if(!keyDown)
            {
                //Pressing down
                if(Input.GetAxis("Vertical") < 0)
                {
                    if(index < maxIndex)
                    {
                        index++;
                    }
                    else
                    {
                        index = 0;
                    }
                }
                //Pressing up
                else if (Input.GetAxis("Vertical") > 0)
                {
                    if(index > 0)
                    {
                        index--;
                    }
                    else
                    {
                        index = maxIndex;
                    }
                }
                keyDown = true;
            }
        }
        else
        {
            keyDown = false;
        }
    }
}
