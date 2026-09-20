using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
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
        Time.timeScale = 1f;
    }

    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;

    private bool isGamePaused = false;
    private bool isGameOver = false;

    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject gameWinUI;

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
        gameOverUI.SetActive(true);
        isGameOver = true;

        Debug.Log("GAME OVER");
        Time.timeScale = 0f;
    }

    public void GameWin()
    {
        gameWinUI.SetActive(true);
        isGameOver = true;

        Debug.Log("GAME WIN");
        Time.timeScale = 0f;
    }

    public bool IsGameOver()
    {
        return isGameOver;
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        Loader.Load(Loader.Scene.MainMenuScene);
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        Loader.Load(Loader.Scene.GameScene);
    }
}