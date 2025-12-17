using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionSystem : MonoBehaviour
{
    public static PotionSystem Instance { get; private set; }

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
}
