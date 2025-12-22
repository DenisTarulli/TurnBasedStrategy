using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleCrate : MonoBehaviour
{
    public static event EventHandler OnAnyDestroyed;

    public static void ResetStaticData()
    {
        OnAnyDestroyed = null;
    }

    [SerializeField] private Transform crateDestroyedPrefab;

    private GridPosition gridPosition;

    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        GridObject gridObject = LevelGrid.Instance.GetGridObjectAtGridPosition(gridPosition);
        gridObject.SetDestructibleCrate(this);
    }

    public void Damage()
    {
        Transform crateDestroyedTransform = Instantiate(crateDestroyedPrefab, transform.position, transform.rotation);

        float explosionForce = UnityEngine.Random.Range(50f, 200f);
        float explosionRange = 10f;
        ApplyExplosionToChildren(crateDestroyedTransform, explosionForce, transform.position, explosionRange);

        Destroy(gameObject);

        OnAnyDestroyed?.Invoke(this, EventArgs.Empty);
    }

    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }

    private void ApplyExplosionToChildren(Transform root, float explosionForce, Vector3 explosionPosition, float explosionRange)
    {
        foreach (Transform child in root)
        {
            if (child.TryGetComponent<Rigidbody>(out Rigidbody childRigidBody))
            {
                childRigidBody.AddExplosionForce(explosionForce, explosionPosition, explosionRange);

                ApplyExplosionToChildren(child, explosionForce, explosionPosition, explosionRange);
            }
        }
    }
}
