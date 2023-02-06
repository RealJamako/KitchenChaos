using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityRandom = UnityEngine.Random;

[CreateAssetMenu(menuName = "Configs/KitchenFryingRecipeSO", fileName = "KitchenFryingRecipeSO")]
public class KitchenFryingRecipeSO : ScriptableObject
{
    public KitchenObjectSO Input;
    public KitchenObjectSO Output;
    public float FryingTimerMax;
}