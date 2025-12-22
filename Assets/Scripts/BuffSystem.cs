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

    private bool healthBuff;
    private bool energyBuff;
    private bool resistanceBuff;
    private bool powerBuff;
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
        healthBuff = false;
        resistanceBuff = false;
        powerBuff = false;
        speedBuff = false;
    }

    public void ApplyDamageModifier(Unit unit, int modifier)
    {
        HealthSystem healthSystem = unit.GetComponent<HealthSystem>();
        healthSystem.SetDamageModifier(modifier);
    }

    public bool IsHealthBuffActive()
    {
        return healthBuff;
    }

    public bool IsEnergyBuffActive()
    {
        return energyBuff;
    }

    public bool IsResistanceBuffActive()
    {
        return resistanceBuff;
    }

    public bool IsPowerBuffActive()
    {
        return powerBuff;
    }

    public bool IsSpeedBuffActive()
    {
        return speedBuff;
    }

    public void SetHealthBuff(bool newState)
    {
        healthBuff = newState;
    }

    public void SetEnergyBuff(bool newState)
    {
        energyBuff = newState;
    }

    public void SetResistanceBuff(bool newState)
    {
        resistanceBuff = newState;
    }

    public void SetPowerBuff(bool newState)
    {
        powerBuff = newState;
    }

    public void SetSpeedBuff(bool newState)
    {
        speedBuff = newState;
    }

}
