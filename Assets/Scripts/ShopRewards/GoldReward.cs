using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldReward : BaseReward
{
    public override void Behaviour()
    {
        int goldToAdd = ShopSystem.Instance.GetGoldReward();
        InventoryManager.Instance.ChangeGoldAmount(goldToAdd);
    }
}
