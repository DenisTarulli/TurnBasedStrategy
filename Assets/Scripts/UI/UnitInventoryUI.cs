using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UnitInventoryUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI expText;
    [SerializeField] private TextMeshProUGUI keysText;

    private void Start()
    {
        InventoryManager.Instance.OnGoldAmountChanged += InventoryManager_OnGoldAmountChanged;
        InventoryManager.Instance.OnKeysAmountChanged += InventoryManager_OnKeysAmountChanged;
        PlayerStats.Instance.OnExpChanged += PlayerStats_OnExpChanged;

        int currentExp = PlayerStats.Instance.GetExp();
        int requiredExp = PlayerStats.Instance.GetExpToLevelUp();

        UpdateExpText(currentExp, requiredExp);
    }

    private void PlayerStats_OnExpChanged(object sender, System.EventArgs e)
    {
        int currentExp = PlayerStats.Instance.GetExp();
        int requiredExp = PlayerStats.Instance.GetExpToLevelUp();

        UpdateExpText(currentExp, requiredExp);
    }

    private void InventoryManager_OnKeysAmountChanged(object sender, InventoryManager.OnKeysAmountChangedEventArgs e)
    {
        keysText.text = $"Keys: {e.keys}";
    }

    private void InventoryManager_OnGoldAmountChanged(object sender, InventoryManager.OnGoldAmountChangedEventArgs e)
    {
        goldText.text = $"Gold: {e.gold}";
    }

    private void UpdateExpText(int currentExp, int requiredExp)
    {
        expText.text = $"Exp: {currentExp}/{requiredExp}";
    }
}
