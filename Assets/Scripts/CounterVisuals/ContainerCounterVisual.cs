using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityRandom = UnityEngine.Random;

public class ContainerCounterVisual : MonoBehaviour
{
    [SerializeField] private ContainerCounter containerCounter;
    private Animator animator;
    private const string OPEN_CLOSE = "OpenClose";

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        containerCounter.OnPlayerPickUp += ContainerCounter_OnPlayerPickUp;
    }

    private void OnDisable()
    {
        containerCounter.OnPlayerPickUp -= ContainerCounter_OnPlayerPickUp;
    }
    private void ContainerCounter_OnPlayerPickUp()
    {
        animator.SetTrigger(OPEN_CLOSE);
    }
}