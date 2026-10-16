using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one PlayersStats! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public event EventHandler OnHealthChanged;
    public event EventHandler OnPowerChanged;
    public event EventHandler OnSpeedChanged;
    public event EventHandler OnEnergyChanged;
    public event EventHandler OnResistanceChanged;
    public event EventHandler OnExpChanged;
    public event EventHandler OnLevelUp;

    [SerializeField] private int health;
    [SerializeField] private int power;
    [SerializeField] private int speed;
    [SerializeField] private int energy;
    [SerializeField] private int resistance;
    [SerializeField] private int exp;
    [SerializeField] private int expToLevelUp = 2;
    [SerializeField] private int level;

    private int expRequirementAddition = 2;

    public void ChangeHealth(int amount)
    {
        health += amount;
        OnHealthChanged?.Invoke(this, EventArgs.Empty);
    }

    public void ChangePower(int amount)
    {
        power += amount;
        OnPowerChanged?.Invoke(this, EventArgs.Empty);
    }

    public void ChangeSpeed(int amount)
    {
        speed += amount;
        OnSpeedChanged?.Invoke(this, EventArgs.Empty);
    }

    public void ChangeEnergy(int amount)
    {
        energy += amount;
        OnEnergyChanged?.Invoke(this, EventArgs.Empty);
    }

    public void ChangeResistance(int amount)
    {
        resistance += amount;
        OnResistanceChanged?.Invoke(this, EventArgs.Empty);
    }

    public void ChangeExp(int amount)
    {
        exp += amount;

        if (exp >= expToLevelUp)
        {
            int spareExp = exp - expToLevelUp;
            expToLevelUp += expRequirementAddition;
            exp = spareExp;
            level++;

            OnLevelUp?.Invoke(this, EventArgs.Empty);
        }

        OnExpChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetHealth()
    {
        return health;
    }

    public int GetPower()
    {
        return power;
    }

    public int GetSpeed()
    {
        return speed;
    }

    public int GetEnergy()
    {
        return energy;
    }

    public int GetResistance()
    {
        return resistance;
    }

    public int GetExp()
    {
        return exp;
    }

    public int GetExpToLevelUp()
    {
        return expToLevelUp;
    }

    public int GetLevel()
    {
        return level;
    }

    public float GetExpNormalized()
    {
        return (float)exp / expToLevelUp;
    }
}
