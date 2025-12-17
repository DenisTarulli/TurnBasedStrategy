using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPotion : BasePotion
{
    public override string GetName()
    {
        return "Speed Potion";
    }
    public override void ConsumePotion()
    {
        base.ConsumePotion();
    }
}
