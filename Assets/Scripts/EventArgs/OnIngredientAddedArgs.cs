using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityRandom = UnityEngine.Random;

public class OnIngredientAddedArgs : EventArgs
{
    public KitchenObjectSO KitchenObjectAdded;
}