using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractAction : BaseAction
{
    public event EventHandler OnInteractStarted;

    private int maxInteractDistance = 1;

    [SerializeField] private float rotateSpeed = 100f; // grados por segundo

    private IInteractable currentInteractable;

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
                    if (x == -maxInteractDistance && z != 0) continue;
                }
                else
                {
                    if (x == maxInteractDistance && z != 0) continue;
                }

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) continue;

                IInteractable interactable =
                    LevelGrid.Instance.GetInteractableAtGridPosition(testGridPosition);

                if (interactable == null) continue;

                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    // ACÁ EMPIEZA LO IMPORTANTE
    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        currentInteractable = LevelGrid.Instance.GetInteractableAtGridPosition(gridPosition);

        StartCoroutine(InteractSequence(gridPosition, onActionComplete));
    }

    private IEnumerator InteractSequence(GridPosition gridPosition, Action onActionComplete)
    {
        IInteractable interactable =
            LevelGrid.Instance.GetInteractableAtGridPosition(gridPosition);

        Transform interactableTransform = ((MonoBehaviour)interactable).transform;

        Vector3 direction = (interactableTransform.position - unit.transform.position).normalized;
        direction.y = 0f;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        while (Quaternion.Angle(unit.transform.rotation, targetRotation) > 0.1f)
        {
            unit.transform.rotation = Quaternion.RotateTowards(
                unit.transform.rotation,
                targetRotation,
                Time.deltaTime * rotateSpeed 
            );
            yield return null;
        }


        ActionStart(onActionComplete);

        // DISPARAR ANIMACIÓN
        OnInteractStarted?.Invoke(this, EventArgs.Empty);

        // INTERACTUAR
        interactable.Interact(OnInteractComplete);
    }


    private void OnInteractComplete()
    {
        ActionComplete();
    }

    public int GetMaxInteractDistance()
    {
        return maxInteractDistance;
    }

    public IInteractable GetCurrentInteractable()
    {
        return currentInteractable;
    }
}
