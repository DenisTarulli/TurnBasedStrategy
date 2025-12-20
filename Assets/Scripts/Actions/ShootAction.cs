using System;
using System.Collections.Generic;
using UnityEngine;

public class ShootAction : BaseAction
{
    public event EventHandler<OnShootEventArgs> OnShoot;
    public static event EventHandler<OnShootEventArgs> OnAnyShoot;
    public class OnShootEventArgs : EventArgs
    {
        public Unit targetUnit;
        public Unit shootingUnit;
    }

    public static new void ResetStaticData()
    {
        OnAnyShoot = null;
    }

    private enum State
    {
        Aiming,
        Shooting,
        Cooloff
    }

    [SerializeField] private LayerMask obstaclesLayerMask;
    [SerializeField] private int damageToDeal = 40;
    [SerializeField] private int maxShootDistance = 7;
    private State state;
    private float stateTimer;
    private Unit targetUnit;
    private bool canShootBullet;

    private void Update()
    {
        if (!isActive)
        {
            return;
        }

        stateTimer -= Time.deltaTime;

        switch (state)
        {
            case State.Aiming:
                RotateTowardsTarget();
                break;
            case State.Shooting:
                if (canShootBullet)
                {
                    Shoot();
                    canShootBullet = false;
                }
                break;
            case State.Cooloff:
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
            case State.Aiming:
                state = State.Shooting;
                float shootingStateTime = 0.1f;
                stateTimer = shootingStateTime;
                break;
            case State.Shooting:
                state = State.Cooloff;
                float cooloffStateTime = 0.8f;
                stateTimer = cooloffStateTime;
                break;
            case State.Cooloff:
                ActionComplete();
                break;
        }
    }

    private void Shoot()
    {
        targetUnit.Damage(damageToDeal);

        OnAnyShoot?.Invoke(this, new OnShootEventArgs
        {
            targetUnit = targetUnit,
            shootingUnit = unit
        });

        OnShoot?.Invoke(this, new OnShootEventArgs
        {
            targetUnit = targetUnit,
            shootingUnit = unit
        });
    }

    private void RotateTowardsTarget()
    {
        Vector3 aimDirection = (targetUnit.GetWorldPosition() - transform.position).normalized;

        float rotateSpeed = 10f;
        transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * rotateSpeed);
    }

    public override string GetActionName()
    {
        return "Shoot";
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        GridPosition unitGridPosition = unit.GetGridPosition();
        return GetValidActionGridPositionList(unitGridPosition, maxShootDistance);
    }

    public List<GridPosition> GetValidActionGridPositionList(GridPosition unitGridPosition, int maxRange)
    {
        List<GridPosition> gridPositionsInRangeList = new List<GridPosition>();

        gridPositionsInRangeList = HexRangeUtils.GetGridPositionsInRange(unitGridPosition, maxRange);
        gridPositionsInRangeList.Remove(unitGridPosition);
        gridPositionsInRangeList.RemoveAll(testGridPosition => !LevelGrid.Instance.IsValidGridPosition(testGridPosition));
        gridPositionsInRangeList.RemoveAll(testGridPosition => !LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition));

        List<GridPosition> gridPositionsToRemove = new List<GridPosition>();

        for (int i = 0; i < gridPositionsInRangeList.Count; i++)
        {
            GridPosition testGridPosition = gridPositionsInRangeList[i];
            Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);

            if (targetUnit.IsEnemy() == unit.IsEnemy())
            {
                gridPositionsToRemove.Add(testGridPosition);
                continue;
            }

            Vector3 unitWorldPosition = LevelGrid.Instance.GetWorldPosition(unitGridPosition);
            Vector3 shootDirection = (targetUnit.GetWorldPosition() - unitWorldPosition).normalized;

            float unitShoulderHeight = 1.7f;
            if (Physics.Raycast(
                unitWorldPosition + Vector3.up * unitShoulderHeight,
                shootDirection,
                Vector3.Distance(unitWorldPosition, targetUnit.GetWorldPosition()),
                obstaclesLayerMask))
            {
                // Blocked by an obstacle
                gridPositionsInRangeList.Remove(testGridPosition);
            }
        }

        HashSet<GridPosition> elementsToRemove = new HashSet<GridPosition>(gridPositionsToRemove);
        gridPositionsInRangeList.RemoveAll(gridPosition => elementsToRemove.Contains(gridPosition));

        return gridPositionsInRangeList;
    }


    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        state = State.Aiming;
        float aimingStateTime = 1f;
        stateTimer = aimingStateTime;

        canShootBullet = true;

        ActionStart(onActionComplete);
    }

    public Unit GetTargetUnit()
    {
        return targetUnit;
    }

    public int GetMaxShootDistance()
    {
        return maxShootDistance;
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 100 + Mathf.RoundToInt((1 - targetUnit.GetHealthNormalized()) * 100f)
        };
    }

    public int GetTargetCountAtPosition(GridPosition gridPosition)
    {
        return GetValidActionGridPositionList(gridPosition, maxShootDistance).Count;
    }
}
