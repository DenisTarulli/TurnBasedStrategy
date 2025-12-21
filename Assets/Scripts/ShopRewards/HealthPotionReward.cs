using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotionReward : BaseReward
{
    public override void Behaviour()
    {
        InventoryManager.Instance.AddPotion("Health Potion");
    }
}
