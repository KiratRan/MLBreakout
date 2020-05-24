using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MixerController : MonoBehaviour
{
    public AudioMixer audioMixer;
    public GameObject settingsUI;
    public static GameObject sliderReference;

    public Slider musicSlider;
    public Slider sfxSlider;

    private void Awake()
    {
        //Grabs current slider values
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume", 0);
        sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume", 0);

        //Reference used in pause script
        sliderReference = GameObject.Find("Music Slider");
    }

    public void setMusicVolume(float volume)
    {
        audioMixer.SetFloat("musicVolume", volume);
    }

    public void setSFXVolume(float volume)
    {
        audioMixer.SetFloat("sfxVolume", volume);
    }

    //Function used for return button from pause menu
    public void closeSettings()
    {
        settingsUI.SetActive(false);
        pauseScript.pauseUI.SetActive(true);
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(pauseScript.settingsButton);
    }

    //When settings is closed grabs current values and saves them in player prefs
    private void OnDisable()
    {
        float musicVolume = 0;
        float sfxVolume = 0;

        audioMixer.GetFloat("musicVolume", out musicVolume);
        audioMixer.GetFloat("sfxVolume", out sfxVolume);

        PlayerPrefs.SetFloat("musicVolume", musicVolume);
        PlayerPrefs.SetFloat("sfxVolume", sfxVolume);
        PlayerPrefs.Save();
    }
}
