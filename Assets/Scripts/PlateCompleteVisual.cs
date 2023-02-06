using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityRandom = UnityEngine.Random;

[DisallowMultipleComponent]
public class PlateCompleteVisual : MonoBehaviour
{
    [SerializeField] private PlateKitchenObject plateKitchenObject;
    [SerializeField] private List<KitchenObjectSO_GameObject> plateObjects;

    private void Start()
    {
        plateKitchenObject.OnIngredientAdded += PlateKitchenObject_OnIngredientAdded;
        foreach (var item in plateObjects)
        {
            item.gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        plateKitchenObject.OnIngredientAdded -= PlateKitchenObject_OnIngredientAdded;
    }

    private void PlateKitchenObject_OnIngredientAdded(object sender, OnIngredientAddedArgs e)
    {
        var kitchenObject = plateObjects.Where(x => x.kitchenObjectSO == e.KitchenObjectAdded).First();
        kitchenObject.gameObject.SetActive(true);
    }
}