using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityRandom = UnityEngine.Random;

public class PlateKitchenObject : KitchenObject
{
    public event EventHandler<OnIngredientAddedArgs> OnIngredientAdded;
    [SerializeField] private List<KitchenObjectSO> validKitchenObjectSOList = new();
    private List<KitchenObjectSO> kitchenObjectSOList = new();
    public List<KitchenObjectSO> KitchenObjectSOList { get { return kitchenObjectSOList; } }

    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO)
    {
        if (!validKitchenObjectSOList.Contains(kitchenObjectSO)) { return false; }
        if (kitchenObjectSOList.Contains(kitchenObjectSO)) { return false; }
        else 
            kitchenObjectSOList.Add(kitchenObjectSO);
        OnIngredientAdded?.Invoke(this, new OnIngredientAddedArgs
        {
            KitchenObjectAdded = kitchenObjectSO,
        });
        return true;
    }
}