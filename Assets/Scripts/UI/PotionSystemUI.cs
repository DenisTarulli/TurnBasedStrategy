using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PotionSystemUI : MonoBehaviour
{
    [SerializeField] private Transform potionButtonPrefab;
    [SerializeField] private Transform potionButtonContainerTransform;
    [SerializeField] private RectTransform backgroundRectTransform;
    [SerializeField] private float potionButtonCellSizeY;
    [SerializeField] private float backgroundWidth;
    [SerializeField] private float extraHeightAmount;

    private BasePotion[] basePotionArray;
    private PotionButtonUI[] potionButtonUIArray;

    private void Start()
    {
        basePotionArray = PotionSystem.Instance.GetBasePotionArray();

        CreatePotionButtons();
        AdjustBackgroundSize();

        potionButtonUIArray = new PotionButtonUI[potionButtonContainerTransform.childCount];
        Debug.Log(potionButtonContainerTransform.childCount);

        for (int i = 0; i < potionButtonContainerTransform.childCount; i++)
        {
            potionButtonUIArray[i] = potionButtonContainerTransform.GetChild(i).GetComponent<PotionButtonUI>();
        }

        InventoryManager.Instance.OnAnyPotionAmountChanged += InventoryManager_OnAnyPotionAmountChanged;
        PotionSystem.Instance.OnSelectedPotionChange += PotionSystem_OnSelectedPotionChange;
    }

    private void PotionSystem_OnSelectedPotionChange(object sender, System.EventArgs e)
    {
        foreach (PotionButtonUI potionButtonUI in potionButtonUIArray)
        {
            potionButtonUI.UpdateSelectedVisual();
        }
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
            potionButtonTransform.GetChild(0).GetComponent<Image>().sprite = basePotion.GetSprite();
            potionButtonTransform.GetComponent<PotionButtonUI>().SetBasePotion(basePotion);

            potionButtonTransform.GetComponent<Button>().onClick.AddListener(() =>
            {
                basePotion.SelectPotion();
            });

            basePotion.SetPotionAmountTextObject(potionButtonTransform.GetComponentInChildren<TextMeshProUGUI>());
        }
    }

    private void UpdatePotionAmountText(TextMeshProUGUI potionText, int newAmount)
    {
        potionText.text = newAmount.ToString();
    }

    private void AdjustBackgroundSize()
    {
        float backgroundNewHeight = (potionButtonCellSizeY * (basePotionArray.Length + 1)) + extraHeightAmount;
        backgroundRectTransform.sizeDelta = new Vector2(backgroundWidth, backgroundNewHeight);
    }
}

