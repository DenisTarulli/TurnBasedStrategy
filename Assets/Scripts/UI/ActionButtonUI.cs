using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionButtonUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI actionNameText;
    [SerializeField] private TextMeshProUGUI actionCostText;
    [SerializeField] private TextMeshProUGUI actionPointsCostText;
    [SerializeField] private Button button;
    [SerializeField] private Image actionIcon;
    [SerializeField] private GameObject selectedGameObject;
    [SerializeField] private Transform tooltipParent;

    private GameObject tooltip;
    private BaseAction baseAction;
    private UIJuiceFeedback uiJuiceFeedback;

    private void Awake()
    {
        uiJuiceFeedback = GetComponent<UIJuiceFeedback>();
    }

    /// <summary>
    /// Assigns the given action to the onClick event of the button
    /// </summary>
    /// <param name="baseAction">Action to perform</param>
    public void SetBaseAction(BaseAction baseAction)
    {
        this.baseAction = baseAction;
        actionIcon.sprite = baseAction.GetIcon();
        actionNameText.text = baseAction.GetActionName().ToUpper();
        actionCostText.text = baseAction.GetEnergyCost().ToString();
        actionPointsCostText.text = "1"; 

        button.onClick.AddListener(() =>
        {
            Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
            if (selectedUnit != null && 
                (!selectedUnit.CanSpendActionPointsToTakeAction(baseAction) || 
                 !selectedUnit.CanSpendEnergyToTakeAction(baseAction)))
            {
                PlayJuiceFeedback();
            }

            UnitActionSystem.Instance.SetSelectedAction(baseAction);
        });
    }

    public void PlayJuiceFeedback()
    {
        if (uiJuiceFeedback != null)
        {
            uiJuiceFeedback.PlayJuice();
        }
    }

    public BaseAction GetBaseAction()
    {
        return baseAction;
    }

    /// <summary>
    /// Sets the <see href="selectedGameObject"/> visual active if the <see href="selectedBaseAction"/>
    /// matches the local <see href="baseAction"/> (and deactivates it if it doesn't)
    /// </summary>
    public void UpdateSelectedVisual()
    {
        BaseAction selectedBaseAction = UnitActionSystem.Instance.GetSelectedAction();
        selectedGameObject.SetActive(selectedBaseAction == baseAction);
    }

    public void SetTooltip(GameObject tooltip)
    {
        this.tooltip = tooltip;
        Instantiate(tooltip, tooltipParent);
    }

    public GameObject GetTooltip()
    {
        return tooltip;
    }
}
