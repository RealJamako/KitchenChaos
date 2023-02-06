using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityRandom = UnityEngine.Random;

public class PlatesCounter : BaseCounter
{
    public event Action OnPlateSpawned;
    public event Action OnPlateRemoved;
    [SerializeField] private KitchenObjectSO plateKitchenObjectSO;
    private const float spawnPlateTimerMax = 5f;
    private int platesSpawned;
    private const int platesSpawnedMax = 4;
    private float spawnPlateTimer;

    private void Update()
    {
        spawnPlateTimer += Time.deltaTime;
        if (spawnPlateTimer > spawnPlateTimerMax)
        {
            spawnPlateTimer = 0f;
            if (platesSpawned < platesSpawnedMax)
            {
                platesSpawned++;
                OnPlateSpawned?.Invoke();
            }
        }
    }

    public override void Interact(PlayerController ply)
    {
        if (ply.HasKitchenObject() && platesSpawned > 0) { return; }
        platesSpawned--;
        OnPlateRemoved?.Invoke();
        KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, ply);
    }
}