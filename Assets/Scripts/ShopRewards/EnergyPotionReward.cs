using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyPotionReward : BaseReward
{
    public override void Behaviour()
    {
        InventoryManager.Instance.AddPotion("Energy Potion");
    }
}
