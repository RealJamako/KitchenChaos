using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityRandom = UnityEngine.Random;

[CreateAssetMenu(menuName = "Configs/AudioRefSO", fileName = "AudioRefSO")]
public class AudioRefSO : ScriptableObject
{
    public AudioClip[] ChopSounds;
    public AudioClip[] DeliveryFail;
    public AudioClip[] DeliverySuccess;
    public AudioClip[] Footsteps;
    public AudioClip[] ObjectDrop;
    public AudioClip[] ObjectPickUp;
    public AudioClip[] PanSizzle;
    public AudioClip[] Trash;
    public AudioClip[] Warning;
}