using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeAction : BaseAction
{
    [SerializeField] private Transform grenadeProjectilePrefab;
    [SerializeField] private int maxThrowDistance = 7;
    [SerializeField] private LayerMask obstaclesLayerMask;
    [SerializeField] private float rotateSpeed = 100f;

    private float timer;
    private bool grenadeThrown;
    private GridPosition targetGridPosition;

    public event EventHandler OnThrowStarted;

    private void Update()
    {
        if (!isActive)
        {
            return;
        }

        timer -= Time.deltaTime;

        if (timer <= 0f && !grenadeThrown)
        {
            grenadeThrown = true;

            Transform grenadeProjectileTransform =
                Instantiate(grenadeProjectilePrefab, unit.GetWorldPosition(), Quaternion.identity);

            GrenadeProjectile grenadeProjectile =
                grenadeProjectileTransform.GetComponent<GrenadeProjectile>();

            grenadeProjectile.Setup(targetGridPosition, OnGrenadeBehaviourComplete);
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
        targetGridPosition = gridPosition;
        grenadeThrown = false;
        timer = 1.8f;

        StartCoroutine(ThrowSequence(onActionComplete));
    }

    private IEnumerator ThrowSequence(Action onActionComplete)
    {

        Vector3 targetWorldPosition = LevelGrid.Instance.GetWorldPosition(targetGridPosition);

        Vector3 direction = (targetWorldPosition - unit.transform.position).normalized;
        direction.y = 0f;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // ROTAR HACIA EL OBJETIVO
        while (Quaternion.Angle(unit.transform.rotation, targetRotation) > 0.1f)
        {
            unit.transform.rotation = Quaternion.RotateTowards(
                unit.transform.rotation,
                targetRotation,
                rotateSpeed * Time.deltaTime
            );
            yield return null;
        }

        // ARRANCA LA ACCIÓN
        OnThrowStarted?.Invoke(this, EventArgs.Empty);
        ActionStart(onActionComplete);
    }



    private void OnGrenadeBehaviourComplete()
    {
        ActionComplete();
    }
}
