using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityRandom = UnityEngine.Random;

[DisallowMultipleComponent]
public class DeliveryManager : MonoBehaviour
{
    public event Action OnRecipeStart;
    public event Action OnRecipeFinish;

    public event Action OnRecipeComplete;
    public event Action OnRecipeFailed;

    public static DeliveryManager Instance { get; private set; }

    [SerializeField] private KitchenRecipeListSO kitchenRecipeList;
    private List<KitchenRecipeSO> waitingRecipes = new();

    public List<KitchenRecipeSO> WaitingRecipes { get { return waitingRecipes; } }

    public int RecipeSuccess { get { return recipeSuccess; }}

    [Range(0f, 15f)]
    [SerializeField] private float spawnRecipeTimerMax;

    [Range(0,5)]
    [SerializeField] private int spawnRecipeCountMax;

    private int spawnRecipeCount;

    private int recipeSuccess;

    private void Awake()
    {
        InstanceCheck();
    }

    private void Start()
    {
        spawnRecipeCount = 0;
        StartCoroutine(SpawnRecipe());
    }

    private IEnumerator SpawnRecipe()
    {
        while (true)
        {
            if (spawnRecipeCount < spawnRecipeCountMax)
            {
                float time = UnityRandom.Range(0, spawnRecipeTimerMax);
                yield return new WaitForSeconds(time);
                KitchenRecipeSO waitingRecipe = kitchenRecipeList.RecipeSOList[UnityRandom.Range(0, kitchenRecipeList.RecipeSOList.Count)];
                waitingRecipes.Add(waitingRecipe);
                spawnRecipeCount++;
                OnRecipeStart?.Invoke();
            }
            yield return null;
        }
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        for (int i = 0; i < waitingRecipes.Count; i++)
        {
            KitchenRecipeSO waitingRecipeSO = waitingRecipes[i];

            if (waitingRecipeSO.kitchenObjectSOList.Count == plateKitchenObject.KitchenObjectSOList.Count)
            {
                bool plateContentsMatchRecipe = true;
                foreach (KitchenObjectSO recipeKitchenObjectSO in waitingRecipeSO.kitchenObjectSOList)
                {
                    bool ingredientFound = false;
                    foreach (KitchenObjectSO plateKitchenObjectSO in plateKitchenObject.KitchenObjectSOList)
                    {
                        if (plateKitchenObjectSO == recipeKitchenObjectSO)
                        {
                            ingredientFound = true;
                            break;
                        }
                    }
                    if (!ingredientFound)
                    {
                        plateContentsMatchRecipe = false;
                    }
                }
                if (plateContentsMatchRecipe)
                {
                    RemoveRecipe(i);
                    recipeSuccess++;
                    OnRecipeFinish?.Invoke();
                    OnRecipeComplete?.Invoke();
                    return;
                }
            }
        }
        //Receipt failed
        OnRecipeFailed?.Invoke();
    }

    private void RemoveRecipe(int indexToRemove)
    {
        Debug.Log("Player delivered recipe.");
        waitingRecipes.RemoveAt(indexToRemove);
        spawnRecipeCount--;
    }

    private void InstanceCheck()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one player instance");
            return;
        }
        else
        {
            Instance = this;
        }
    }
}