using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityRandom = UnityEngine.Random;

[DisallowMultipleComponent]
public abstract class BaseCounter : MonoBehaviour, IKitchenObjectParent
{
    public static event EventHandler OnAnyObjectPlaced;

    [SerializeField] private Transform spawnPoint;

    public KitchenObject KitchenObject { get { return kitchenObject; } 
        set 
        {
            if (value != null)
            {
                OnAnyObjectPlaced?.Invoke(this, EventArgs.Empty);
            }
            kitchenObject = value; 
        } 
    }

    private KitchenObject kitchenObject;

    public static void ResetStaticData()
    {
        OnAnyObjectPlaced = null;
    }

    public abstract void Interact(PlayerController ply);

    public virtual void InteractAlt(PlayerController ply) { }

    public Transform GetKitchenObjectFollowTransform()
    {
        return spawnPoint;
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
}