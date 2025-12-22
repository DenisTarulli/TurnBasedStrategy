using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResistancePotion : BasePotion
{
    public override string GetName()
    {
        return "Resistance Potion";
    }
    public override void ConsumePotion()
    {
        if (PotionSystem.Instance.TryConsumePotion(this))
        {
            BuffSystem.Instance.SetResistanceBuff(true);
        }
    }
}
