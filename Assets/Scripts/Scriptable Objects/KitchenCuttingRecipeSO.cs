using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityRandom = UnityEngine.Random;

[CreateAssetMenu(menuName = "Configs/KitchenCuttingRecipeSO", fileName = "KitchenCuttingRecipeSO")]
public class KitchenCuttingRecipeSO : ScriptableObject
{
    public KitchenObjectSO Input;
    public KitchenObjectSO Output;
    public int cuttingProgressMax;
}