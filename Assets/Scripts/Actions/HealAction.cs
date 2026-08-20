using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealAction : BaseAction
{
    public event EventHandler OnHealActionStarted;

    [SerializeField] private int healAmount = 20;

    private float timer;
    private bool alreadyHealed;

    private void Update()
    {
        if (!isActive)
        {
            return;
        }

        if (!alreadyHealed)
        {
            HealthSystem healthSystem = GetComponent<HealthSystem>();

            int extraHeal = 0;

            if (BuffSystem.Instance.IsHealthBuffActive())
            {
                extraHeal = 3;
                BuffSystem.Instance.SetHealthBuff(false);
            }

            healthSystem.Heal(healAmount + extraHeal);

            alreadyHealed = true;
        }

        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            ActionComplete();
        }
    }

    public override string GetActionName()
    {
        return "Heal";
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 0
        };
    }

    public override int GetActionPointsCost()
    {
        return 1;
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        GridPosition unitGridPosition = unit.GetGridPosition();

        return new List<GridPosition>
        {
            unitGridPosition,
        };
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        timer = 0.25f;
        alreadyHealed = false;

        OnHealActionStarted?.Invoke(this, EventArgs.Empty);

        ActionStart(onActionComplete);
    }
}