using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : BasePotion
{
    public override string GetName()
    {
        return "Health Potion";
    }
    public override void ConsumePotion()
    {
        if (PotionSystem.Instance.TryConsumePotion(this))
        {
            BuffSystem.Instance.SetHealthBuff(true);
            SoundManager.Instance.PlaySFX(SoundManager.SoundType.ConsumirPocion);
        }
    }
}
