using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResistancePotionReward : BaseReward
{
    public override void Behaviour()
    {
        InventoryManager.Instance.AddPotion("Resistance Potion");
        InventoryManager.Instance.ChangeGoldAmount(-10);
    }
}
