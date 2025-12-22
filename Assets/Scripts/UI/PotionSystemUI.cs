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

    private BasePotion[] basePotionArray;

    private void Start()
    {
        basePotionArray = PotionSystem.Instance.GetBasePotionArray();

        CreatePotionButtons();

        InventoryManager.Instance.OnAnyPotionAmountChanged += InventoryManager_OnAnyPotionAmountChanged;
    }

    private void InventoryManager_OnAnyPotionAmountChanged(object sender, InventoryManager.OnAnyPotionAmountChangedEventArgs e)
    {
        for (int i = 0; i < basePotionArray.Length; i++)
        {
            if (basePotionArray[i].GetName() == e.name)
            {
                TextMeshProUGUI potionText = basePotionArray[i].GetPotionAmountTextObject();
                UpdatePotionAmountText(potionText, e.amount);
            }
        }
    }

    private void CreatePotionButtons()
    {
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

    private void UpdatePotionAmountText(TextMeshProUGUI potionText, int newAmount)
    {
        potionText.text = newAmount.ToString();
    }
}

