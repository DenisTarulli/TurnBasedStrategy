using System.Collections.Generic;
using UnityEngine;

public class EnemyBodyColorController : MonoBehaviour
{
    // Tipos de partes del cuerpo (LOCAL al script)
    public enum BodyPartType
    {
        Arm,
        Body,
        Hood,
        Legs
    }

    [System.Serializable]
    public class EnemyBodyPart
    {
        public BodyPartType bodyPart;
        public SkinnedMeshRenderer renderer;
        public Color color;
    }

    [SerializeField] private List<EnemyBodyPart> bodyParts = new List<EnemyBodyPart>();

    private void Start()
    {
        ApplyColors();
    }

    private void ApplyColors()
    {
        foreach (EnemyBodyPart part in bodyParts)
        {
            if (part.renderer == null)
            {
                Debug.LogWarning($"Falta SkinnedMeshRenderer en {part.bodyPart}", this);
                continue;
            }

            // Crear instancia de material SOLO para este renderer
            Material materialInstance = part.renderer.material;

            // Tu shader usa _BaseColor
            materialInstance.SetColor("_BaseColor", part.color);
        }
    }
}
