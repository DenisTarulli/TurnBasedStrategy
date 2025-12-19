using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class SwordAction : BaseAction
{
    public event EventHandler OnSwordActionStarted;
    public event EventHandler OnSwordActionCompleted;
    public static event EventHandler OnAnySwordHit;

    public static new void ResetStaticData()
    {
        OnAnySwordHit = null;
    }

    private int maxSwordDistance = 1;

    private enum State
    {
        SwingingSwordBeforeHit,
        SwingingSwordAfterHit
    }

    private State state;
    private float stateTimer;
    private Unit targetUnit;

    private void Update()
    {
        if (!isActive)
        {
            return;
        }

        stateTimer -= Time.deltaTime;

        switch (state)
        {
            case State.SwingingSwordBeforeHit:
                RotateTowardsTarget();
                break;
            case State.SwingingSwordAfterHit:
                break;
        }

        if (stateTimer <= 0f)
        {
            NextState();
        }
    }

    private void NextState()
    {
        switch (state)
        {
            case State.SwingingSwordBeforeHit:
                state = State.SwingingSwordAfterHit;
                float afterHitStateTime = 0.5f;
                stateTimer = afterHitStateTime;
                int damage = 100;
                targetUnit.Damage(damage);
                OnAnySwordHit?.Invoke(this, EventArgs.Empty);
                break;
            case State.SwingingSwordAfterHit:
                OnSwordActionCompleted?.Invoke(this, EventArgs.Empty);
                ActionComplete();
                break;
        }
    }

    private void RotateTowardsTarget()
    {
        Vector3 aimDirection = (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;

        float rotateSpeed = 10f;
        transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * rotateSpeed);
    }

    public override string GetActionName()
    {
        return "Sword";
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 200
        };
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = unit.GetGridPosition();

        for (int x = -maxSwordDistance; x <= maxSwordDistance; x++)
        {
            for (int z = -maxSwordDistance; z <= maxSwordDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (unitGridPosition.z % 2 != 0)
                {
                    // Unit is on odd row
                    if (x == -maxSwordDistance && z != 0)
                    {
                        // Hex is out of action range
                        continue;
                    }
                }
                else
                {
                    // Unit is on even row
                    if (x == maxSwordDistance && z != 0)
                    {
                        // Hex is out of action range
                        continue;
                    }
                }

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    // Cell is outside of the grid bounds
                    continue;
                }

                if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    // Grid position is empty, no Unit
                    continue;
                }

                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);

                if (targetUnit.IsEnemy() == unit.IsEnemy())
                {
                    // Both Units are on the same "team"
                    continue;
                }

                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        state = State.SwingingSwordBeforeHit;
        float afterHitStateTime = 0.7f;
        stateTimer = afterHitStateTime;

        OnSwordActionStarted?.Invoke(this, EventArgs.Empty);

        ActionStart(onActionComplete);
    }

    public int GetMaxSwordDistance()
    {
        return maxSwordDistance;
    }
}
