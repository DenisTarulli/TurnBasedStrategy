using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{
    protected Unit unit;
    protected bool isActive;
    protected Action onActionComplete;

    protected virtual void Awake()
    {
        unit = GetComponent<Unit>();
    }

    public abstract string GetActionName();

    public abstract void TakeAction(GridPosition gridPosition, Action onActionComplete);

    /// <summary>
    /// Checks if the given <see href="gridPosition"/> is in the <see href="validGridPositionList"/>
    /// </summary>
    /// <param name="gridPosition">Grid position to check</param>
    /// <returns>True if the <see href="gridPosition"/> is valid</returns>
    public virtual bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        List<GridPosition> validGridPositionList = GetValidActionGridPositionList();
        return validGridPositionList.Contains(gridPosition);
    }

    /// <summary>
    /// Cycles through all the of the potential grid positions within the maximum move distance
    /// of the unit and returns a list of all the grid positions that meet the requirements
    /// </summary>
    /// <returns><see cref="List{GridPosition}"/> of type <see cref="GridPosition"/></returns>
    public abstract List<GridPosition> GetValidActionGridPositionList();
}
