using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractAction : BaseAction
{
    public event EventHandler OnInteractStarted;

    private int maxInteractDistance = 1;

    private void Update()
    {
        if (!isActive)
        {
            return;
        }
    }

    public override string GetActionName()
    {
        return "Interact";
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
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = unit.GetGridPosition();

        for (int x = -maxInteractDistance; x <= maxInteractDistance; x++)
        {
            for (int z = -maxInteractDistance; z <= maxInteractDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (unitGridPosition.z % 2 != 0)
                {
                    // Unit is on odd row
                    if (x == -maxInteractDistance && z != 0)
                    {
                        // Hex is out of action range
                        continue;
                    }
                }
                else
                {
                    // Unit is on even row
                    if (x == maxInteractDistance && z != 0)
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

                IInteractable interactable = LevelGrid.Instance.GetInteractableAtGridPosition(testGridPosition);

                if (interactable == null)
                {
                    // No interactable on this GridPosition
                    continue;
                }

                if (interactable.GetType() == typeof(Chest))
                {
                    // Chest on testGridPosition
                    if (!InventoryManager.Instance.HasKeys())
                    {
                        // Player has no keys to open the chest
                        continue;
                    }
                }

                if (interactable.GetType() == typeof(KeyPedestal))
                {
                    // KeyPedestal on testGridPosition
                    KeyPedestal keyPedestal = (KeyPedestal)interactable;

                    if (!keyPedestal.HasKeyToCollect())
                    {
                        // Key has already been looted
                        continue;
                    }
                }

                if (interactable.GetType() == typeof(Door))
                {
                    // Door on testGridPosition
                    Door door = (Door)interactable;

                    if (door.IsOpen())
                    {
                        // Door has already been opened
                        continue;
                    }
                }

                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        IInteractable interactable = LevelGrid.Instance.GetInteractableAtGridPosition(gridPosition);

        OnInteractStarted?.Invoke(this, EventArgs.Empty);

        interactable.Interact(OnInteractComplete);

        ActionStart(onActionComplete);
    }

    public int GetMaxInteractDistance()
    {
        return maxInteractDistance;
    }

    private void OnInteractComplete()
    {
        ActionComplete();
    }
}
