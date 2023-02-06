using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityRandom = UnityEngine.Random;

[DisallowMultipleComponent]
public class ClearCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public override void Interact(PlayerController ply)
    {
        if (HasKitchenObject())
        {
            //If this counter has an object on it and the player is not holding an object
            //Parent the object to the player
            if (!ply.HasKitchenObject())
            {
                KitchenObject.SetKitchenObjectParent = ply;
            }
            else
            {
                if (ply.KitchenObject.TryGetPlate(out PlateKitchenObject playerHeldPlate))
                {
                    if (playerHeldPlate.TryAddIngredient(KitchenObject.GetKitchenObjectSO))
                    {
                        KitchenObject.DestorySelf();
                    }
                }
                else
                {
                    if (KitchenObject.TryGetPlate(out PlateKitchenObject counterHeldPlate))
                    {
                        if (counterHeldPlate.TryAddIngredient(ply.KitchenObject.GetKitchenObjectSO))
                        {
                            ply.KitchenObject.DestorySelf();
                        }
                    }
                }
            }
        }
        else
        {
            //If this counter does not have an object on it and the player is holding an object
            //Parent the object to the player
            if (ply.HasKitchenObject())
            {
                ply.KitchenObject.SetKitchenObjectParent = this;
            }
        }
    }
}