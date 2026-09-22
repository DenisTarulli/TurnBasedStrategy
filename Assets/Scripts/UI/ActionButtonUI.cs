using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActionButtonUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TextMeshProUGUI actionNameText;
    [SerializeField] private TextMeshProUGUI actionCostText;
    [SerializeField] private TextMeshProUGUI actionPointsCostText;
    [SerializeField] private Button button;
    [SerializeField] private Image actionIcon;
    [SerializeField] private GameObject selectedGameObject;
    [SerializeField] private Transform tooltipParent;

    [Header("Cost Icons (Optional, auto-detected if empty)")]
    [SerializeField] private Image energyCostIcon;
    [SerializeField] private Image actionPointsCostIcon;

    [Header("Availability Colors")]
    [SerializeField] private Color enabledColor = Color.white;
    [SerializeField] private Color disabledColor = new Color(0.45f, 0.45f, 0.45f, 1f);

    private GameObject tooltip;
    private BaseAction baseAction;
    private UIJuiceFeedback uiJuiceFeedback;

    private Color originalEnergyCostTextColor = Color.white;
    private Color originalApCostTextColor = Color.white;
    private Color originalEnergyIconColor = Color.white;
    private Color originalApIconColor = Color.white;

    private void Awake()
    {
        uiJuiceFeedback = GetComponent<UIJuiceFeedback>();

        if (actionCostText != null)
        {
            originalEnergyCostTextColor = actionCostText.color;
            if (energyCostIcon == null && actionCostText.transform.parent != null)
            {
                foreach (Transform child in actionCostText.transform.parent)
                {
                    if (child != actionCostText.transform && child.TryGetComponent<Image>(out Image img))
                    {
                        energyCostIcon = img;
                        break;
                    }
                }
            }
        }

        if (actionPointsCostText != null)
        {
            originalApCostTextColor = actionPointsCostText.color;
            if (actionPointsCostIcon == null && actionPointsCostText.transform.parent != null)
            {
                foreach (Transform child in actionPointsCostText.transform.parent)
                {
                    if (child != actionPointsCostText.transform && child.TryGetComponent<Image>(out Image img))
                    {
                        actionPointsCostIcon = img;
                        break;
                    }
                }
            }
        }

        if (energyCostIcon != null)
        {
            originalEnergyIconColor = energyCostIcon.color;
        }

        if (actionPointsCostIcon != null)
        {
            originalApIconColor = actionPointsCostIcon.color;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (PlayerResourcesUI.Instance != null && baseAction != null)
        {
            PlayerResourcesUI.Instance.ShowHoverResourceCostPreview(baseAction.GetEnergyCost(), baseAction.GetActionPointsCost());
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (PlayerResourcesUI.Instance != null)
        {
            PlayerResourcesUI.Instance.ClearHoverResourceCostPreview();
        }
    }

    private void OnDisable()
    {
        if (PlayerResourcesUI.Instance != null)
        {
            PlayerResourcesUI.Instance.ClearHoverResourceCostPreview();
        }
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

        UpdateAvailabilityVisual();
    }

    /// <summary>
    /// Cambia el color del icono y nombre a blanco si la acción puede usarse, o a gris si faltan recursos.
    /// </summary>
    public void UpdateAvailabilityVisual()
    {
        Unit selectedUnit = UnitActionSystem.Instance != null ? UnitActionSystem.Instance.GetSelectedUnit() : null;
        bool canUse = selectedUnit != null && baseAction != null &&
                      selectedUnit.CanSpendActionPointsToTakeAction(baseAction) && 
                      selectedUnit.CanSpendEnergyToTakeAction(baseAction);

        Color targetColor = canUse ? enabledColor : disabledColor;

        if (actionIcon != null)
        {
            actionIcon.color = targetColor;
        }

        if (actionNameText != null)
        {
            actionNameText.color = targetColor;
        }

        if (actionCostText != null)
        {
            actionCostText.color = canUse ? originalEnergyCostTextColor : disabledColor;
        }

        if (actionPointsCostText != null)
        {
            actionPointsCostText.color = canUse ? originalApCostTextColor : disabledColor;
        }

        if (energyCostIcon != null)
        {
            energyCostIcon.color = canUse 
                ? originalEnergyIconColor 
                : new Color(originalEnergyIconColor.r * 0.45f, originalEnergyIconColor.g * 0.45f, originalEnergyIconColor.b * 0.45f, 0.7f);
        }

        if (actionPointsCostIcon != null)
        {
            actionPointsCostIcon.color = canUse 
                ? originalApIconColor 
                : new Color(originalApIconColor.r * 0.45f, originalApIconColor.g * 0.45f, originalApIconColor.b * 0.45f, 0.7f);
        }
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
