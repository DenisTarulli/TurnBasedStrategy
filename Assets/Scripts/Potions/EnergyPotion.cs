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
        base.ConsumePotion();
    }
}
