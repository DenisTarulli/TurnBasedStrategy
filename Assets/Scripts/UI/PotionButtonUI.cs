using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PotionButtonUI : MonoBehaviour
{
    [SerializeField] private GameObject selectedGameObject;
    [SerializeField] private Transform tooltipParent;
    private BasePotion basePotion;

    private GameObject tooltip;

    public void UpdateSelectedVisual()
    {
        BasePotion selectedPotion = PotionSystem.Instance.GetSelectedPotion();
        selectedGameObject.SetActive(selectedPotion == basePotion);
    }

    public void SetBasePotion(BasePotion basePotion)
    {
        this.basePotion = basePotion;
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
