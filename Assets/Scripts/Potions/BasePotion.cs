using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class BasePotion : MonoBehaviour
{
    [SerializeField] protected Sprite potionSprite;
    protected TextMeshProUGUI potionAmountText;
    protected string potionName;

    public abstract string GetName();
    public abstract void ConsumePotion();

    public void SelectPotion()
    {
        PotionSystem.Instance.SetSelectedPotion(this);
    }

    public Sprite GetSprite()
    {
        return potionSprite;
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
