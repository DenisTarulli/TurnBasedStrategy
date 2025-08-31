using System.Collections.Generic;
using UnityEngine;

public class MoveAction : MonoBehaviour
{
    private const string UNIT_ISWALKING = "IsWalking";

    [SerializeField] private Animator unitAnimator;
    [SerializeField] private int maxMoveDistance = 4;

    private Vector3 targetPosition;
    private Unit unit;

    private void Awake()
    {
        unit = GetComponent<Unit>();
        targetPosition = transform.position;
    }

    private void Update()
    {
        float stoppingDistance = 0.1f;
        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

        if (distanceToTarget > stoppingDistance)
        {
            Vector3 moveDirection = (targetPosition - transform.position).normalized;
            float moveSpeed = 4f;

            float rotateSpeed = 10f;
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);

            transform.position += moveSpeed * Time.deltaTime * moveDirection;

            unitAnimator.SetBool(UNIT_ISWALKING, true);
        }
        else
        {
            unitAnimator.SetBool(UNIT_ISWALKING, false);
        }
    }

    public void Move(GridPosition gridPosition)
    {
        targetPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
    }

    /// <summary>
    /// Checks if the given <see href="gridPosition"/> is in the <see href="validGridPositionList"/>
    /// </summary>
    /// <param name="gridPosition">Grid position to check</param>
    /// <returns>True if the <see href="gridPosition"/> is valid</returns>
    public bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        List<GridPosition> validGridPositionList = GetValidActionGridPositionList();
        return validGridPositionList.Contains(gridPosition);
    }

    /// <summary>
    /// Cycles through all the of the potential grid positions within the maximum move distance
    /// and returns a list of all the grid positions that meet the requirements
    /// </summary>
    /// <returns><see cref="List{GridPosition}"/> of type <see cref="GridPosition"/></returns>
    public List<GridPosition> GetValidActionGridPositionList()
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

                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }
}
