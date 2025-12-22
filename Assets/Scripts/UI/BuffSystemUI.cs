using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffSystemUI : MonoBehaviour
{
    [SerializeField] private GameObject energyBuffActiveVisual;
    [SerializeField] private GameObject healthBuffActiveVisual;
    [SerializeField] private GameObject powerBuffActiveVisual;
    [SerializeField] private GameObject resistanceBuffActiveVisual;
    [SerializeField] private GameObject speedBuffActiveVisual;

    private void Start()
    {
        BuffSystem.Instance.OnEnergyBuffChanged += BuffSystem_OnEnergyBuffChanged;
        BuffSystem.Instance.OnHealthBuffChanged += BuffSystem_OnHealthBuffChanged;
        BuffSystem.Instance.OnPowerBuffChanged += BuffSystem_OnPowerBuffChanged;
        BuffSystem.Instance.OnResistanceBuffChanged += BuffSystem_OnResistanceBuffChanged;
        BuffSystem.Instance.OnSpeedBuffChanged += BuffSystem_OnSpeedBuffChanged;
    }

    private void BuffSystem_OnEnergyBuffChanged(object sender, System.EventArgs e)
    {
        ToggleBuffVisual(BuffSystem.Instance.IsEnergyBuffActive(), energyBuffActiveVisual);
    }

    private void BuffSystem_OnHealthBuffChanged(object sender, System.EventArgs e)
    {
        ToggleBuffVisual(BuffSystem.Instance.IsHealthBuffActive(), healthBuffActiveVisual);
    }    

    private void BuffSystem_OnPowerBuffChanged(object sender, System.EventArgs e)
    {
        ToggleBuffVisual(BuffSystem.Instance.IsPowerBuffActive(), powerBuffActiveVisual);
    }

    private void BuffSystem_OnResistanceBuffChanged(object sender, System.EventArgs e)
    {
        ToggleBuffVisual(BuffSystem.Instance.IsResistanceBuffActive(), resistanceBuffActiveVisual);
    }

    private void BuffSystem_OnSpeedBuffChanged(object sender, System.EventArgs e)
    {
        ToggleBuffVisual(BuffSystem.Instance.IsSpeedBuffActive(), speedBuffActiveVisual);
    }

    private void ToggleBuffVisual(bool state, GameObject visual)
    {
        visual.SetActive(state);
    }
}
