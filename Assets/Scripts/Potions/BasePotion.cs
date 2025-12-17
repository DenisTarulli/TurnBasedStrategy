using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class BasePotion : MonoBehaviour
{
    //[SerializeField] protected Sprite spriteVisual;
    [SerializeField] protected Color potionColorUI;
    protected TextMeshProUGUI potionAmountText;
    protected string potionName;

    private void Start()
    {
        InventoryManager.Instance.OnAnyPotionAmountChanged += InventoryManager_OnAnyPotionAmountChanged;
    }

    private void InventoryManager_OnAnyPotionAmountChanged(object sender, InventoryManager.OnAnyPotionAmountChangedEventArgs e)
    {
        if (e.name != GetName())
        {
            return;
        }

        UpdatePotionAmountText(e.amount);
    }

    public abstract string GetName();
    public virtual void ConsumePotion()
    {
        if (PotionSystem.Instance.TryConsumePotion(this))
        {
            Debug.Log($"Used {GetName()}");
        }

    }
    public Color GetColor()
    {
        return potionColorUI;
    }
    protected void UpdatePotionAmountText(int newAmount)
    {
        potionAmountText.text = newAmount.ToString();
    }

    public void SetPotionAmountTextObject(TextMeshProUGUI potionAmountText)
    {
        this.potionAmountText = potionAmountText;
    }
}
