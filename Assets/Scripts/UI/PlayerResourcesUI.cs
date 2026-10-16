using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerResourcesUI : MonoBehaviour
{
    public static PlayerResourcesUI Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI energyText;
    [SerializeField] private TextMeshProUGUI actionPointsText;
    [SerializeField] private TextMeshProUGUI nextTurnEnergyGainText;
    [SerializeField] private GameObject energyIconContainer;
    [SerializeField] private GameObject actionPointsIconContainer;
    [SerializeField] private Unit playerUnit;

    private GameObject[] energyIconArray;
    private GameObject[] actionPointsIconArray;
    private Image[] energyImageArray;
    private Image[] actionPointsImageArray;
    private Color[] energyOriginalColorArray;
    private Color[] actionPointsOriginalColorArray;

    private Coroutine previewCoroutine;
    private bool isHoveringAnotherAction = false;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one PlayerResourcesUI! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }

        Instance = this;
        SetIconsArray();
    }

    private void Start()
    {
        Unit.OnAnyEnergyChanged += Unit_OnAnyEnergyChanged;
        Unit.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChanged;
        playerUnit.OnPassiveEnergyGainChange += PlayerUnit_OnPassiveEnergyChange;
        BuffSystem.Instance.OnEnergyBuffChanged += BuffSystem_OnEnergyBuffChanged;

        if (UnitActionSystem.Instance != null)
        {
            UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;
            UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;
        }

        if (TurnSystem.Instance != null)
        {
            TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        }

        BaseAction.OnAnyActionCompleted += BaseAction_OnAnyActionCompleted;

        UpdatePlayerResourcesText(energyText, playerUnit.GetEnergy(), playerUnit.GetMaxEnergy());
        UpdatePlayerResourcesText(actionPointsText, playerUnit.GetActionPoints(), playerUnit.GetMaxActionPoints());
        UpdateNextTurnEnergyGainText();
        UpdateSelectedActionCostPreview();
    }

    private void OnDisable()
    {
        HideResourceCostPreview();
    }

    private void OnDestroy()
    {
        HideResourceCostPreview();

        Unit.OnAnyEnergyChanged -= Unit_OnAnyEnergyChanged;
        Unit.OnAnyActionPointsChanged -= Unit_OnAnyActionPointsChanged;
        if (playerUnit != null)
        {
            playerUnit.OnPassiveEnergyGainChange -= PlayerUnit_OnPassiveEnergyChange;
        }
        if (BuffSystem.Instance != null)
        {
            BuffSystem.Instance.OnEnergyBuffChanged -= BuffSystem_OnEnergyBuffChanged;
        }

        if (UnitActionSystem.Instance != null)
        {
            UnitActionSystem.Instance.OnSelectedActionChanged -= UnitActionSystem_OnSelectedActionChanged;
            UnitActionSystem.Instance.OnSelectedUnitChanged -= UnitActionSystem_OnSelectedUnitChanged;
        }

        if (TurnSystem.Instance != null)
        {
            TurnSystem.Instance.OnTurnChanged -= TurnSystem_OnTurnChanged;
        }

        BaseAction.OnAnyActionCompleted -= BaseAction_OnAnyActionCompleted;
    }

    private void UnitActionSystem_OnSelectedActionChanged(object sender, System.EventArgs e)
    {
        if (!isHoveringAnotherAction)
        {
            UpdateSelectedActionCostPreview();
        }
    }

    private void UnitActionSystem_OnSelectedUnitChanged(object sender, System.EventArgs e)
    {
        if (!isHoveringAnotherAction)
        {
            UpdateSelectedActionCostPreview();
        }
    }

    private void TurnSystem_OnTurnChanged(object sender, System.EventArgs e)
    {
        if (TurnSystem.Instance != null && !TurnSystem.Instance.IsPlayerTurn())
        {
            HideResourceCostPreview();
        }
        else
        {
            UpdateSelectedActionCostPreview();
        }
    }

    private void BaseAction_OnAnyActionCompleted(object sender, System.EventArgs e)
    {
        if (!isHoveringAnotherAction)
        {
            UpdateSelectedActionCostPreview();
        }
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
            RestoreAllIconColors();
            UpdatePlayerResourcesText(energyText, playerUnit.GetEnergy(), playerUnit.GetMaxEnergy());
            UpdatePlayerEnergyIcons(playerUnit.GetEnergy());
            UpdateNextTurnEnergyGainText();

            if (!isHoveringAnotherAction)
            {
                UpdateSelectedActionCostPreview();
            }
        }        
    }

    private void Unit_OnAnyActionPointsChanged(object sender, System.EventArgs e)
    {
        Unit unit = (Unit)sender;

        if (unit == playerUnit)
        {
            RestoreAllIconColors();
            UpdatePlayerResourcesText(actionPointsText, playerUnit.GetActionPoints(), playerUnit.GetMaxActionPoints());
            UpdatePlayerActionPointsIcons(playerUnit.GetActionPoints());
            UpdateNextTurnEnergyGainText();

            if (!isHoveringAnotherAction)
            {
                UpdateSelectedActionCostPreview();
            }
        }
    }

    private void SetIconsArray()
    {
        if (energyIconArray != null) return;

        int energyIconAmount = energyIconContainer.transform.childCount;
        int actionPointsIconAmount = actionPointsIconContainer.transform.childCount;

        energyIconArray = new GameObject[energyIconAmount];
        energyImageArray = new Image[energyIconAmount];
        energyOriginalColorArray = new Color[energyIconAmount];

        actionPointsIconArray = new GameObject[actionPointsIconAmount];
        actionPointsImageArray = new Image[actionPointsIconAmount];
        actionPointsOriginalColorArray = new Color[actionPointsIconAmount];

        for (int i = 0; i < energyIconAmount; i++)
        {
            energyIconArray[i] = energyIconContainer.transform.GetChild(i).gameObject;
            energyImageArray[i] = energyIconArray[i].GetComponent<Image>();
            if (energyImageArray[i] != null)
            {
                energyOriginalColorArray[i] = energyImageArray[i].color;
            }
        }

        for (int i = 0; i < actionPointsIconAmount; i++)
        {
            actionPointsIconArray[i] = actionPointsIconContainer.transform.GetChild(i).gameObject;
            actionPointsImageArray[i] = actionPointsIconArray[i].GetComponent<Image>();
            if (actionPointsImageArray[i] != null)
            {
                actionPointsOriginalColorArray[i] = actionPointsImageArray[i].color;
            }
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

    /// <summary>
    /// Muestra temporalmente el costo de la habilidad sobre la que el mouse está posado.
    /// </summary>
    public void ShowHoverResourceCostPreview(int energyCost, int actionPointsCost)
    {
        isHoveringAnotherAction = true;
        StartPreviewRoutine(energyCost, actionPointsCost);
    }

    /// <summary>
    /// Se invoca cuando el mouse sale de un botón de habilidad. Vuelve a mostrar el costo de la acción seleccionada.
    /// </summary>
    public void ClearHoverResourceCostPreview()
    {
        isHoveringAnotherAction = false;
        UpdateSelectedActionCostPreview();
    }

    /// <summary>
    /// Actualiza la previsualización al costo de la acción actualmente seleccionada por el jugador.
    /// </summary>
    public void UpdateSelectedActionCostPreview()
    {
        if (TurnSystem.Instance != null && !TurnSystem.Instance.IsPlayerTurn())
        {
            HideResourceCostPreview();
            return;
        }

        if (UnitActionSystem.Instance == null)
        {
            HideResourceCostPreview();
            return;
        }

        BaseAction selectedAction = UnitActionSystem.Instance.GetSelectedAction();
        if (selectedAction == null)
        {
            HideResourceCostPreview();
            return;
        }

        StartPreviewRoutine(selectedAction.GetEnergyCost(), selectedAction.GetActionPointsCost());
    }

    private void StartPreviewRoutine(int energyCost, int actionPointsCost)
    {
        if (previewCoroutine != null)
        {
            StopCoroutine(previewCoroutine);
            previewCoroutine = null;
        }

        RestoreAllIconColors();

        if (playerUnit == null)
        {
            if (UnitActionSystem.Instance != null && UnitActionSystem.Instance.GetSelectedUnit() != null)
            {
                playerUnit = UnitActionSystem.Instance.GetSelectedUnit();
            }
            else
            {
                return;
            }
        }

        previewCoroutine = StartCoroutine(ResourceCostPreviewRoutine(energyCost, actionPointsCost));
    }

    /// <summary>
    /// Mantiene compatibilidad con llamadas existentes a ShowResourceCostPreview.
    /// </summary>
    public void ShowResourceCostPreview(int energyCost, int actionPointsCost)
    {
        ShowHoverResourceCostPreview(energyCost, actionPointsCost);
    }

    /// <summary>
    /// Cancela la previsualización y restaura todos los iconos a su color y opacidad original.
    /// </summary>
    public void HideResourceCostPreview()
    {
        if (previewCoroutine != null)
        {
            StopCoroutine(previewCoroutine);
            previewCoroutine = null;
        }

        RestoreAllIconColors();
    }

    private void RestoreAllIconColors()
    {
        if (energyImageArray != null)
        {
            for (int i = 0; i < energyImageArray.Length; i++)
            {
                if (energyImageArray[i] != null)
                {
                    energyImageArray[i].color = energyOriginalColorArray[i];
                }
            }
        }

        if (actionPointsImageArray != null)
        {
            for (int i = 0; i < actionPointsImageArray.Length; i++)
            {
                if (actionPointsImageArray[i] != null)
                {
                    actionPointsImageArray[i].color = actionPointsOriginalColorArray[i];
                }
            }
        }
    }

    private IEnumerator ResourceCostPreviewRoutine(int energyCost, int actionPointsCost)
    {
        int currentEnergy = playerUnit.GetEnergy();
        int currentAP = playerUnit.GetActionPoints();

        bool hasEnoughEnergy = currentEnergy >= energyCost;
        bool hasEnoughAP = currentAP >= actionPointsCost;

        // Calcular índices de los rayitos de energía a previsualizar (los últimos que se gastarían)
        int energyStartIndex = Mathf.Max(0, currentEnergy - energyCost);
        int energyEndIndex = currentEnergy - 1;

        // Calcular índices de los Action Points a previsualizar
        int apStartIndex = Mathf.Max(0, currentAP - actionPointsCost);
        int apEndIndex = currentAP - 1;

        Color warningRed = new Color(0.95f, 0.25f, 0.25f, 1f);

        while (true)
        {
            float t = Mathf.PingPong(Time.unscaledTime * 4f, 1f);
            float alpha = Mathf.Lerp(0.2f, 1f, t);

            // Previsualización de Energía
            if (energyCost > 0)
            {
                for (int i = energyStartIndex; i <= energyEndIndex && i < energyImageArray.Length; i++)
                {
                    if (energyImageArray[i] != null && energyIconArray[i].activeSelf)
                    {
                        if (hasEnoughEnergy)
                        {
                            Color baseColor = energyOriginalColorArray[i];
                            energyImageArray[i].color = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);
                        }
                        else
                        {
                            energyImageArray[i].color = Color.Lerp(warningRed, new Color(warningRed.r, warningRed.g, warningRed.b, 0.3f), t);
                        }
                    }
                }
            }

            // Previsualización de Action Points
            if (actionPointsCost > 0)
            {
                for (int i = apStartIndex; i <= apEndIndex && i < actionPointsImageArray.Length; i++)
                {
                    if (actionPointsImageArray[i] != null && actionPointsIconArray[i].activeSelf)
                    {
                        if (hasEnoughAP)
                        {
                            Color baseColor = actionPointsOriginalColorArray[i];
                            actionPointsImageArray[i].color = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);
                        }
                        else
                        {
                            actionPointsImageArray[i].color = Color.Lerp(warningRed, new Color(warningRed.r, warningRed.g, warningRed.b, 0.3f), t);
                        }
                    }
                }
            }

            yield return null;
        }
    }
}
