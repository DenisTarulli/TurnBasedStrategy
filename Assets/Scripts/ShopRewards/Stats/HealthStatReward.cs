using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthStatReward : BaseReward
{
    public override void Behaviour()
    {
        PlayerStats.Instance.ChangeHealth(1);
        PlayerCosmeticColorSystem.Instance.PaintHealthColor();
    }
}
