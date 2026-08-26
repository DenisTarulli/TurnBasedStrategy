using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResistanceStatReward : BaseReward
{
    public override void Behaviour()
    {
        PlayerStats.Instance.ChangeResistance(1);
        PlayerCosmeticColorSystem.Instance.PaintResistanceColor();
    }
}
