using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityRandom = UnityEngine.Random;

public class PlateCounterVisual : MonoBehaviour
{
    [SerializeField] private PlatesCounter plateCounter;
    [SerializeField] private Transform counterTopPoint;
    [SerializeField] private GameObject plateVisual;

    private List<GameObject> plateVisualList = new();

    private const float plateOffsetY = 0.1f;

    private void Start()
    {
        plateCounter.OnPlateSpawned += PlateCounter_OnPlateSpawned;
        plateCounter.OnPlateRemoved += PlateCounter_OnPlateRemoved;
    }

    private void OnDisable()
    {
        plateCounter.OnPlateSpawned += PlateCounter_OnPlateSpawned;
        plateCounter.OnPlateRemoved -= PlateCounter_OnPlateRemoved;
    }

    private void PlateCounter_OnPlateSpawned()
    {
        GameObject plateGameObject = Instantiate(plateVisual, counterTopPoint);
        plateGameObject.transform.localPosition = new(0, plateOffsetY * plateVisualList.Count, 0);
        plateVisualList.Add(plateGameObject);
    }

    private void PlateCounter_OnPlateRemoved()
    {
        GameObject plateGameobject = plateVisualList.Last();
        plateVisualList.Remove(plateGameobject);
        Destroy(plateGameobject);
    }
}