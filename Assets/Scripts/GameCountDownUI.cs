using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityRandom = UnityEngine.Random;
using TMPro;

public class GameCountDownUI : MonoBehaviour
{
    [SerializeField] private TMP_Text countdownText;

    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
        GameManager.Instance.OnStateChange += Instance_OnStateChange;
    }

    private void Instance_OnStateChange()
    {
        ShowAndHideVisual(gameManager.IsCountDownActive());
    }

    private void Update()
    {
        countdownText.text = gameManager.CountDownVisualTimer.ToString("0");
    }

    private void ShowAndHideVisual(bool set)
    {
        gameObject.SetActive(set);
    }
}