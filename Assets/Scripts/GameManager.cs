using System;
using System.Collections;
using System.Collections.Generic;
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

    [SerializeField] private GameObject gameOverUI;

    private void Update()
    {
        if (isGameOver)
        {
            return;
        }

        if (InputManager.Instance.IsEscapeButtonDownThisFrame())
        {
            TogglePauseGame();
        }
    }

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

    public void GameOver()
    {
        StartCoroutine(GameOverAfterDelay());
    }

    private IEnumerator GameOverAfterDelay()
    {
        yield return new WaitForSecondsRealtime(5f);

        gameOverUI.SetActive(true);
        isGameOver = true;

        Debug.Log("GAME OVER");
        Time.timeScale = 0f;
    }


    public bool IsGameOver()
    {
        return isGameOver;
    }

    public void MainMenu()
    {
        Loader.Load(Loader.Scene.MainMenuScene);
    }

    public void Restart()
    {
        Loader.Load(Loader.Scene.GameScene);
    }

    public void StartGameOverWithDelay(float delay)
    {
        StartCoroutine(GameOverCoroutine(delay));
    }

    private IEnumerator GameOverCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        GameOver();
    }
    public void SetPlayerDead()
    {
        isPlayerDead = true;
    }

    public bool IsPlayerDead()
    {
        return isPlayerDead;
    }


}
