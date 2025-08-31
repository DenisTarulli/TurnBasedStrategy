using System;
using UnityEngine;
using UnityEngine.UI;

public class UnitActionSystemUI : MonoBehaviour
{
    [SerializeField] private Transform actionButtonPrefab;
    [SerializeField] private Transform actionButtonContainterTransform;

    private void Start()
    {
        CreateUnitActionButtons();

        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;
    }

    private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs e)
    {
        CreateUnitActionButtons();
    }

    private void CreateUnitActionButtons()
    {
        foreach (Transform buttonTransform in actionButtonContainterTransform)
        {
            Destroy(buttonTransform.gameObject);
        }

        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();

        foreach (BaseAction baseAction in selectedUnit.GetBaseActionArray())
        {
            Transform actionButtonTransform = Instantiate(actionButtonPrefab, actionButtonContainterTransform);
            ActionButtonUI actionButtonUI = actionButtonTransform.GetComponent<ActionButtonUI>();
            actionButtonUI.SetBaseAction(baseAction);
        }
    }
}
