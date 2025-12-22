using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffSystem : MonoBehaviour
{
    public static BuffSystem Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one BuffSystem! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public event EventHandler OnEnergyBuffChanged;
    public event EventHandler OnHealthBuffChanged;
    public event EventHandler OnPowerBuffChanged;
    public event EventHandler OnResistanceBuffChanged;
    public event EventHandler OnSpeedBuffChanged;

    private bool energyBuff;
    private bool healthBuff;
    private bool powerBuff;
    private bool resistanceBuff;
    private bool speedBuff;

    private void Start()
    {
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
    }

    private void TurnSystem_OnTurnChanged(object sender, System.EventArgs e)
    {
        if (!TurnSystem.Instance.IsPlayerTurn())
        {
            return;
        }

        ResetBuffs();
    }

    public void ResetBuffs()
    {
        SetHealthBuff(false);
        SetPowerBuff(false);
        SetResistanceBuff(false);
        SetSpeedBuff(false);
    }

    public void ApplyDamageModifier(Unit unit, int modifier)
    {
        HealthSystem healthSystem = unit.GetComponent<HealthSystem>();
        healthSystem.SetDamageModifier(modifier);
    }

    public bool IsEnergyBuffActive()
    {
        return energyBuff;
    }

    public bool IsHealthBuffActive()
    {
        return healthBuff;
    }

    public bool IsPowerBuffActive()
    {
        return powerBuff;
    }

    public bool IsResistanceBuffActive()
    {
        return resistanceBuff;
    }

    public bool IsSpeedBuffActive()
    {
        return speedBuff;
    }

    public void SetEnergyBuff(bool newState)
    {
        energyBuff = newState;
        OnEnergyBuffChanged?.Invoke(this, EventArgs.Empty);
    }

    public void SetHealthBuff(bool newState)
    {
        healthBuff = newState;
        OnHealthBuffChanged?.Invoke(this, EventArgs.Empty);
    }

    public void SetPowerBuff(bool newState)
    {
        powerBuff = newState;
        OnPowerBuffChanged?.Invoke(this, EventArgs.Empty);
    }

    public void SetResistanceBuff(bool newState)
    {
        resistanceBuff = newState;
        OnResistanceBuffChanged?.Invoke(this, EventArgs.Empty);
    }

    public void SetSpeedBuff(bool newState)
    {
        speedBuff = newState;
        OnSpeedBuffChanged?.Invoke(this, EventArgs.Empty);
    }

}
