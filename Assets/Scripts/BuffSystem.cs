using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffSystem : MonoBehaviour
{
    public static BuffSystem Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one BuffSystem! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void ApplyDamageModifier(Unit unit, int modifier)
    {
        HealthSystem healthSystem = unit.GetComponent<HealthSystem>();
        healthSystem.SetDamageModifier(modifier);
    }
}
