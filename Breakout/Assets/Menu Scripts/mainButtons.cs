using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class mainButtons : MonoBehaviour
{
    [SerializeField] public menuIndexer mi;
    [SerializeField] public Animator ani;
    [SerializeField] public int current;
    public static string sceneName = "";
    public AudioMixer audioMixer;

    private void Start()
    {
        //Grabs saved volume levels or defaults to max volume if player prefs aren't found and sets audio levels
        audioMixer.SetFloat("musicVolume", PlayerPrefs.GetFloat("musicVolume", 0));
        audioMixer.SetFloat("sfxVolume", PlayerPrefs.GetFloat("sfxVolume", 0));
        
        if(PlayerPrefs.HasKey("paddleSpeed") == true)
        {
            int value = PlayerPrefs.GetInt("paddleSpeed");

            if(value == 0)
            {
                PaddleMovement.sens = 4.0f;
            }
            else if (value == 2)
            {
                PaddleMovement.sens = 8.0f;
            }
            else
            {
                PaddleMovement.sens = 4.0f;
            }

        }
    }

    public void loadScene()
    {
        switch(current)
        {
            case 0:
                SceneManager.LoadScene("Single Player");
                break;
            case 1:
                SceneManager.LoadScene("Two Player");
                break;
            case 2:
                SceneManager.LoadScene("Single AI");
                break;
            case 3:
                SceneManager.LoadScene("Other Menu");
                break;
            case 4:
                Application.Quit();
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (mi.index == current)
        {
            ani.SetBool("selected", true);

            if(Input.GetAxis("Submit") == 1)
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
