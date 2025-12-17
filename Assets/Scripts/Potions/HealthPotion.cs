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
        base.ConsumePotion();
    }
}
