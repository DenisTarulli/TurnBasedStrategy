using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealAction : BaseAction
{
    [SerializeField] private int healAmount = 20;
    [SerializeField] private float healDelay = 2f;

    private float timer;

    public event EventHandler OnHealStarted;


    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            if (!isActive) return;

            HealthSystem healthSystem = GetComponent<HealthSystem>();

            int extraHeal = 0;
            if (BuffSystem.Instance.IsHealthBuffActive())
            {
                extraHeal = 3;
                BuffSystem.Instance.SetHealthBuff(false);
            }

            healthSystem.Heal(healAmount + extraHeal);

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
        timer = healDelay;

        OnHealStarted?.Invoke(this, EventArgs.Empty);

        ActionStart(onActionComplete);
    }
}
