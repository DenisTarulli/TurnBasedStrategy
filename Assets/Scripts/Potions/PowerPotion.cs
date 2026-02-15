using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPotion : BasePotion
{
    public override string GetName()
    {
        return "Power Potion";
    }
    public override void ConsumePotion()
    {
        if (PotionSystem.Instance.TryConsumePotion(this))
        {
            BuffSystem.Instance.SetPowerBuff(true);
            SoundManager.Instance.PlaySFX(SoundManager.SoundType.ConsumirPocion);
        }
    }
}
