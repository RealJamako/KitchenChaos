using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityRandom = UnityEngine.Random;

[DisallowMultipleComponent]
public class CounterVisual : MonoBehaviour
{
    [SerializeField] private BaseCounter counter;
    [SerializeField] private GameObject[] visualObject;

    private void Start()
    {
        PlayerController.Instance.OnSelectedCounterChange += Player_OnSelectedCounterChange;
        ShowOrHideVisual(false);
    }

    private void OnDisable()
    {
        PlayerController.Instance.OnSelectedCounterChange -= Player_OnSelectedCounterChange;
    }

    private void Player_OnSelectedCounterChange(object sender, OnSelectedCounterChangedArgs e)
    {
        if (e.SelectedCounter == counter)
        {
            ShowOrHideVisual(true);
        }
        else
        {
            ShowOrHideVisual(false);
        }
    }

    private void ShowOrHideVisual(bool set)
    {
        foreach (var item in visualObject) 
        {
            item.SetActive(set);
        }
    }
}