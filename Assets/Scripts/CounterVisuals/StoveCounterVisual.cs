using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityRandom = UnityEngine.Random;

[DisallowMultipleComponent]
public class StoveCounterVisual : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;

    [SerializeField] private GameObject stoveVisual;
    [SerializeField] private ParticleSystem stoveVisualParticle;

    private void Start()
    {
        stoveCounter.OnStoveStateChanged += StoveCounter_OnStoveStateChanged;
    }

    private void OnDisable()
    {
        stoveCounter.OnStoveStateChanged -= StoveCounter_OnStoveStateChanged;
    }

    private void StoveCounter_OnStoveStateChanged(object sender, OnStoveStateChangedArgs e)
    {
        bool showVisual = e.StoveState != StoveStates.Idle;
        ShowAndHideVisual(showVisual);
    }

    private void ShowAndHideVisual(bool set)
    {
        if (set)
        {
            stoveVisual.SetActive(true);
            stoveVisualParticle.Play();
        }
        else
        {
            stoveVisual.SetActive(false);
            stoveVisualParticle.Stop();
        }
    }
}