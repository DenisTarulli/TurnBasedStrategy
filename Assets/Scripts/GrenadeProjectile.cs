using System;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeProjectile : MonoBehaviour
{
    public static event EventHandler OnAnyGrenadeExploded;

    public static void ResetStaticData()
    {
        OnAnyGrenadeExploded = null;
    }

    [SerializeField] private Transform grenadeExplodeVfxPrefab;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private AnimationCurve arcYAnimationCurve;
    [SerializeField] private int damage;
    [SerializeField] private int range;

    private Vector3 targetPosition;
    private Action onGrenadeBehaviourComplete;
    private float totalDistanceToTarget;
    private Vector3 positionXZ;

    private void Update()
    {
        Vector3 moveDirection = (targetPosition - positionXZ).normalized;

        float moveSpeed = 15f;
        positionXZ += moveSpeed * Time.deltaTime * moveDirection;

        float distance = Vector3.Distance(positionXZ, targetPosition);
        float distanceNormalized = 1 - distance / totalDistanceToTarget;

        float maxHeight = totalDistanceToTarget / 4f;
        float positionY = arcYAnimationCurve.Evaluate(distanceNormalized) * maxHeight;
        transform.position = new Vector3(positionXZ.x, positionY, positionXZ.z);

        float reachedTargetDistance = .2f;
        if (Vector3.Distance(positionXZ, targetPosition) < reachedTargetDistance)
        {
            GridPosition targetGridPosition = LevelGrid.Instance.GetGridPosition(targetPosition);

            List<GridPosition> gridPositionsInRange = HexRangeUtils.GetGridPositionsInRange(targetGridPosition, range);
            Debug.Log(gridPositionsInRange.Count);

            int extraDamage = 0;

            if (BuffSystem.Instance.IsPowerBuffActive())
            {
                extraDamage = 3;
                BuffSystem.Instance.SetPowerBuff(false);
            }

            foreach (GridPosition gridPosition in gridPositionsInRange)
            {
                if (!LevelGrid.Instance.IsValidGridPosition(gridPosition))
                {
                    continue;
                }

                Unit unitInGridPosition = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);
                DestructibleCrate destructibleCrate = LevelGrid.Instance.GetDestructibleCrateAtGridPosition(gridPosition);

                if (unitInGridPosition != null)
                {
                    unitInGridPosition.Damage(damage + extraDamage + PlayerStats.Instance.GetPower());
                }

                if (destructibleCrate != null)
                {
                    destructibleCrate.Damage();
                }
            }

            //float damageRadius = 2f;
            //Collider[] colliderArray = Physics.OverlapSphere(targetPosition, damageRadius);

            //foreach (Collider collider in colliderArray)
            //{
            //    if (collider.TryGetComponent<Unit>(out Unit targetUnit))
            //    {
            //        int extraDamage = 0;

            //        if (BuffSystem.Instance.IsPowerBuffActive())
            //        {
            //            extraDamage = 3;
            //            BuffSystem.Instance.SetPowerBuff(false);
            //        }

            //        targetUnit.Damage(damage + extraDamage + PlayerStats.Instance.GetPower());
            //        continue;
            //    }

            //    if (collider.TryGetComponent<DestructibleCrate>(out DestructibleCrate destructibleCrate))
            //    {
            //        destructibleCrate.Damage();
            //    }
            //}

            OnAnyGrenadeExploded?.Invoke(this, EventArgs.Empty);

            trailRenderer.transform.parent = null;

            Instantiate(grenadeExplodeVfxPrefab, targetPosition + Vector3.up * 1f, Quaternion.identity);

            Destroy(gameObject);

            onGrenadeBehaviourComplete();
        }
    }

    public void Setup(GridPosition targetGridPosition, Action onGrenadeBehaviourComplete)
    {
        this.onGrenadeBehaviourComplete = onGrenadeBehaviourComplete;
        targetPosition = LevelGrid.Instance.GetWorldPosition(targetGridPosition);

        positionXZ = transform.position;
        positionXZ.y = 0f;
        totalDistanceToTarget = Vector3.Distance(positionXZ, targetPosition);
    }
}
