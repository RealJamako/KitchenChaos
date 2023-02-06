using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityRandom = UnityEngine.Random;

[DisallowMultipleComponent]
public class DeliveryManagerUI : MonoBehaviour
{
    [SerializeField] private GameObject container;
    [SerializeField] private GameObject recipeTemplate;

    private void Awake()
    {
        recipeTemplate.SetActive(false);
    }

    private void Start()
    {
        DeliveryManager.Instance.OnRecipeStart += UpdateVisual;
        DeliveryManager.Instance.OnRecipeFinish += UpdateVisual;
        UpdateVisual();
    }

    private void OnDisable()
    {
        DeliveryManager.Instance.OnRecipeStart -= UpdateVisual;
        DeliveryManager.Instance.OnRecipeFinish -= UpdateVisual;
    }

    private void UpdateVisual()
    {
        foreach (Transform child in container.transform)
        {
            if (child == recipeTemplate.transform) continue;
            Destroy(child.gameObject);
        }

        foreach (var item in DeliveryManager.Instance.WaitingRecipes)
        {
            var spawnedItem = Instantiate(recipeTemplate, container.transform);
            spawnedItem.SetActive(true);
            spawnedItem.GetComponent<DeliveryRecipeSetter>().SetRecipeSO(item);
        }
    }
}