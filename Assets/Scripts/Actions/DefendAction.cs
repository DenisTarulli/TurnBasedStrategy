using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DefendActionVisual))]
public class DefendAction : BaseAction
{
    [SerializeField] private int damageMitigated = 10;
    private float timer;

    public event EventHandler OnDefendStarted;
    public event EventHandler OnDefendCompleted;

    private void Update()
    {
        if (!isActive)
        {
            return;
        }

        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            int activeBuff = 0;

            if (BuffSystem.Instance.IsResistanceBuffActive())
            {
                activeBuff = 3;
                BuffSystem.Instance.SetResistanceBuff(false);
            }

            BuffSystem.Instance.ApplyDamageModifier(unit, damageMitigated + activeBuff);
            unit.SetIsDefending(true);

            OnDefendCompleted?.Invoke(this, EventArgs.Empty);

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
        if (unit.IsDefending())
        {
            return new List<GridPosition>();
        }

        return new List<GridPosition>
        {
            unit.GetGridPosition()
        };
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        timer = 1.8f;

        OnDefendStarted?.Invoke(this, EventArgs.Empty); 

        ActionStart(onActionComplete); 
    }
}
