using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeAction : BaseAction
{
    [SerializeField] private Transform grenadeProjectilePrefab;
    [SerializeField] private int maxThrowDistance = 7;
    [SerializeField] private LayerMask obstaclesLayerMask;

    public event EventHandler OnThrowStarted;

    private void Update()
    {
        if (!isActive)
        {
            return;
        }        
    }

    public override string GetActionName()
    {
        return "Grenade";
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 0,
        };
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        GridPosition unitGridPosition = unit.GetGridPosition();
        return GetValidActionGridPositionList(unitGridPosition, maxThrowDistance);
    }

    public List<GridPosition> GetValidActionGridPositionList(GridPosition unitGridPosition, int maxRange)
    {
        List<GridPosition> gridPositionsInRangeList = new List<GridPosition>();

        gridPositionsInRangeList = HexRangeUtils.GetGridPositionsInRange(unitGridPosition, maxRange);
        gridPositionsInRangeList.RemoveAll(testGridPosition => !LevelGrid.Instance.IsValidGridPosition(testGridPosition));

        for (int i = 0; i < gridPositionsInRangeList.Count; i++)
        {
            GridPosition testGridPosition = gridPositionsInRangeList[i];
            Vector3 targetHex = LevelGrid.Instance.GetWorldPosition(testGridPosition);

            Vector3 unitWorldPosition = LevelGrid.Instance.GetWorldPosition(unitGridPosition);
            Vector3 shootDirection = (targetHex- unitWorldPosition).normalized;

            float unitShoulderHeight = 1.7f;
            if (Physics.Raycast(
                unitWorldPosition + Vector3.up * unitShoulderHeight,
                shootDirection,
                Vector3.Distance(unitWorldPosition, targetHex),
                obstaclesLayerMask))
            {
                // Blocked by an obstacle
                gridPositionsInRangeList.Remove(testGridPosition);
            }
        }

        return gridPositionsInRangeList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        OnThrowStarted?.Invoke(this, EventArgs.Empty);

        Transform grenadeProjectileTransform = Instantiate(grenadeProjectilePrefab, unit.GetWorldPosition(), Quaternion.identity);
        GrenadeProjectile grenadeProjectile = grenadeProjectileTransform.GetComponent<GrenadeProjectile>();
        grenadeProjectile.Setup(gridPosition, OnGrenadeBehaviourComplete);

        ActionStart(onActionComplete);
    }

    private void OnGrenadeBehaviourComplete()
    {
        ActionComplete();
    }
}
