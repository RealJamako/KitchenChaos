using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityRandom = UnityEngine.Random;

[DisallowMultipleComponent]
public class SettingsUI : MonoBehaviour
{
    [Header("Main Settings Container")]
    [SerializeField] private Button muteMusicButton;
    [SerializeField] private Button muteSoundEffectsButton;
    [SerializeField] private Button backButton;
    [SerializeField] private Button rebindButton;

    [Header("Rebind Settings Container")]
    [SerializeField] private Button rebindBackButton;
    [SerializeField] private Button rebindActionA;
    [SerializeField] private Button rebindActionB;
    [SerializeField] private Button rebindPause;
    [SerializeField] private GameObject rebindAwaitingText;
    [SerializeField] private TMP_Text rebindActionAText;
    [SerializeField] private TMP_Text rebindActionBText;
    [SerializeField] private TMP_Text rebindPauseText;

    [Header("Menu / Containers")]
    [SerializeField] private GameObject settingsContainer;
    [SerializeField] private GameObject keyrebindContainer;

    [SerializeField] private TMP_Text headerTitle;

    private PauseGameUI pauseMenuUI;

    private const string MAIN_SETTINGS_TITLE = "Settings";
    private const string REBINDING_TITLE = "Rebind Controls";

    private void Awake()
    {
        pauseMenuUI = GetComponentInParent<PauseGameUI>();
    }

    private void OnEnable()
    {
        muteSoundEffectsButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.MuteVolume = !SoundManager.Instance.MuteVolume;
        });
        muteMusicButton.onClick.AddListener(() =>
        {
            MusicManager.Instance.MuteVolume = !MusicManager.Instance.MuteVolume;
        });
        backButton.onClick.AddListener(() =>
        {
            pauseMenuUI.ShowAndHidePauseContents(true);
            gameObject.SetActive(false);
        });
        rebindButton.onClick.AddListener(() =>
        {
            SetHeaderText(REBINDING_TITLE);
            SetKeyBindText();
            settingsContainer.SetActive(false);
            keyrebindContainer.SetActive(true);
        });
        rebindBackButton.onClick.AddListener(() =>
        { 
            SetHeaderText(MAIN_SETTINGS_TITLE);
            settingsContainer.SetActive(true);
            keyrebindContainer.SetActive(false);
        });


        //Rebindings
        rebindActionA.onClick.AddListener(() =>
        {
            RebindBinding(KeyBindings.ActionA);
        });
        rebindActionB.onClick.AddListener(() =>
        {
            RebindBinding(KeyBindings.ActionA);
        });
        rebindPause.onClick.AddListener(() =>
        {
            RebindBinding(KeyBindings.ActionA);
        });
        settingsContainer.SetActive(true);
        SetHeaderText(MAIN_SETTINGS_TITLE);
        rebindButton.Select();
    }

    private void Start()
    {
        keyrebindContainer.SetActive(false);
        ShowAndHideAwaitingRebind(false);
    }

    private void OnDisable()
    {
        muteSoundEffectsButton.onClick.RemoveAllListeners();
        muteMusicButton.onClick.RemoveAllListeners();
        backButton.onClick.RemoveAllListeners();
        rebindButton.onClick.RemoveAllListeners();

        //Rebindings
        rebindActionA.onClick.RemoveAllListeners();
        rebindActionB.onClick.RemoveAllListeners();
        rebindPause.onClick.RemoveAllListeners();

        keyrebindContainer.SetActive(false);
    }

    private void SetHeaderText(string text)
    {
        headerTitle.text = text.ToUpper();
    }

    private void SetKeyBindText()
    {
        rebindActionAText.text = PlayerInput.Instance.GetBindingText(KeyBindings.ActionA);
        rebindActionBText.text = PlayerInput.Instance.GetBindingText(KeyBindings.ActionB);
        rebindPauseText.text = PlayerInput.Instance.GetBindingText(KeyBindings.Pause);
    }

    private void ShowAndHideAwaitingRebind(bool set)
    {
        rebindAwaitingText.SetActive(set);
    }

    private void RebindBinding(KeyBindings binding)
    {
        ShowAndHideAwaitingRebind(true);
        PlayerInput.Instance.RebindBinding(binding, (x) => {
            ShowAndHideAwaitingRebind(x);
            SetKeyBindText();
        });
    }
}