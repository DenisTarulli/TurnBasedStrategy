using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PotionSystemUI : MonoBehaviour
{
    [SerializeField] private Transform potionButtonPrefab;
    [SerializeField] private Transform potionButtonContainerTransform;
    //[SerializeField] private float disabledButtonColorAlpha = 55;

    private void Start()
    {
        CreatePotionButtons();
    }

    private void CreatePotionButtons()
    {
        BasePotion[] basePotionArray = PotionSystem.Instance.GetBasePotionArray();

        foreach (BasePotion basePotion in basePotionArray)
        {
            Transform potionButtonTransform = Instantiate(potionButtonPrefab, potionButtonContainerTransform);
            potionButtonTransform.GetChild(0).GetComponent<Image>().color = basePotion.GetColor();

            potionButtonTransform.GetComponent<Button>().onClick.AddListener(() =>
            {
                basePotion.ConsumePotion();
            });
            
            basePotion.SetPotionAmountTextObject(potionButtonTransform.GetComponentInChildren<TextMeshProUGUI>());
        }
    }
}

