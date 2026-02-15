using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyPotion : BasePotion
{
    public override string GetName()
    {
        return "Energy Potion";
    }
    public override void ConsumePotion()
    {
        if (PotionSystem.Instance.TryConsumePotion(this))
        {
            BuffSystem.Instance.SetEnergyBuff(true);
            SoundManager.Instance.PlaySFX(SoundManager.SoundType.ConsumirPocion);
        }
    }
}
