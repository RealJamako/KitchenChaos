using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityRandom = UnityEngine.Random;
using TMPro;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class DeliveryRecipeSetter : MonoBehaviour
{
    [SerializeField] private TMP_Text recipeNameText;
    [SerializeField] private GameObject iconContainer;
    [SerializeField] private GameObject iconTemplate;

    private void Awake()
    {
        iconTemplate.SetActive(false);
    }

    public void SetRecipeSO(KitchenRecipeSO recipeSO)
    {
        recipeNameText.text = recipeSO.RecipeName;

        foreach (Transform child in iconContainer.transform)
        {
            if (child == iconTemplate.transform) continue;
            Destroy(child.gameObject);
        }

        foreach (KitchenObjectSO kitchenObjectSO in recipeSO.kitchenObjectSOList)
        {
            GameObject iconGameObject = Instantiate(iconTemplate, iconContainer.transform);
            iconGameObject.SetActive(true);
            iconGameObject.GetComponent<Image>().sprite = kitchenObjectSO.Sprite;
        }   
    }
}