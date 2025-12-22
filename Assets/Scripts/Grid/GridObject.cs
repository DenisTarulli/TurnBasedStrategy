using System.Collections.Generic;
using System.Numerics;
using System.Transactions;
using UnityEngine;

public class GridObject
{
    private GridSystemHex<GridObject> gridSystem;
    private GridPosition gridPosition;
    private List<Unit> unitList;
    private IInteractable interactable;
    private DestructibleCrate destructibleCrate;

    public GridObject(GridSystemHex<GridObject> gridSystem, GridPosition gridPosition)
    {
        this.gridSystem = gridSystem;
        this.gridPosition = gridPosition;
        unitList = new List<Unit>();
    }

    public override string ToString()
    {
        string unitString = "";

        foreach (Unit unit in unitList)
        {
            unitString += unit + "\n";
        }

        return gridPosition.ToString() + "\n" + unitString;
    }

    public void AddUnit(Unit unit)
    {
        unitList.Add(unit);
    }

    public void RemoveUnit(Unit unit)
    {
        unitList.Remove(unit);
    }

    public List<Unit> GetUnitList()
    {
        return unitList;
    }

    public bool HasAnyUnit()
    {
        return unitList.Count > 0;
    }

    public Unit GetUnit()
    {
        if (HasAnyUnit())
        {
            return unitList[0];
        }
        else
        {
            return null;
        }
    }

    public IInteractable GetInteractable()
    {
        return interactable;
    }

    public void SetInteractable(IInteractable interactable)
    {
        this.interactable = interactable;
    }

    public void ClearInteractable()
    {
        this.interactable = null;
    }

    public void SetDestructibleCrate(DestructibleCrate crate)
    {
        this.destructibleCrate = crate;
    }

    public DestructibleCrate GetDestructibleCrate()
    {
        return destructibleCrate;
    }
}
