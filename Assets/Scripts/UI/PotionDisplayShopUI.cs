using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PotionDisplayShopUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI amountText;
    [SerializeField] private string potionKey;

    public void UpdateAmountText()
    {
        amountText.text = $"x{InventoryManager.Instance.GetSpecificPotionAmount(potionKey)}";
    }
}
