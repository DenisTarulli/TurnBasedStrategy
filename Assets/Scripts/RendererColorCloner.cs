using UnityEngine;
using System.Collections.Generic;

public static class RendererColorCloner
{
    private static readonly int BaseColorID = Shader.PropertyToID("_BaseColor");

    public static void CloneColors(GameObject source, GameObject target)
    {
        Renderer[] sourceRenderers = source.GetComponentsInChildren<Renderer>(true);
        Renderer[] targetRenderers = target.GetComponentsInChildren<Renderer>(true);

        Dictionary<string, Renderer> sourceMap = new Dictionary<string, Renderer>();

        foreach (Renderer r in sourceRenderers)
        {
            if (!sourceMap.ContainsKey(r.gameObject.name))
            {
                sourceMap.Add(r.gameObject.name, r);
            }
        }

        foreach (Renderer targetRenderer in targetRenderers)
        {
            if (!sourceMap.TryGetValue(targetRenderer.gameObject.name, out Renderer sourceRenderer))
                continue;

            MaterialPropertyBlock mpb = new MaterialPropertyBlock();
            sourceRenderer.GetPropertyBlock(mpb);

            bool copiedFromMPB = false;

            // Intentar copiar desde MPB
            try
            {
                Color mpbColor = mpb.GetColor(BaseColorID);
                if (mpbColor != default)
                {
                    targetRenderer.SetPropertyBlock(mpb);
                    copiedFromMPB = true;
                }
            }
            catch { }

            //Fallback: copiar desde material
            if (!copiedFromMPB && sourceRenderer.sharedMaterial != null &&
                sourceRenderer.sharedMaterial.HasProperty(BaseColorID))
            {
                Color color = sourceRenderer.sharedMaterial.GetColor(BaseColorID);
                Material matInstance = targetRenderer.material;
                matInstance.SetColor(BaseColorID, color);
            }
        }
    }
}
