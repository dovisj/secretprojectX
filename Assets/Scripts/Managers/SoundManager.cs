using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] flowerGetSounds;
    [SerializeField]
    private AudioClip pourWater;
    [SerializeField]
    private AudioClip[] wooshSounds;
    [SerializeField]
    private AudioClip chillMusic;
    [SerializeField]
    private AudioClip controlMusic;

    [SerializeField] private AudioClip itemGrabSound;
    protected static SoundManager instance;
    private AudioSource _audioSource;
    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (SoundManager)FindObjectOfType(typeof(SoundManager));

                if (instance == null)
                {
                    Debug.LogError("An instance of " + typeof(SoundManager) + " is needed in the scene, but there is none.");
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        PlayChillMusic();
    }

    public void PlayRandomFlowerGetSound()
    {
        _audioSource.PlayOneShot(flowerGetSounds[Random.Range(0, flowerGetSounds.Length)]);
    }
    public void PlayChillMusic()
    {
        _audioSource.clip = chillMusic;
        _audioSource.loop = true;
        _audioSource.Play();
    }

    public void PlayUnderControlMusic()
    {
        _audioSource.clip = controlMusic;
        _audioSource.loop = true;
        _audioSource.Play();
    }
    
    public void PlayItemGrabSound()
    {
        _audioSource.PlayOneShot(itemGrabSound);
    }
    
    public void PlayRandomWoosh()
    {
        _audioSource.PlayOneShot(wooshSounds[Random.Range(0, wooshSounds.Length)]);
    }
    
    public void PlayPourWater()
    {
        _audioSource.PlayOneShot(pourWater);
    }
}
