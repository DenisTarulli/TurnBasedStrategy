using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private bool isPlayerDead;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one GameManager! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;

    private bool isGamePaused = false;
    private bool isGameOver = false;
    private bool isGameWon = false;

    [Header("UI")]
    [SerializeField] public GameObject gameOverUI;
    [SerializeField] public GameObject gameWonUI;

    private void Update()
    {
        if (isGameOver || isGameWon)
        {
            return;
        }

        if (InputManager.Instance.IsEscapeButtonDownThisFrame())
        {
            TogglePauseGame();
        }
    }

    // =========================
    // PAUSA
    // =========================
    public void TogglePauseGame()
    {
        if (ShopSystem.Instance.IsShopOpen())
        {
            return;
        }

        isGamePaused = !isGamePaused;

        if (isGamePaused)
        {
            Time.timeScale = 0f;
            OnGamePaused?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            Time.timeScale = 1f;
            OnGameUnpaused?.Invoke(this, EventArgs.Empty);
        }
    }

    // =========================
    // GAME OVER (DERROTA)
    // =========================
    public void GameOver()
    {
        if (isGameOver || isGameWon) return;
        StartCoroutine(GameOverAfterDelay());
    }

    private IEnumerator GameOverAfterDelay()
    {
        yield return new WaitForSecondsRealtime(3f);

        gameOverUI.SetActive(true);
        isGameOver = true;

        Debug.Log("GAME OVER");
        Time.timeScale = 0f;
    }

    // =========================
    // GAME WON (VICTORIA)
    // =========================
    public void GameWon()
    {
        if (isGameWon || isGameOver) return;
        StartCoroutine(GameWonAfterDelay());
    }

    private IEnumerator GameWonAfterDelay()
    {
        yield return new WaitForSecondsRealtime(3f);

        gameWonUI.SetActive(true);
        isGameWon = true;

        Debug.Log("GAME WON");
        Time.timeScale = 0f;
    }

    // =========================
    // GETTERS
    // =========================
    public bool IsGameOver()
    {
        return isGameOver;
    }

    public bool IsGameWon()
    {
        return isGameWon;
    }

    public void SetPlayerDead()
    {
        isPlayerDead = true;
    }

    public bool IsPlayerDead()
    {
        return isPlayerDead;
    }

    // =========================
    // UI BUTTONS
    // =========================
    public void MainMenu()
    {
        Time.timeScale = 1f;
        Loader.Load(Loader.Scene.MainMenuScene);
        SoundManager.Instance.RestartMusic();
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        StaticReset.ResetAll();
        Loader.Load(Loader.Scene.GameScene);
        SoundManager.Instance.RestartMusic();
    }

    public void StartGameOverWithDelay(float delay)
    {
        StartCoroutine(GameOverCoroutine(delay));
    }

    private IEnumerator GameOverCoroutine(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        GameOver();
    }
}