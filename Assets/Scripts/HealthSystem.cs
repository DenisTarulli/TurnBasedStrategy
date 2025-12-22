using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public event EventHandler OnDead;
    public event EventHandler OnHealthAmountChange;

    [SerializeField] private int health = 100;
    private Unit unit;
    private int healthMax;
    private int damageModifier;

    private void Awake()
    {
        healthMax = health;
        unit = GetComponent<Unit>();
    }

    private void Start()
    {
        if (!unit.IsEnemy())
        {
            int newHealth = PlayerStats.Instance.GetHealth();
            healthMax = newHealth;
            health = healthMax;
            PlayerStats.Instance.OnHealthChanged += PlayerStats_OnHealthChanged;
        }

        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
    }

    private void PlayerStats_OnHealthChanged(object sender, EventArgs e)
    {
        healthMax = PlayerStats.Instance.GetHealth();
        GetComponentInChildren<UnitWorldUI>().UpdateHealthBar();
        Debug.Log(healthMax);
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        if (!TurnSystem.Instance.IsPlayerTurn())
        {
            return;
        }

        ResetDamageModifier();
    }

    public void Damage(int damageAmount)
    {
        if (damageAmount > damageModifier)
        {
            health -= damageAmount - damageModifier;
        }

        if (health < 0)
        {
            health = 0;
        }

        OnHealthAmountChange?.Invoke(this, EventArgs.Empty);

        if (health == 0)
        {
            Die();
        }

        Debug.Log(health);
    }

    private void Die()
    {
        OnDead?.Invoke(this, EventArgs.Empty);
    }

    public float GetHealthNormalized()
    {
        return (float)health / healthMax;
    }

    public void Heal(int healAmount)
    {
        health += healAmount;
        OnHealthAmountChange?.Invoke(this, EventArgs.Empty);

        if (health > healthMax)
        {
            health = healthMax;
        }

        Debug.Log(health);
    }

    public int GetDamageModifier()
    {
        return damageModifier;
    }

    public void SetDamageModifier(int damageModifier)
    {
        this.damageModifier = damageModifier;
    }

    private void ResetDamageModifier()
    {
        damageModifier = 0;
    }
}
