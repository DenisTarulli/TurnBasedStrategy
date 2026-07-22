using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerResourcesUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI energyText;
    [SerializeField] private TextMeshProUGUI actionPointsText;
    [SerializeField] private TextMeshProUGUI nextTurnEnergyGainText;
    [SerializeField] private GameObject energyIconContainer;
    [SerializeField] private GameObject actionPointsIconContainer;
    [SerializeField] private Unit playerUnit;

    private GameObject[] energyIconArray;
    private GameObject[] actionPointsIconArray;

    private void Start()
    {
        Unit.OnAnyEnergyChanged += Unit_OnAnyEnergyChanged;
        Unit.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChanged;
        playerUnit.OnPassiveEnergyGainChange += PlayerUnit_OnPassiveEnergyChange;
        BuffSystem.Instance.OnEnergyBuffChanged += BuffSystem_OnEnergyBuffChanged;

        UpdatePlayerResourcesText(energyText, playerUnit.GetEnergy(), playerUnit.GetMaxEnergy());
        UpdatePlayerResourcesText(actionPointsText, playerUnit.GetActionPoints(), playerUnit.GetMaxActionPoints());
        UpdateNextTurnEnergyGainText();
        SetIconsArray();
    }

    private void BuffSystem_OnEnergyBuffChanged(object sender, System.EventArgs e)
    {
        UpdateNextTurnEnergyGainText();
    }

    private void PlayerUnit_OnPassiveEnergyChange(object sender, System.EventArgs e)
    {
        UpdateNextTurnEnergyGainText();
    }

    private void Unit_OnAnyEnergyChanged(object sender, System.EventArgs e)
    {
        Unit unit = (Unit)sender;

        if (unit == playerUnit)
        {
            UpdatePlayerResourcesText(energyText, playerUnit.GetEnergy(), playerUnit.GetMaxEnergy());
            UpdatePlayerEnergyIcons(playerUnit.GetEnergy());
            UpdateNextTurnEnergyGainText();
        }        
    }

    private void Unit_OnAnyActionPointsChanged(object sender, System.EventArgs e)
    {
        Unit unit = (Unit)sender;

        if (unit == playerUnit)
        {
            UpdatePlayerResourcesText(actionPointsText, playerUnit.GetActionPoints(), playerUnit.GetMaxActionPoints());
            UpdatePlayerActionPointsIcons(playerUnit.GetActionPoints());
            UpdateNextTurnEnergyGainText();
        }
    }

    private void SetIconsArray()
    {
        int energyIconAmount = energyIconContainer.transform.childCount;
        int actionPointsIconAmount = actionPointsIconContainer.transform.childCount;

        energyIconArray = new GameObject[energyIconAmount];
        actionPointsIconArray = new GameObject[actionPointsIconAmount];

        for (int i = 0; i < energyIconAmount; i++)
        {
            energyIconArray[i] = energyIconContainer.transform.GetChild(i).gameObject;
        }

        for (int i = 0; i < actionPointsIconAmount; i++)
        {
            actionPointsIconArray[i] = actionPointsIconContainer.transform.GetChild(i).gameObject;
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

    private void UpdatePlayerEnergyIcons(int energyCurrentValue)
    {
        for (int i = 0; i < energyIconArray.Length; i++)
        {
            if (energyCurrentValue <= i)
            {
                energyIconArray[i].SetActive(false);
            }
            else
            {
                energyIconArray[i].SetActive(true);
            }
        }
    }
    private void UpdatePlayerActionPointsIcons(int actionPointsCurrentValue)
    {
        for (int i = 0; i < actionPointsIconArray.Length; i++)
        {
            if (actionPointsCurrentValue <= i)
            {
                actionPointsIconArray[i].SetActive(false);
            }
            else
            {
                actionPointsIconArray[i].SetActive(true);
            }
        }
    }
}
