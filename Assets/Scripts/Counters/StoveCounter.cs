using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Rendering;
using UnityEngine;
using UnityRandom = UnityEngine.Random;

public class StoveCounter : BaseCounter, IHasProgress
{
    public event EventHandler<OnStoveStateChangedArgs> OnStoveStateChanged;
    public event EventHandler<OnCounterProgressChangedArgs> OnProgressChanged;

    [SerializeField] private KitchenFryingRecipeSO[] kitchenFryingRecipeSOArray;
    private KitchenFryingRecipeSO kitchenFryingRecipeSO;

    // this is only used to visualise what state the stove is in, using the debug inspector.
    private StoveStates state; 

    private float timer;

    private void Start()
    {
        state = StoveStates.Idle;
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
            StopAllCoroutines();
            StartNextStoveState(StoveStates.Idle);
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
                    StartNextStoveState(StoveStates.Frying);
                }
            }
        }
    }

    private void StartNextStoveState(StoveStates startState)
    {
        timer = 0;
        state = startState;
        switch (startState)
        {
            case StoveStates.Idle:
                HandleIdleState();
                break;
            case StoveStates.Frying:
                kitchenFryingRecipeSO = GetFryingRecipeSOWithInput(KitchenObject);
                StartCoroutine(HandleFryingState());
                break;
            case StoveStates.Fried:
                kitchenFryingRecipeSO = GetFryingRecipeSOWithInput(KitchenObject);
                StartCoroutine(HandleFriedState());
                break;
            case StoveStates.Burned:
                HandleBurnedState();
                break;
            default:
                break;
        }
        OnStoveStateChanged?.Invoke(this, new OnStoveStateChangedArgs
        {
            StoveState = startState
        });
    }

    private IEnumerator HandleFryingState()
    {
        while (timer < kitchenFryingRecipeSO.FryingTimerMax)
        {
            timer += Time.deltaTime;
            OnProgressChanged?.Invoke(this, new OnCounterProgressChangedArgs
            {
                ProgressNormalized = timer / kitchenFryingRecipeSO.FryingTimerMax
            });
            yield return null;
        }
        KitchenObject.DestorySelf();
        KitchenObject.SpawnKitchenObject(kitchenFryingRecipeSO.Output, this);
        StartNextStoveState(StoveStates.Fried);
    }

    private IEnumerator HandleFriedState()
    {
        while (timer < kitchenFryingRecipeSO.FryingTimerMax)
        {
            timer += Time.deltaTime;
            OnProgressChanged?.Invoke(this, new OnCounterProgressChangedArgs
            {
                ProgressNormalized = timer / kitchenFryingRecipeSO.FryingTimerMax
            });
            yield return null;
        }
        KitchenObject.DestorySelf();
        KitchenObject.SpawnKitchenObject(kitchenFryingRecipeSO.Output, this);
        StartNextStoveState(StoveStates.Burned);
    }

    private void HandleBurnedState()
    {
        KitchenObject.DestorySelf();
        KitchenObject.SpawnKitchenObject(kitchenFryingRecipeSO.Output, this);
        StartNextStoveState(StoveStates.Idle);
    }

    private void HandleIdleState()
    {
        OnProgressChanged?.Invoke(this, new OnCounterProgressChangedArgs
        {
            ProgressNormalized = 0
        });
        kitchenFryingRecipeSO = null;
    }

    private KitchenObjectSO GetOutput(KitchenObject inputObject)
    {
        if (inputObject == null) { return null; }
        return kitchenFryingRecipeSOArray.FirstOrDefault(x => x.Input == inputObject.GetKitchenObjectSO).Output;
    }

    private bool HasRecipeWithInput(KitchenObject inputObject)
    {
        if (inputObject == null) { return false; }
        return kitchenFryingRecipeSOArray.Where(x => x.Input == inputObject.GetKitchenObjectSO).Any();
    }

    private KitchenFryingRecipeSO GetFryingRecipeSOWithInput(KitchenObject inputObject)
    {
        if (inputObject == null) { return null; }
        return kitchenFryingRecipeSOArray.FirstOrDefault(x => x.Input == inputObject.GetKitchenObjectSO);
    }
}