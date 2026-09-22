using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UnitActionSystemUI : MonoBehaviour
{
    [SerializeField] private Transform actionButtonPrefab;
    [SerializeField] private Transform actionButtonContainterTransform;
    [SerializeField] private RectTransform backgroundRectTransform;
    [SerializeField] private float actionButtonCellSizeX;
    [SerializeField] private float backgroundHeight;
    [SerializeField] private float extraWidthAmount;

    private List<ActionButtonUI> actionButtonUIList;

    private void Awake()
    {
        actionButtonUIList = new List<ActionButtonUI>();
    }

    private void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;
        UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;
        UnitActionSystem.Instance.OnActionFailedNoResources += UnitActionSystem_OnActionFailedNoResources;
        Unit.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChanged;
        Unit.OnAnyEnergyChanged += Unit_OnAnyEnergyChanged;
        BaseAction.OnAnyActionCompleted += BaseAction_OnAnyActionCompleted;

        if (TurnSystem.Instance != null)
        {
            TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        }

        CreateUnitActionButtons();
        UpdateSelectedVisual();
        UpdateActionButtonsAvailability();
        AdjustBackgroundSize();
    }

    private void OnDestroy()
    {
        if (UnitActionSystem.Instance != null)
        {
            UnitActionSystem.Instance.OnSelectedUnitChanged -= UnitActionSystem_OnSelectedUnitChanged;
            UnitActionSystem.Instance.OnSelectedActionChanged -= UnitActionSystem_OnSelectedActionChanged;
            UnitActionSystem.Instance.OnActionFailedNoResources -= UnitActionSystem_OnActionFailedNoResources;
        }

        Unit.OnAnyActionPointsChanged -= Unit_OnAnyActionPointsChanged;
        Unit.OnAnyEnergyChanged -= Unit_OnAnyEnergyChanged;
        BaseAction.OnAnyActionCompleted -= BaseAction_OnAnyActionCompleted;

        if (TurnSystem.Instance != null)
        {
            TurnSystem.Instance.OnTurnChanged -= TurnSystem_OnTurnChanged;
        }
    }

    private void Unit_OnAnyActionPointsChanged(object sender, EventArgs e)
    {
        UpdateActionButtonsAvailability();
    }

    private void Unit_OnAnyEnergyChanged(object sender, EventArgs e)
    {
        UpdateActionButtonsAvailability();
    }

    private void BaseAction_OnAnyActionCompleted(object sender, EventArgs e)
    {
        UpdateActionButtonsAvailability();
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        UpdateActionButtonsAvailability();
    }

    private void UnitActionSystem_OnActionFailedNoResources(object sender, BaseAction failedAction)
    {
        foreach (ActionButtonUI actionButtonUI in actionButtonUIList)
        {
            if (actionButtonUI.GetBaseAction() == failedAction)
            {
                actionButtonUI.PlayJuiceFeedback();
                break;
            }
        }
    }

    private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e)
    {
        UpdateSelectedVisual();
        UpdateActionButtonsAvailability();
    }

    private void UpdateActionButtonsAvailability()
    {
        foreach (ActionButtonUI actionButtonUI in actionButtonUIList)
        {
            actionButtonUI.UpdateAvailabilityVisual();
        }
    }

    private void CreateUnitActionButtons()
    {
        foreach (Transform buttonTransform in actionButtonContainterTransform)
        {
            Destroy(buttonTransform.gameObject);
        }

        actionButtonUIList.Clear();

        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();

        foreach (BaseAction baseAction in selectedUnit.GetBaseActionArray())
        {
            Transform actionButtonTransform = Instantiate(actionButtonPrefab, actionButtonContainterTransform);
            ActionButtonUI actionButtonUI = actionButtonTransform.GetComponent<ActionButtonUI>();
            actionButtonUI.SetBaseAction(baseAction);
            actionButtonUI.SetTooltip(baseAction.GetTooltip());

            actionButtonUIList.Add(actionButtonUI);
        }
    }

    private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs e)
    {
        CreateUnitActionButtons();
        UpdateSelectedVisual();
    }

    private void UpdateSelectedVisual()
    {
        foreach (ActionButtonUI actionButtonUI in actionButtonUIList)
        {
            actionButtonUI.UpdateSelectedVisual();
        }
    }

    private void AdjustBackgroundSize()
    {
        float backgroundNewWidth = (actionButtonCellSizeX * (actionButtonUIList.Count + 1)) + extraWidthAmount; 
        backgroundRectTransform.sizeDelta = new Vector2(backgroundNewWidth, backgroundHeight);
    }
}
