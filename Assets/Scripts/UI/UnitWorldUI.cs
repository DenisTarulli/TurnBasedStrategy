using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitWorldUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI actionPointsText;
    [SerializeField] private TextMeshProUGUI energyText;
    [SerializeField] private Unit unit;
    [SerializeField] private Image healthBarImage;
    [SerializeField] private HealthSystem healthSystem;

    private void Start()
    {
        if (!unit.IsEnemy())
        {
            Unit.OnAnyEnergyChanged += Unit_OnAnyEnergyChanged;
            UpdateEnergyText();
            PlayerStats.Instance.OnHealthChanged += PlayerStats_OnHealthChanged;
        }

        Unit.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChanged;
        healthSystem.OnHealthAmountChange += HealthSystem_OnHealthAmountChange;        

        UpdateActionsPointsText();
        UpdateHealthBar();
    }

    private void PlayerStats_OnHealthChanged(object sender, EventArgs e)
    {
        UpdateHealthBar();
    }

    private void UpdateActionsPointsText()
    {
        actionPointsText.text = unit.GetActionsPoints().ToString();
    }

    private void UpdateEnergyText()
    {
        energyText.text = unit.GetEnergy().ToString();
    }

    private void Unit_OnAnyEnergyChanged(object sender, EventArgs e)
    {
        UpdateEnergyText();
    }

    private void Unit_OnAnyActionPointsChanged(object sender, EventArgs e)
    {
        UpdateActionsPointsText();
    }

    public void UpdateHealthBar()
    {
        healthBarImage.fillAmount = healthSystem.GetHealthNormalized();
    }

    private void HealthSystem_OnHealthAmountChange(object sender, EventArgs e)
    {
        UpdateHealthBar();
    }
}
