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
    public event EventHandler<OnGoldAmountChangedEventArgs> OnGoldAmountChanged;
    public class OnGoldAmountChangedEventArgs : EventArgs
    {
        public int gold;
    }
    public event EventHandler<OnKeysAmountChangedEventArgs> OnKeysAmountChanged;
    public class OnKeysAmountChangedEventArgs : EventArgs
    {
        public int keys;
    }

    public static InventoryManager Instance { get; private set; }

    private int keys;
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

        //foreach (KeyValuePair<string, int> potions in potionsInventory)
        //{
        //    Debug.Log($"Potion: {potions.Key} - Amount: {potions.Value}");
        //}
    }

    /// <summary>
    /// Add or remove x amount of keys
    /// </summary>
    /// <param name="amount">Amount to add/remove</param>
    public void ChangeKeysAmount(int amount)
    {
        this.keys += amount;

        if (keys < 0)
        {
            keys = 0;
        }

        OnKeysAmountChanged?.Invoke(this, new OnKeysAmountChangedEventArgs
        {
            keys = this.keys
        });
    }

    public bool HasKeys()
    {
        return keys > 0;
    }

    public int GetGoldAmount()
    {
        return gold;
    }

    public int GetKeysAmount()
    {
        return keys;
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
        ChangePotionAmount(potionKey, -1);
    }

    public void AddPotion(string potionKey)
    {
        ChangePotionAmount(potionKey, 1);
    }

    /// <summary>
    /// Add or remove x amount of potions matching the given key
    /// </summary>
    /// <param name="potionKey">Potion key name</param>
    /// <param name="amount">Amount to add/remove</param>
    public void ChangePotionAmount(string potionKey, int amount)
    {
        foreach (KeyValuePair<string, int> potions in potionsInventory)
        {
            if (potions.Key == potionKey)
            {
                potionsInventory[potionKey] += amount;
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

    public int GetSpecificPotionAmount(string potionKey)
    {
        foreach (KeyValuePair<string, int> potions in potionsInventory)
        {
            if (potions.Key == potionKey)
            {                
                return potions.Value;
            }
        }

        Debug.LogError($"There is no potion with the key: {potionKey}");
        return 0;
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

    public void ChangeGoldAmount(int amount)
    {
        gold += amount;

        if (gold < 0)
        {
            gold = 0;
        }

        OnGoldAmountChanged?.Invoke(this, new OnGoldAmountChangedEventArgs
        {
            gold = this.gold
        });
    }
}
