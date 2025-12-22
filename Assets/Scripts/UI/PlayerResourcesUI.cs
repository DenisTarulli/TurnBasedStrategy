using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerResourcesUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI energyText;
    [SerializeField] private TextMeshProUGUI actionPointsText;
    [SerializeField] private TextMeshProUGUI nextTurnEnergyGainText;
    [SerializeField] private Unit playerUnit;

    private void Start()
    {
        Unit.OnAnyEnergyChanged += Unit_OnAnyEnergyChanged;
        Unit.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChanged;
        playerUnit.OnMaxEnergyChanged += PlayerUnit_OnMaxEnergyChanged;
        BuffSystem.Instance.OnEnergyBuffChanged += BuffSystem_OnEnergyBuffChanged;

        UpdatePlayerResourcesText(energyText, playerUnit.GetEnergy(), playerUnit.GetMaxEnergy());
        UpdatePlayerResourcesText(actionPointsText, playerUnit.GetActionPoints(), playerUnit.GetMaxActionPoints());
        UpdateNextTurnEnergyGainText();
    }

    private void BuffSystem_OnEnergyBuffChanged(object sender, System.EventArgs e)
    {
        UpdateNextTurnEnergyGainText();
    }

    private void PlayerUnit_OnMaxEnergyChanged(object sender, System.EventArgs e)
    {
        UpdatePlayerResourcesText(energyText, playerUnit.GetEnergy(), playerUnit.GetMaxEnergy());
    }

    private void Unit_OnAnyEnergyChanged(object sender, System.EventArgs e)
    {
        Unit unit = (Unit)sender;

        if (unit == playerUnit)
        {
            UpdatePlayerResourcesText(energyText, playerUnit.GetEnergy(), playerUnit.GetMaxEnergy());
            UpdateNextTurnEnergyGainText();
        }        
    }

    private void Unit_OnAnyActionPointsChanged(object sender, System.EventArgs e)
    {
        Unit unit = (Unit)sender;

        if (unit == playerUnit)
        {
            UpdatePlayerResourcesText(actionPointsText, playerUnit.GetActionPoints(), playerUnit.GetMaxActionPoints());
            UpdateNextTurnEnergyGainText();
        }
    }

    private void UpdatePlayerResourcesText(TextMeshProUGUI textToUpdate, int currentValue, int maxValue)
    {
        textToUpdate.text = $"{currentValue}/{maxValue}";
    }

    private void UpdateNextTurnEnergyGainText()
    {
        nextTurnEnergyGainText.text = $"+{playerUnit.GetNextTurnEnergyRegen()}";
    }
}
