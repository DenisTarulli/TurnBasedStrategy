using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public event EventHandler OnDead;
    public event EventHandler OnHealthAmountChange;
    public event EventHandler OnDamaged;

    [SerializeField] private int health = 20;
    private Unit unit;
    private int healthMax;
    private int initialHealthMax;
    private int damageModifier;

    private void Awake()
    {
        healthMax = health;
        initialHealthMax = healthMax;
        unit = GetComponent<Unit>();
    }

    private void Start()
    {
        if (!unit.IsEnemy())
        {
            healthMax = health;
            PlayerStats.Instance.OnHealthChanged += PlayerStats_OnHealthChanged;
        }

        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
    }

    private void PlayerStats_OnHealthChanged(object sender, EventArgs e)
    {
        healthMax = initialHealthMax + PlayerStats.Instance.GetHealth();
        unit.gameObject.GetComponentInChildren<UnitWorldUI>().UpdateHealthBar();
        unit.GetComponentInChildren<UnitWorldUI>().UpdateHealthBar();
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
        Debug.Log($"damageAmount: {damageAmount}");
        if (damageAmount > damageModifier)
        {
            int amountToDamage = 1;

            if (!unit.IsEnemy())
            {
                amountToDamage = -damageModifier - PlayerStats.Instance.GetResistance();
            }

            amountToDamage += damageAmount;

            health -= amountToDamage;

            OnDamaged?.Invoke(this, EventArgs.Empty);
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

    public int GetCurrentHealth()
    {
        return health;
    }

    public int GetMaxHealth()
    {
        return healthMax;
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