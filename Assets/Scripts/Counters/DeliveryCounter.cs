using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityRandom = UnityEngine.Random;

public class DeliveryCounter : BaseCounter
{
    public static DeliveryCounter Instance { get; private set; }

    private void Awake()
    {
        InstanceCheck();
    }

    public override void Interact(PlayerController ply)
    {
        if (!ply.HasKitchenObject()) { return; }
        if(ply.KitchenObject.TryGetPlate(out PlateKitchenObject plateKitchenObject))
        {
            DeliveryManager.Instance.DeliverRecipe(plateKitchenObject);
            ply.KitchenObject.DestorySelf();
        }
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