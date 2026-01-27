using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractAction : BaseAction
{
    public event EventHandler OnInteractStarted;

    private int maxInteractDistance = 1;

    [SerializeField] private float rotationSpeed = 7200f; // grados por segundo

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
        StartCoroutine(InteractSequence(gridPosition, onActionComplete));
    }

    private IEnumerator InteractSequence(GridPosition gridPosition, Action onActionComplete)
    {
        IInteractable interactable =
            LevelGrid.Instance.GetInteractableAtGridPosition(gridPosition);

        Transform interactableTransform = ((MonoBehaviour)interactable).transform;


        // ROTAR HACIA EL OBJETO
        yield return RotateTowards(interactableTransform.position);

        // DISPARAR ANIMACIÓN
        OnInteractStarted?.Invoke(this, EventArgs.Empty);

        //  INTERACTUAR
        interactable.Interact(OnInteractComplete);

        ActionStart(onActionComplete);
    }

    private IEnumerator RotateTowards(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - unit.transform.position).normalized;
        direction.y = 0f;

        if (direction == Vector3.zero)
            yield break;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        while (Quaternion.Angle(unit.transform.rotation, targetRotation) > 1f)
        {
            unit.transform.rotation = Quaternion.RotateTowards(
                unit.transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );

            yield return null;
        }

        unit.transform.rotation = targetRotation;
    }

    private void OnInteractComplete()
    {
        ActionComplete();
    }

    public int GetMaxInteractDistance()
    {
        return maxInteractDistance;
    }
}
