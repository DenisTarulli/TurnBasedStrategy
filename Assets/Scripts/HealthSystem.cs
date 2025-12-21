using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public event EventHandler OnDead;
    public event EventHandler OnHealthAmountChange;

    [SerializeField] private int health = 100;
    private int healthMax;
    private int damageModifier;

    private void Awake()
    {
        healthMax = health;
    }

    private void Start()
    {
        if (!GetComponent<Unit>().IsEnemy())
        {
            healthMax = PlayerStats.Instance.GetHealth();
        }

        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
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
        health -= damageAmount - damageModifier;

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
