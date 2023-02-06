using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityRandom = UnityEngine.Random;

public class PlayerSound : MonoBehaviour
{
    private PlayerController player;

    private float footstepTimer;
    private float footstepTimeMax = 0.1f;

    private void Awake()
    {
        player = GetComponent<PlayerController>();
    }

    private void Update()
    {
        footstepTimer -= Time.deltaTime;
        if (footstepTimer < 0f)
        {
            footstepTimer = footstepTimeMax;
            if (player.IsWalking())
            {
                SoundManager.Instance.PlayFoostepSounds(this.transform.position, 1f);
            }
        }
    }
}