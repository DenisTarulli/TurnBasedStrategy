using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPotion : BasePotion
{
    public static event EventHandler OnSpeedPotionConsumed;
    public static void ResetStaticData()
    {
        OnSpeedPotionConsumed = null;
    }

    public override string GetName()
    {
        return "Speed Potion";
    }
    public override void ConsumePotion()
    {
        if (PotionSystem.Instance.TryConsumePotion(this))
        {
            BuffSystem.Instance.SetSpeedBuff(true);
            OnSpeedPotionConsumed?.Invoke(this, EventArgs.Empty);
            SoundManager.Instance.PlaySFX(SoundManager.SoundType.ConsumirPocion);
        }
    }
}
