using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStatsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI energyStatText;
    [SerializeField] private TextMeshProUGUI healthStatText;
    [SerializeField] private TextMeshProUGUI powerStatText;
    [SerializeField] private TextMeshProUGUI resistanceStatText;
    [SerializeField] private TextMeshProUGUI speedStatText;
    [SerializeField] private HealthSystem playerUnitHealthSystem;

    private void Start()
    {
        PlayerStats.Instance.OnEnergyChanged += PlayerStats_OnEnergyChanged;
        PlayerStats.Instance.OnHealthChanged += PlayerStats_OnHealthChanged;
        PlayerStats.Instance.OnPowerChanged += PlayerStats_OnPowerChanged;
        PlayerStats.Instance.OnResistanceChanged += PlayerStats_OnResistanceChanged;
        PlayerStats.Instance.OnSpeedChanged += PlayerStats_OnSpeedChanged;

        playerUnitHealthSystem.OnHealthAmountChange += HealthSystem_OnHealthAmountChange;

        UpdateAllStatsText();
    }

    private void HealthSystem_OnHealthAmountChange(object sender, System.EventArgs e)
    {
        UpdateHealthStat();
    }

    private void PlayerStats_OnEnergyChanged(object sender, System.EventArgs e)
    {
        UpdateStatAmountText(energyStatText, PlayerStats.Instance.GetEnergy());
    }

    private void PlayerStats_OnHealthChanged(object sender, System.EventArgs e)
    {
        UpdateHealthStat();
    }

    private void PlayerStats_OnPowerChanged(object sender, System.EventArgs e)
    {
        UpdateStatAmountText(powerStatText, PlayerStats.Instance.GetPower());
    }

    private void PlayerStats_OnResistanceChanged(object sender, System.EventArgs e)
    {
        UpdateStatAmountText(resistanceStatText, PlayerStats.Instance.GetResistance());
    }

    private void PlayerStats_OnSpeedChanged(object sender, System.EventArgs e)
    {
        UpdateStatAmountText(speedStatText, PlayerStats.Instance.GetSpeed());
    }

    private void UpdateAllStatsText()
    {
        UpdateStatAmountText(energyStatText, PlayerStats.Instance.GetEnergy());
        UpdateHealthStat();
        UpdateStatAmountText(powerStatText, PlayerStats.Instance.GetPower());
        UpdateStatAmountText(resistanceStatText, PlayerStats.Instance.GetResistance());
        UpdateStatAmountText(speedStatText, PlayerStats.Instance.GetSpeed());
    }

    private void UpdateStatAmountText(TextMeshProUGUI textToUpdate, int amount)
    {
        textToUpdate.text = amount.ToString();
    }

    private void UpdateHealthStat()
    {
        healthStatText.text = $"{playerUnitHealthSystem.GetCurrentHealth()}/{playerUnitHealthSystem.GetMaxHealth()}";
    }
}
