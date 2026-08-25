using System;
using UnityEngine;

/// <summary>
/// Reemplaza a UnitRagdollSpawner + UnitRagdoll.
/// Al morir, separa cada parte del cuerpo (SkinnedMeshRenderer) del propio
/// personaje que murio -conservando su color/material actual-, la convierte
/// en un objeto fisico independiente, y le aplica una fuerza de explosion.
/// No necesita ningun prefab de ragdoll aparte.
/// </summary>
public class UnitDismemberOnDeath : MonoBehaviour
{
    [SerializeField] private float explosionForceMin = 400f;
    [SerializeField] private float explosionForceMax = 600f;
    [SerializeField] private float explosionRadius = 10f;
    [SerializeField] private float pieceLifetime = 6f;

    private HealthSystem healthSystem;

    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
        healthSystem.OnDead += HealthSystem_OnDead;
    }

    private void HealthSystem_OnDead(object sender, EventArgs e)
    {
        SkinnedMeshRenderer[] bodyParts = GetComponentsInChildren<SkinnedMeshRenderer>();

        Vector3 randomOffset = new Vector3(UnityEngine.Random.Range(-1f, 1f), 0f, UnityEngine.Random.Range(-1f, 1f));
        Vector3 explosionPosition = transform.position + randomOffset;
        float explosionForce = UnityEngine.Random.Range(explosionForceMin, explosionForceMax);

        foreach (SkinnedMeshRenderer bodyPart in bodyParts)
        {
            DismemberPart(bodyPart, explosionPosition, explosionForce);
        }

        Destroy(gameObject);
    }

    private void DismemberPart(SkinnedMeshRenderer skinnedMeshRenderer, Vector3 explosionPosition, float explosionForce)
    {
        Transform partTransform = skinnedMeshRenderer.transform;
        GameObject partObject = partTransform.gameObject;

        // Congela la pose animada actual en una malla estatica (mismo color/material que ya tenia)
        Mesh bakedMesh = new Mesh();
        skinnedMeshRenderer.BakeMesh(bakedMesh);
        Material[] originalMaterials = skinnedMeshRenderer.sharedMaterials;

        Destroy(skinnedMeshRenderer);

        MeshFilter meshFilter = partObject.AddComponent<MeshFilter>();
        meshFilter.mesh = bakedMesh;

        MeshRenderer meshRenderer = partObject.AddComponent<MeshRenderer>();
        meshRenderer.sharedMaterials = originalMaterials;

        // Se independiza del esqueleto/personaje, manteniendo su posicion actual en el mundo
        partTransform.SetParent(null, true);

        MeshCollider meshCollider = partObject.AddComponent<MeshCollider>();
        meshCollider.convex = true;

        Rigidbody rigidBody = partObject.AddComponent<Rigidbody>();
        rigidBody.AddExplosionForce(explosionForce, explosionPosition, explosionRadius);

        Destroy(partObject, pieceLifetime);
    }
}