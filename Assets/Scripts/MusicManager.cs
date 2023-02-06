using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityRandom = UnityEngine.Random;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }
    private AudioSource audioSource;

    private float volume;
    private bool muteVolume;
    public bool MuteVolume { 
        get 
        { 
            return muteVolume; 
        } 
        set 
        {
            if (audioSource == null) { return; }
            audioSource.volume = value ? 0 : volume;
            muteVolume = value; 
        } 
    }

    private void Awake()
    {
        InstanceCheck();
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        volume = audioSource.volume;
    }

    private void InstanceCheck()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one player instance");
            return;
        }
        else
        {
            Instance = this;
        }
    }
}