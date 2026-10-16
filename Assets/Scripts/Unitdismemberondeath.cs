using System;
using UnityEngine;

/// <summary>
/// Reemplaza a UnitRagdollSpawner + UnitRagdoll.
/// Al morir, separa cada parte del cuerpo (SkinnedMeshRenderer) del propio
/// personaje que murio -conservando su color/material actual-, la convierte
/// en un objeto fisico independiente, y la deja caer por gravedad (con un
/// empujon minimo, no una explosion). No necesita ningun prefab de ragdoll aparte.
/// </summary>
public class UnitDismemberOnDeath : MonoBehaviour
{
    [SerializeField] private float scatterForceMin = 0.5f;
    [SerializeField] private float scatterForceMax = 2f;
    [SerializeField] private float scatterTorque = 2f;
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

        foreach (SkinnedMeshRenderer bodyPart in bodyParts)
        {
            DismemberPart(bodyPart);
        }

        Destroy(gameObject);
    }

    private void DismemberPart(SkinnedMeshRenderer skinnedMeshRenderer)
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

        // IMPORTANTE: al pasar de Skinned a malla normal, una escala "invisible" heredada
        // del hueso puede aparecer de golpe (esto causa piezas gigantes/diminutas).
        // La neutralizamos dejando la escala en (1,1,1) una vez ya desprendida.
        partTransform.localScale = Vector3.one;

        MeshCollider meshCollider = partObject.AddComponent<MeshCollider>();
        meshCollider.convex = true;

        Rigidbody rigidBody = partObject.AddComponent<Rigidbody>();

        Vector3 randomScatterDirection = UnityEngine.Random.onUnitSphere;
        float scatterForce = UnityEngine.Random.Range(scatterForceMin, scatterForceMax);
        rigidBody.AddForce(randomScatterDirection * scatterForce, ForceMode.Impulse);

        Vector3 randomTorque = UnityEngine.Random.insideUnitSphere * scatterTorque;
        rigidBody.AddTorque(randomTorque, ForceMode.Impulse);

        Destroy(partObject, pieceLifetime);
    }
}