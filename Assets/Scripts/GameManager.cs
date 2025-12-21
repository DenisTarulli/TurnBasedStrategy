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
    }

    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;

    private bool isGamePaused = false;

    private void Update()
    {
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
}
