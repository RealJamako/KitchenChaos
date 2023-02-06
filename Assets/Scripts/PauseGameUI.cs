using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityRandom = UnityEngine.Random;

[DisallowMultipleComponent]
public class PauseGameUI : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button quitButton;

    [SerializeField] private GameObject[] pauseMenuContents;
    [SerializeField] private GameObject settingsMenu;

    private void OnEnable()
    {
        resumeButton.onClick.AddListener(() =>
        {
            GameManager.Instance.Instance_OnPause();
        });
        settingsButton.onClick.AddListener(() =>
        {
            ShowAndHidePauseContents(false);
            settingsMenu.SetActive(true);
        });
        quitButton.onClick.AddListener(() =>
        {
            Loader.LoadScene(GameScenes.MainMenuScene);
        });
        resumeButton.Select();
    }

    private void OnDisable()
    {
        resumeButton.onClick.RemoveAllListeners();
        settingsButton.onClick.RemoveAllListeners();
        quitButton.onClick.RemoveAllListeners();
    }

    private void Start()
    {
        GameManager.Instance.OnGamePaused += Instance_OnGamePaused;
        ShowAndHideVisual(false);
        settingsMenu.SetActive(false);
    }

    private void Instance_OnGamePaused()
    {
        if (!gameObject.activeInHierarchy)
        {
            ShowAndHideVisual(true);
        }
        else
        {
            ShowAndHideVisual(false);
            settingsMenu.SetActive(false);
        }
    }

    private void ShowAndHideVisual(bool set)
    {
        gameObject.SetActive(set);
    }

    public void ShowAndHidePauseContents(bool set)
    {
        foreach (var gameobject in pauseMenuContents)
        {
            gameobject.SetActive(set);
        }
    }
}