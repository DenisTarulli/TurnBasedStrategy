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
            float damageRadius = 2f;
            Collider[] colliderArray = Physics.OverlapSphere(targetPosition, damageRadius);

            foreach (Collider collider in colliderArray)
            {
                if (collider.TryGetComponent<Unit>(out Unit targetUnit))
                {
                    targetUnit.Damage(30);
                    continue;
                }

                if (collider.TryGetComponent<DestructibleCrate>(out DestructibleCrate destructibleCrate))
                {
                    destructibleCrate.Damage();
                }
            }

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
