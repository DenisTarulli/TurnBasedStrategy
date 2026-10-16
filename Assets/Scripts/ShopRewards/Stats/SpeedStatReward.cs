using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedStatReward : BaseReward
{
    public override void Behaviour()
    {
        PlayerStats.Instance.ChangeSpeed(1);
        PlayerCosmeticColorSystem.Instance.PaintSpeedColor();
    }
}
