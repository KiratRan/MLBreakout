using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class otherButtons : MonoBehaviour
{
    [SerializeField] public menuIndexer mi;
    [SerializeField] public Animator ani;
    [SerializeField] public int current;

    public void loadScene()
    {
        switch (current)
        {
            case 0:
                SceneManager.LoadScene("Leaderboard");
                break;
            case 1:
                //credits
                break;
            case 2:
                SceneManager.LoadScene("Main Menu");
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (mi.index == current)
        {
            ani.SetBool("selected", true);

            if (Input.GetAxis("Submit") == 1)
            {
                ani.SetBool("pressed", true);
            }
            else if (ani.GetBool("pressed") == true)
            {
                ani.SetBool("pressed", false);
            }
        }
        else
        {
            ani.SetBool("selected", false);
        }
    }
}
