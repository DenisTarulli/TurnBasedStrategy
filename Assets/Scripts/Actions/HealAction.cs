using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealAction : BaseAction
{
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
            healthSystem.Heal(healAmount);

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

        ActionStart(onActionComplete);
    }
}
