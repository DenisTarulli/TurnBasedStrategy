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
    public abstract void ConsumePotion();
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
