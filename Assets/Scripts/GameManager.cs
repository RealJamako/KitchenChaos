using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityRandom = UnityEngine.Random;

[DisallowMultipleComponent]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public event Action OnGamePaused;

    public float CountDownVisualTimer { get { return countDownVisualTimer; } }

    public float CountDownTimerNormalized 
    {
        get
        {
            return 1 - (gameplayStateTime / gameplayStateTimeMax);
        }
    }

    public event Action OnStateChange;

    private GameStates gameState;

    private readonly float waitingStateTime = 1f;
    private readonly float countdownStateTime = 3f;
    private readonly float gameOverStateTime = 4f;
    private readonly float gameplayStateTimeMax = 120f;
    private float gameplayStateTime;

    private float countDownVisualTimer = 3f;

    private bool isGamePaused;

    private void Awake()
    {
        isGamePaused = false;
        InstanceCheck();
    }

    private void Start()
    {
        PlayerInput.Instance.OnPause += Instance_OnPause;
        ShowAndHideCursor(false);
        StartNextGameState(GameStates.Waiting);
    }

    public void Instance_OnPause()
    {
        isGamePaused = !isGamePaused;
        ShowAndHideCursor(isGamePaused);
        OnGamePaused?.Invoke();
        Time.timeScale = isGamePaused ? 0 : 1;
    }

    private void StartNextGameState(GameStates state)
    {
        gameState = state;
        switch (gameState)
        {
            case GameStates.Waiting:
                StartCoroutine(SetNextGameState(GameStates.Countdown, waitingStateTime));
                break;
            case GameStates.Countdown:
                StartCoroutine(SetNextGameState(GameStates.Playing, countdownStateTime));
                StartCoroutine(StartCountDown());
                break;
            case GameStates.Playing:
                gameplayStateTime = gameplayStateTimeMax;
                StartCoroutine(SetNextGameState(GameStates.GameOver, gameplayStateTime));
                StartCoroutine(StartGamePlayCountDown());
                break;
            case GameStates.GameOver:
                StartCoroutine(AwaitSceneChange(gameOverStateTime, (x) => Loader.LoadScene(x)));
                break;
            default:
                break;
        }
        OnStateChange?.Invoke();
    }

    public bool IsGamePlaying()
    {
        return gameState == GameStates.Playing;
    }

    public bool IsCountDownActive()
    {
        return gameState == GameStates.Countdown;
    }

    public bool IsGameOver()
    {
        return gameState == GameStates.GameOver;
    }

    private IEnumerator SetNextGameState(GameStates state, float waitingTime)
    {
        yield return new WaitForSeconds(waitingTime);
        StartNextGameState(state);
    }

    private IEnumerator AwaitSceneChange(float waitingTime, Action<GameScenes> sceneTarget)
    {
        yield return new WaitForSeconds(waitingTime);
        sceneTarget(GameScenes.MainMenuScene);
    }

    private IEnumerator StartCountDown()
    {
        while (countDownVisualTimer > 0)
        {
            countDownVisualTimer -= Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator StartGamePlayCountDown()
    {
        while (gameplayStateTime > 0)
        {
            gameplayStateTime -= Time.deltaTime;
            yield return null;
        }
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

    public void ShowAndHideCursor(bool set)
    {
        Cursor.visible = set;
        print($"Cursor Visable: {set}");
    }
}