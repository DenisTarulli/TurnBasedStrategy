using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPotionReward : BaseReward
{
    public override void Behaviour()
    {
        InventoryManager.Instance.AddPotion("Speed Potion");
    }
}
