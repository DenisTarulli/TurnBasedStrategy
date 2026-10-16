using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitInventoryUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI expText;
    [SerializeField] private TextMeshProUGUI keysText;
    [SerializeField] private Image expBar;

    private void Start()
    {
        InventoryManager.Instance.OnGoldAmountChanged += InventoryManager_OnGoldAmountChanged;
        InventoryManager.Instance.OnKeysAmountChanged += InventoryManager_OnKeysAmountChanged;
        PlayerStats.Instance.OnExpChanged += PlayerStats_OnExpChanged;

        int currentExp = PlayerStats.Instance.GetExp();
        int requiredExp = PlayerStats.Instance.GetExpToLevelUp();

        UpdateExpText(currentExp, requiredExp);
        UpdateExpBar();
        UpdateInventoryAmounts();
    }

    private void PlayerStats_OnExpChanged(object sender, System.EventArgs e)
    {
        int currentExp = PlayerStats.Instance.GetExp();
        int requiredExp = PlayerStats.Instance.GetExpToLevelUp();

        UpdateExpText(currentExp, requiredExp);
        UpdateExpBar();
    }

    private void InventoryManager_OnKeysAmountChanged(object sender, InventoryManager.OnKeysAmountChangedEventArgs e)
    {
        keysText.text = $"{e.keys}";
    }

    private void InventoryManager_OnGoldAmountChanged(object sender, InventoryManager.OnGoldAmountChangedEventArgs e)
    {
        goldText.text = $"{e.gold}";
    }

    private void UpdateExpText(int currentExp, int requiredExp)
    {
        expText.text = $"{currentExp}/{requiredExp}";
    }

    private void UpdateExpBar()
    {
        expBar.fillAmount = PlayerStats.Instance.GetExpNormalized();
    }

    private void UpdateInventoryAmounts()
    {
        goldText.text = $"{InventoryManager.Instance.GetGoldAmount()}";
        keysText.text = $"{InventoryManager.Instance.GetKeysAmount()}";
        expText.text = $"{PlayerStats.Instance.GetExp()/PlayerStats.Instance.GetExpToLevelUp()}";
    }
}
