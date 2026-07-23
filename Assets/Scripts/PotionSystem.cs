using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionSystem : MonoBehaviour
{
    public static PotionSystem Instance { get; private set; }

    public event EventHandler OnSelectedPotionChange;

    private BasePotion selectedPotion;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one PotionSystem! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public BasePotion[] GetBasePotionArray()
    {
        BasePotion[] basePotionArray = GetComponents<BasePotion>();

        return basePotionArray;
    }

    public bool TryConsumePotion(BasePotion basePotion)
    {
        string potionName = basePotion.GetName();

        if (!InventoryManager.Instance.HasKeyPotion(potionName))
        {
            Debug.Log($"No {potionName} available");
            return false;
        }

        InventoryManager.Instance.RemovePotion(basePotion.GetName());
        return true;
    }

    public void SetSelectedPotion(BasePotion newSelectedPotion)
    {
        selectedPotion = newSelectedPotion;
        OnSelectedPotionChange?.Invoke(this, EventArgs.Empty);
    }

    public void Consume()
    {
        selectedPotion.ConsumePotion();
    }

    public BasePotion GetSelectedPotion()
    {
        return selectedPotion;
    }
}
