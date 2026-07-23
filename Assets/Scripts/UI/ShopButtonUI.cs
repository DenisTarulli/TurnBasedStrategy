using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopButtonUI : MonoBehaviour
{
    [SerializeField] private GameObject selectedGameObject;
    [SerializeField] private Transform displayContainer;
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI buttonText;
    [SerializeField] private Color disabledTextColor;
    [SerializeField] private Image imagePrefab;

    private bool isSelected;

    private List<BaseReward> rewards;

    public bool IsSelected()
    {
        return isSelected;
    }

    public void Select()
    {
        isSelected = true;
    }

    public void Deselect()
    {
        isSelected = false;
    }

    public Button GetButton()
    {
        return button;
    }

    public void AddAndSetImageInContainer(Sprite newSprite, Color color)
    {
        Image newImage = Instantiate(imagePrefab, displayContainer);
        newImage.sprite = newSprite;
    }

    public void SetDisabledTextColor()
    {
        buttonText.color = disabledTextColor;
    }

    public void UpdateSelectedVisual()
    {
        if (isSelected)
        {
            selectedGameObject.SetActive(true);
        }
        else
        {
            selectedGameObject.SetActive(false);
        }
    }
}
