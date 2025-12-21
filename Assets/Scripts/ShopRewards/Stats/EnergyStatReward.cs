using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyStatReward : BaseReward
{
    public override void Behaviour()
    {
        PlayerStats.Instance.ChangeEnergy(1);
    }
}
