using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionButtonUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI actionNameText;
    [SerializeField] private TextMeshProUGUI actionCostText;
    [SerializeField] private Button button;
    [SerializeField] private GameObject selectedGameObject;
    [SerializeField] private GameObject selectedGameObjectText;

    private BaseAction baseAction;

    /// <summary>
    /// Assigns the given action to the onClick event of the button
    /// </summary>
    /// <param name="baseAction">Action to perform</param>
    public void SetBaseAction(BaseAction baseAction)
    {
        this.baseAction = baseAction;
        actionNameText.text = baseAction.GetActionName().ToUpper();
        actionCostText.text = baseAction.GetEnergyCost().ToString();

        button.onClick.AddListener(() =>
        {
            UnitActionSystem.Instance.SetSelectedAction(baseAction);
        });
    }

    /// <summary>
    /// Sets the <see href="selectedGameObject"/> visual active if the <see href="selectedBaseAction"/>
    /// matches the local <see href="baseAction"/> (and deactivates it if it doesn't)
    /// </summary>
    public void UpdateSelectedVisual()
    {
        BaseAction selectedBaseAction = UnitActionSystem.Instance.GetSelectedAction();
        selectedGameObject.SetActive(selectedBaseAction == baseAction);
        selectedGameObjectText.SetActive(selectedBaseAction == baseAction);
    }
}
