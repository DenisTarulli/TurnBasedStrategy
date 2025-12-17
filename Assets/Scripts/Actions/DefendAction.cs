using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DefendActionVisual))]
public class DefendAction : BaseAction
{
    [SerializeField] private int damageMitigated = 10;
    private float timer;

    public event EventHandler OnDefendStateChanged;

    private void Update()
    {
        if (!isActive)
        {
            return;
        }

        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            BuffSystem.Instance.ApplyDamageModifier(unit, damageMitigated);
            OnDefendStateChanged?.Invoke(this, EventArgs.Empty);

            ActionComplete();
        }
    }

    public override string GetActionName()
    {
        return "Defend";
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 0
        };
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

        ActionStart(onActionComplete);
    }
}
