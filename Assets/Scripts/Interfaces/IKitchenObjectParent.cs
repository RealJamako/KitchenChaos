using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityRandom = UnityEngine.Random;

public interface IKitchenObjectParent
{
    public KitchenObject KitchenObject { get; set; }
    public Transform GetKitchenObjectFollowTransform();
    public void ClearKitchenObject();
    public bool HasKitchenObject();
}