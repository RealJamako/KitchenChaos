using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityRandom = UnityEngine.Random;

public class CuttingCounter : BaseCounter, IHasProgress
{
    public static event EventHandler OnAnyCut;

    public event Action OnCut;
    public event EventHandler<OnCounterProgressChangedArgs> OnProgressChanged;

    [SerializeField] private KitchenCuttingRecipeSO[] cuttingRecipeSOArray;

    private int cuttingProgress;
    private bool cuttingComplete;

    new public static void ResetStaticData()
    {
        OnAnyCut = null;
    }

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
                if (ply.KitchenObject.TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    if (plateKitchenObject.TryAddIngredient(KitchenObject.GetKitchenObjectSO))
                    {
                        KitchenObject.DestorySelf();
                    }
                }
            }
        }
        else
        {
            //If this counter does not have an object on it and the player is holding an object
            //Parent the players held oject to this object
            if (ply.HasKitchenObject())
            {
                if (HasRecipeWithInput(ply.KitchenObject))
                {
                    ply.KitchenObject.SetKitchenObjectParent = this;
                    cuttingProgress = 0;
                    cuttingComplete = false;
                }
            }
        }
    }

    public override void InteractAlt(PlayerController ply)
    {
        if (!HasKitchenObject() && !HasRecipeWithInput(KitchenObject) || cuttingComplete) { return; }
        cuttingProgress++;
        OnCut?.Invoke();
        OnAnyCut?.Invoke(this, EventArgs.Empty);
        KitchenCuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(KitchenObject);
        OnProgressChanged?.Invoke(this, new OnCounterProgressChangedArgs
        {
            ProgressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
        });
        if (cuttingProgress >= cuttingRecipeSO.cuttingProgressMax)
        {
            OnProgressChanged?.Invoke(this, new OnCounterProgressChangedArgs
            {
                ProgressNormalized = 0
            });
            KitchenObjectSO outputKitchenObject = GetOutput(KitchenObject);
            KitchenObject.DestorySelf();
            KitchenObject.SpawnKitchenObject(outputKitchenObject, this);
            cuttingComplete = true;
        }
    }

    private KitchenObjectSO GetOutput(KitchenObject inputObject)
    {
        if (inputObject == null) { return null; }
        return cuttingRecipeSOArray.FirstOrDefault(x => x.Input == inputObject.GetKitchenObjectSO).Output;
    }

    private bool HasRecipeWithInput(KitchenObject inputObject)
    {
        if (inputObject == null) { return false; }
        return cuttingRecipeSOArray.Where(x => x.Input == inputObject.GetKitchenObjectSO).Any();
    }

    private KitchenCuttingRecipeSO GetCuttingRecipeSOWithInput(KitchenObject inputObject)
    {
        if (inputObject == null) { return null; }
        return cuttingRecipeSOArray.FirstOrDefault(x => x.Input == inputObject.GetKitchenObjectSO);
    }
}