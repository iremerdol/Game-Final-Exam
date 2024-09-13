using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    private AudioSource soundSource;
    private AudioSource musicSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        soundSource = GetComponent<AudioSource>();
        musicSource = transform.GetChild(0).GetComponent<AudioSource>();

        //assign initial volumes
        ChangeMusicVolume(0);
        ChangeSoundVolume(0);
    }

    public void PlaySound(AudioClip sound)
    {
        soundSource.PlayOneShot(sound);
    }
    public void ChangeSoundVolume(float change)
    {
        ChangeSourceVolume(0.5f, "soundVolume", change, soundSource);
    }
    public void ChangeMusicVolume(float change)
    {
        ChangeSourceVolume(0.1f, "musicVolume", change, musicSource);
    }
    private void ChangeSourceVolume(float baseVolume, string volumeName, float change, AudioSource source){
        //get initial value of volume and change it
        float currentVolume = PlayerPrefs.GetFloat(volumeName, 1);
        currentVolume += change;

        //check if we reached the maximum or minimum value
        if(currentVolume > 1.1)
            currentVolume = 0;
        else if(currentVolume < 0)
            currentVolume = 1;    

        //assign final value
        float finalVolume = currentVolume * baseVolume;
        source.volume = finalVolume;

        //save final value to player prefs
        PlayerPrefs.SetFloat(volumeName, currentVolume);
    }
}

/* public class SoundManager : MonoBehaviour
{
    public static SoundManager instance {get; private set;}
    private AudioSource soundSource;
    private AudioSource musicSource;

    private void Awake()
    {
        soundSource = GetComponent<AudioSource>();
        musicSource = transform.GetChild(0).GetComponent<AudioSource>();
        

        //keep this object even when we change scenes
        if(instance == null){
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        //destroy duplicate objects
        else if(instance != null && instance != this){
            Destroy(gameObject);
        }

        //assign initial volumes
        ChangeMusicVolume(0);
        ChangeSoundVolume(0);
    }
    public void PlaySound(AudioClip sound)
    {
        soundSource.PlayOneShot(sound);
    }
    public void ChangeSoundVolume(float change)
    {
        ChangeSourceVolume(0.5f, "soundVolume", change, soundSource);
    }
    public void ChangeMusicVolume(float change)
    {
        ChangeSourceVolume(0.1f, "musicVolume", change, musicSource);
    }
    private void ChangeSourceVolume(float baseVolume, string volumeName, float change, AudioSource source){
        //get initial value of volume and change it
        float currentVolume = PlayerPrefs.GetFloat(volumeName, 1);
        currentVolume += change;

        //check if we reached the maximum or minimum value
        if(currentVolume > 1.1)
            currentVolume = 0;
        else if(currentVolume < 0)
            currentVolume = 1;    

        //assign final value
        float finalVolume = currentVolume * baseVolume;
        source.volume = finalVolume;

        //save final value to player prefs
        PlayerPrefs.SetFloat(volumeName, currentVolume);
    } 
} */
