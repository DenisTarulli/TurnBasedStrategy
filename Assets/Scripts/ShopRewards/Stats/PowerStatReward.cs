using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerStatReward : BaseReward
{
    public override void Behaviour()
    {
        PlayerStats.Instance.ChangePower(1);
    }
}
