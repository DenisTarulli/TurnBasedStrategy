using System;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : BaseAction
{
    public event EventHandler OnStartMoving;
    public event EventHandler OnStopMoving;

    [SerializeField] private int maxMoveDistance = 4;
    [SerializeField] private bool meleeUnit;

    private List<Vector3> positionList;
    private int currentPositionIndex;

    private void Update()
    {
        if (!isActive)
        {
            return;
        }

        Vector3 targetPosition = positionList[currentPositionIndex];
        Vector3 moveDirection = (targetPosition - transform.position).normalized;

        float rotateSpeed = 10f;
        transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);

        float stoppingDistance = 0.1f;
        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {
            float moveSpeed = 4f;            

            transform.position += moveSpeed * Time.deltaTime * moveDirection;
        }
        else
        {
            transform.position = targetPosition;
            currentPositionIndex++;

            if (currentPositionIndex >= positionList.Count)
            {
                OnStopMoving?.Invoke(this, EventArgs.Empty);

                ActionComplete();
            }            
        }
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        List<GridPosition> pathGridPositionList = Pathfinding.Instance.GetCachedPath(gridPosition);

        currentPositionIndex = 0;
        positionList = new List<Vector3>();

        foreach (GridPosition pathGridPosition in pathGridPositionList)
        {
            positionList.Add(LevelGrid.Instance.GetWorldPosition(pathGridPosition));
        }

        OnStartMoving?.Invoke(this, EventArgs.Empty);

        ActionStart(onActionComplete);
    }

    /// <summary>
    /// Cycles through all the of the potential grid positions within the maximum move distance
    /// of the unit and returns a list of all the grid positions that meet the requirements
    /// </summary>
    /// <returns><see cref="List{GridPosition}"/> of type <see cref="GridPosition"/></returns>
    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = unit.GetGridPosition();

        for (int x = -maxMoveDistance; x <= maxMoveDistance; x++)
        {
            for (int z = -maxMoveDistance; z <= maxMoveDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    // Cell is outside of the grid bounds
                    continue;
                }

                if (unitGridPosition == testGridPosition)
                {
                    // Same grid position where the unit is already at
                    continue;
                }

                if (LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    // Grid position already occupied with another Unit
                    continue;
                }
                
                int pathFindingDistanceMultiplier = 10;
                if (Pathfinding.Instance.GetPathLength(unitGridPosition, testGridPosition) > maxMoveDistance * pathFindingDistanceMultiplier)
                {
                    // Path length is too long
                    continue;
                }

                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public override string GetActionName()
    {
        return "Move";
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        int targetCountAtGridPosition;
        int pathLengthMod = 0;

        if (meleeUnit)
        {
            targetCountAtGridPosition = unit.GetAction<SwordAction>().GetTargetCountAtPosition(gridPosition);

            if (targetCountAtGridPosition != 0)
            {
                int pathLengthModDiv = 10;
                int pathLength = Pathfinding.Instance.GetPathLength(unit.GetGridPosition(), gridPosition) / pathLengthModDiv;
                pathLengthMod = 20 - pathLength;
            }
            else
            {
                int pathLengthModDiv = 10;
                GridPosition playerGridPosition = playerUnit.GetGridPosition();
                int pathLength = Pathfinding.Instance.GetPathLength(gridPosition, playerGridPosition) / pathLengthModDiv;
                pathLengthMod = 20 - pathLength;
            }
        }
        else
        {
            targetCountAtGridPosition = unit.GetAction<ShootAction>().GetTargetCountAtPosition(gridPosition);
            
            if (targetCountAtGridPosition != 0)
            {
                int pathLengthModDiv = 10;
                int pathLength = Pathfinding.Instance.GetPathLength(unit.GetGridPosition(), gridPosition) / pathLengthModDiv;
                pathLengthMod = 20 - pathLength;
            }
            else
            {
                int pathLengthModDiv = 10;
                GridPosition playerGridPosition = playerUnit.GetGridPosition();
                int pathLength = Pathfinding.Instance.GetPathLength(gridPosition, playerGridPosition) / pathLengthModDiv;
                pathLengthMod = 20 - pathLength;
            }
        }

        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = (targetCountAtGridPosition * 10) + pathLengthMod
        };
    }
}
