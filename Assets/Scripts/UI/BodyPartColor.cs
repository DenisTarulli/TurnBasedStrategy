using UnityEngine;

public class BodyPartColor : MonoBehaviour
{
    public enum BodyPartType
    {
        Head,
        Boots,
        LeftArm,
        RightArm,
        Shoulders,
        Tunic
    }

    [SerializeField] private BodyPartType bodyPartType;

    private SkinnedMeshRenderer skinnedMeshRenderer;
    private MaterialPropertyBlock mpb;

    private void Awake()
    {
        skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        mpb = new MaterialPropertyBlock();
    }

    public BodyPartType GetBodyPartType()
    {
        return bodyPartType;
    }

    public void SetColor(Color color)
    {
        skinnedMeshRenderer.GetPropertyBlock(mpb);
        mpb.SetColor("_BaseColor", color);
        skinnedMeshRenderer.SetPropertyBlock(mpb);
    }
    public Color GetCurrentColor()
    {
        MaterialPropertyBlock block = new MaterialPropertyBlock();
        skinnedMeshRenderer.GetPropertyBlock(block);

        if (block != null && block.HasColor("_BaseColor"))
        {
            return block.GetColor("_BaseColor");
        }

        return Color.white;
    }

}
