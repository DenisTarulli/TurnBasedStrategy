using System;
using UnityEngine;

public class UnitExplosionDeath : MonoBehaviour
{
    [Header("Explosion Visual")]
    [SerializeField] private Transform explodedPrefab;

    [Tooltip("Hips / RootBone / Mesh visual")]
    [SerializeField] private Transform visualRoot;

    [Header("Explosion Physics")]
    [SerializeField] private float minExplosionForce = 250f;
    [SerializeField] private float maxExplosionForce = 450f;
    [SerializeField] private float explosionRadius = 5f;
    [SerializeField] private float upwardModifier = 0.3f;

    [Header("Cleanup")]
    [SerializeField] private float destroyExplodedAfter = 5f;

    private HealthSystem healthSystem;

    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();

        if (healthSystem != null)
        {
            healthSystem.OnDead += HealthSystem_OnDead;
        }
        else
        {
            Debug.LogError($"{name} no tiene HealthSystem", this);
        }
    }

    private void HealthSystem_OnDead(object sender, EventArgs e)
    {
        if (explodedPrefab == null || visualRoot == null)
        {
            Debug.LogError("Faltan referencias en UnitExplosionDeath", this);
            return;
        }

        // Instanciar en la posición REAL del cuerpo
        Transform explodedTransform = Instantiate(
            explodedPrefab,
            visualRoot.position,
            visualRoot.rotation
        );

        // Copiar pose del personaje vivo
        CopyPose(visualRoot, explodedTransform);

        // Explosión física
        float explosionForce = UnityEngine.Random.Range(
            minExplosionForce,
            maxExplosionForce
        );

        ApplyExplosionToChildren(
            explodedTransform,
            explosionForce,
            visualRoot.position,
            explosionRadius
        );

        Destroy(explodedTransform.gameObject, destroyExplodedAfter);
        Destroy(gameObject);
    }

    private void CopyPose(Transform source, Transform target)
    {
        foreach (Transform sourceChild in source)
        {
            Transform targetChild = target.Find(sourceChild.name);
            if (targetChild == null) continue;

            targetChild.SetPositionAndRotation(
                sourceChild.position,
                sourceChild.rotation
            );

            CopyPose(sourceChild, targetChild);
        }
    }

    private void ApplyExplosionToChildren(
        Transform root,
        float explosionForce,
        Vector3 explosionPosition,
        float explosionRange)
    {
        foreach (Transform child in root)
        {
            if (child.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                rb.isKinematic = false;
                rb.WakeUp();

                rb.AddExplosionForce(
                    explosionForce,
                    explosionPosition,
                    explosionRange,
                    explosionForce * upwardModifier,
                    ForceMode.Impulse
                );
            }

            ApplyExplosionToChildren(
                child,
                explosionForce,
                explosionPosition,
                explosionRange
            );
        }
    }
}
