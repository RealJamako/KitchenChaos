using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityRandom = UnityEngine.Random;

[CreateAssetMenu(menuName = "Configs/KitchenObjectSO", fileName = "KitchenObjectSO")]
public class KitchenObjectSO : ScriptableObject
{
    public GameObject Prefab;
    public Sprite Sprite;
    public string Name;
}