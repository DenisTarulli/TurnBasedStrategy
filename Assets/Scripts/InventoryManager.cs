using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

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

    private bool hasKey;
    private int gold;

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
