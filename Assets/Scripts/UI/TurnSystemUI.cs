using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnSystemUI : MonoBehaviour
{
    [SerializeField] private Button endTurnButton;
    [SerializeField] private Color endTurnEnabledColor;
    [SerializeField] private Color endTurnDisabledColor;
    [SerializeField] private TextMeshProUGUI turnNumberText;
    [SerializeField] private GameObject enemyTurnVisualGameObject;
    private Unit playerUnit;
    private TextMeshProUGUI endTurnButtonText;

    private void Start()
    {
        playerUnit = UnitManager.Instance.GetFriendlyUnitList()[0];
        endTurnButtonText = endTurnButton.GetComponentInChildren<TextMeshProUGUI>();

        //endTurnButton.onClick.AddListener(() =>
        //{
        //    TurnSystem.Instance.NextTurn();
        //});

        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        BaseAction.OnAnyActionStarted += BaseAction_OnAnyActionStarted;
        BaseAction.OnAnyActionCompleted += BaseAction_OnAnyActionCompleted;
        TurnSystem.Instance.OnNewRoomEntered += TurnSystem_OnNewRoomEntered;

        UpdateTurnText();
        UpdateEnemyTurnVisual();
        UpdateEndTurnButtonVisibility();
    }

    private void TurnSystem_OnNewRoomEntered(object sender, EventArgs e)
    {
        UpdateTurnText();
    }

    private void BaseAction_OnAnyActionStarted(object sender, EventArgs e)
    {
        BaseAction senderAction = sender as BaseAction;
        Unit senderUnit = senderAction.gameObject.GetComponent<Unit>();

        if (senderUnit != playerUnit)
        {
            return;
        }

        ToggleEndTurnButton();
    }

    private void BaseAction_OnAnyActionCompleted(object sender, EventArgs e)
    {
        BaseAction senderAction = sender as BaseAction;
        Unit senderUnit = senderAction.gameObject.GetComponent<Unit>();

        if (senderUnit != playerUnit)
        {
            return;
        }

        ToggleEndTurnButton();
    }

    private void ToggleEndTurnButton()
    {
        endTurnButton.interactable = !endTurnButton.interactable;

        if (endTurnButton.interactable)
        {
            endTurnButtonText.color = endTurnEnabledColor;
        }
        else
        {
            endTurnButtonText.color = endTurnDisabledColor;
        }
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        UpdateTurnText();
        UpdateEnemyTurnVisual();
        UpdateEndTurnButtonVisibility();
    }

    public void UpdateTurnText()
    {
        turnNumberText.text = $"TURN {TurnSystem.Instance.GetTurnNumber()}/{TurnSystem.Instance.GetTurnLimit()}";
    }

    private void UpdateEnemyTurnVisual()
    {
        enemyTurnVisualGameObject.SetActive(!TurnSystem.Instance.IsPlayerTurn());
    }

    private void UpdateEndTurnButtonVisibility()
    {
        endTurnButton.gameObject.SetActive(TurnSystem.Instance.IsPlayerTurn());
    }
}
