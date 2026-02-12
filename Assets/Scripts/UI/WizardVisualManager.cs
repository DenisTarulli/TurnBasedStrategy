using System.Collections.Generic;
using UnityEngine;

public class WizardVisualManager : MonoBehaviour
{
    [SerializeField] private List<BodyPartColor> bodyParts;

    private Dictionary<BodyPartColor.BodyPartType, BodyPartColor> bodyPartDictionary;

    private int paintedPartsIndex = 0;

    private readonly BodyPartColor.BodyPartType[] paintOrder =
    {
        BodyPartColor.BodyPartType.Boots,
        BodyPartColor.BodyPartType.Head,
        BodyPartColor.BodyPartType.RightArm,
        BodyPartColor.BodyPartType.LeftArm,
        BodyPartColor.BodyPartType.Shoulders,
        BodyPartColor.BodyPartType.Tunic
    };

    private void Awake()
    {
        bodyPartDictionary = new Dictionary<BodyPartColor.BodyPartType, BodyPartColor>();

        foreach (var part in bodyParts)
        {
            bodyPartDictionary[part.GetBodyPartType()] = part;
        }
    }

    private void Start()
    {
        PlayerStats.Instance.OnSpeedChanged += (_, __) => PaintNextPart(Color.green);
        PlayerStats.Instance.OnHealthChanged += (_, __) => PaintNextPart(Color.red);
        PlayerStats.Instance.OnEnergyChanged += (_, __) => PaintNextPart(Color.yellow);
        PlayerStats.Instance.OnResistanceChanged += (_, __) => PaintNextPart(Color.blue);
        PlayerStats.Instance.OnPowerChanged += (_, __) => PaintNextPart(new Color(0.6f, 0f, 0.8f));
    }

    private void PaintNextPart(Color color)
    {
        if (paintedPartsIndex >= paintOrder.Length)
        {
            return;
        }

        var partToPaint = paintOrder[paintedPartsIndex];

        if (bodyPartDictionary.TryGetValue(partToPaint, out var bodyPart))
        {
            bodyPart.SetColor(color);
            paintedPartsIndex++;
        }
    }
}
