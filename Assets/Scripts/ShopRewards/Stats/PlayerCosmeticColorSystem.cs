using System.Collections.Generic;
using UnityEngine;

/// Pinta una parte AL AZAR, entre las que todavia no fueron pintadas, del cuerpo
/// del mago, con el color correspondiente a la stat elegida en el Shop
/// Cada parte solo puede pintarse UNA vez: una vez pintada, no vuelve a sortearse.
public class PlayerCosmeticColorSystem : MonoBehaviour
{
    public static PlayerCosmeticColorSystem Instance { get; private set; }

    [SerializeField] private Material healthMaterial;
    [SerializeField] private Material energyMaterial;
    [SerializeField] private Material powerMaterial;
    [SerializeField] private Material resistanceMaterial;
    [SerializeField] private Material speedMaterial;

    private List<SkinnedMeshRenderer> unpaintedParts;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Hay mas de un PlayerCosmeticColorSystem! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }

        Instance = this;

        unpaintedParts = new List<SkinnedMeshRenderer>(GetComponentsInChildren<SkinnedMeshRenderer>());
    }

    public void PaintHealthColor()
    {
        PaintRandomUnpaintedPart(healthMaterial);
    }

    public void PaintEnergyColor()
    {
        PaintRandomUnpaintedPart(energyMaterial);
    }

    public void PaintPowerColor()
    {
        PaintRandomUnpaintedPart(powerMaterial);
    }

    public void PaintResistanceColor()
    {
        PaintRandomUnpaintedPart(resistanceMaterial);
    }

    public void PaintSpeedColor()
    {
        PaintRandomUnpaintedPart(speedMaterial);
    }

    private void PaintRandomUnpaintedPart(Material colorMaterial)
    {
        if (colorMaterial == null)
        {
            Debug.LogWarning("PlayerCosmeticColorSystem: falta asignar el material de color correspondiente en el Inspector.");
            return;
        }

        if (unpaintedParts == null || unpaintedParts.Count == 0)
        {
            Debug.LogWarning("PlayerCosmeticColorSystem: ya no quedan partes sin pintar.");
            return;
        }

        int randomIndex = Random.Range(0, unpaintedParts.Count);
        SkinnedMeshRenderer targetRenderer = unpaintedParts[randomIndex];

        targetRenderer.sharedMaterial = colorMaterial;

        // Ya fue pintada: sale de la lista para siempre, no puede volver a sortearse
        unpaintedParts.RemoveAt(randomIndex);
    }
}