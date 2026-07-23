using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PotionButtonUI : MonoBehaviour
{
    [SerializeField] private GameObject selectedGameObject;
    private BasePotion basePotion;

    public void UpdateSelectedVisual()
    {
        BasePotion selectedPotion = PotionSystem.Instance.GetSelectedPotion();
        selectedGameObject.SetActive(selectedPotion == basePotion);
    }

    public void SetBasePotion(BasePotion basePotion)
    {
        this.basePotion = basePotion;
    }
}
