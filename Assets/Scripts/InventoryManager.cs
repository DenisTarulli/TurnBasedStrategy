using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public event EventHandler<OnAnyPotionAmountChangedEventArgs> OnAnyPotionAmountChanged;
    public class OnAnyPotionAmountChangedEventArgs : EventArgs
    {
        public int amount;
        public string name;
    }

    public static InventoryManager Instance { get; private set; }

    private bool hasKey;
    private int gold;

    private Dictionary<string, int> potionsInventory = new Dictionary<string, int>();
    private string[] potionsNames;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one InventoryManager! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        BasePotion[] basePotionArray = PotionSystem.Instance.GetBasePotionArray();
        potionsNames = new string[basePotionArray.Length];

        for (int i = 0; i < basePotionArray.Length; i++)
        {
            potionsNames[i] = basePotionArray[i].GetName();
        }

        for (int i = 0; i < potionsNames.Length; ++i)
        {
            potionsInventory.Add(potionsNames[i], 0);
        }

        foreach (KeyValuePair<string, int> potions in potionsInventory)
        {
            Debug.Log($"Potion: {potions.Key} - Amount: {potions.Value}");
        }
    }

    public void SetHasKey(bool hasKey)
    {
        this.hasKey = hasKey;
    }

    public bool HasAKey()
    {
        return hasKey;
    }

    public int GetGoldAmount()
    {
        return gold;
    }

    public bool HasKeyPotion(string potionKey)
    {
        foreach (KeyValuePair<string, int> potions in potionsInventory)
        {
            if (potions.Key == potionKey)
            {
                return potions.Value != 0;
            }
        }

        return false;
    }

    public void RemovePotion(string potionKey)
    {
        foreach (KeyValuePair<string, int> potions in potionsInventory)
        {
            if (potions.Key == potionKey)
            {
                potionsInventory[potionKey]--;
                int newAmount = potionsInventory[potionKey];
                OnAnyPotionAmountChanged?.Invoke(this, new OnAnyPotionAmountChangedEventArgs
                {
                    amount = newAmount,
                    name = potionKey
                });

                return;
            }
        }
    }

    public void AddPotion(string potionKey)
    {
        foreach (KeyValuePair<string, int> potions in potionsInventory)
        {
            if (potions.Key == potionKey)
            {
                potionsInventory[potionKey]++;
                int newAmount = potionsInventory[potionKey];

                OnAnyPotionAmountChanged?.Invoke(this, new OnAnyPotionAmountChangedEventArgs
                {
                    amount = newAmount,
                    name = potionKey
                });

                return;
            }
        }
    }

    public void AddPotionsTesting()
    {
        for (int i = 0; i < potionsInventory.Count; ++i)
        {
            potionsInventory[potionsNames[i]]++;
            int newAmount = potionsInventory[potionsNames[i]];
            string potionName = potionsNames[i];
            OnAnyPotionAmountChanged?.Invoke(this, new OnAnyPotionAmountChangedEventArgs
            {
                amount = newAmount,
                name = potionName
            });
        }
    }
}
