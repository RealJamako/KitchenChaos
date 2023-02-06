using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityRandom = UnityEngine.Random;

public class ContainerCounter : BaseCounter
{
    public event Action OnPlayerPickUp;
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public override void Interact(PlayerController ply)
    {
        if (ply.HasKitchenObject()) { return; }
        KitchenObject.SpawnKitchenObject(kitchenObjectSO, ply);
        OnPlayerPickUp?.Invoke();
    }
}