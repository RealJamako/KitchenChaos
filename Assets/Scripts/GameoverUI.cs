using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityRandom = UnityEngine.Random;
using TMPro;

[DisallowMultipleComponent]
public class GameoverUI : MonoBehaviour
{
    [SerializeField] private TMP_Text recipesDelivered;

    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
        GameManager.Instance.OnStateChange += Instance_OnStateChange;
    }

    private void Instance_OnStateChange()
    {
        ShowAndHideVisual(gameManager.IsGameOver());
        if (gameManager.IsGameOver())
        {
            recipesDelivered.text = DeliveryManager.Instance.RecipeSuccess.ToString();
        }
    }

    private void ShowAndHideVisual(bool set)
    {
        gameObject.SetActive(set);
    }
}