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

    public void SetPotionAmountTextObject(TextMeshProUGUI potionAmountText)
    {
        this.potionAmountText = potionAmountText;
    }

    public TextMeshProUGUI GetPotionAmountTextObject()
    {
        return potionAmountText;
    }
}
