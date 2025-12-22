using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPotionReward : BaseReward
{
    public override void Behaviour()
    {
        InventoryManager.Instance.AddPotion("Power Potion");
        InventoryManager.Instance.ChangeGoldAmount(-10);
    }
}
