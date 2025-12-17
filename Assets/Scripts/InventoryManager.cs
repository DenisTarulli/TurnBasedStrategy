using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    private bool hasKey;
    private int gold;

    private Dictionary<string, int> potionsInventory = new Dictionary<string, int>();
    [SerializeField] private string[] potionsNames;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one InventoryManager! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }

        Instance = this;

        for (int i = 0; i < potionsNames.Length; ++i)
        {
            potionsInventory.Add(potionsNames[i], 0);
        }
    }

    private void Start()
    {
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
}
