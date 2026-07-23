using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopSystem : MonoBehaviour
{
    public static ShopSystem Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one ShopSystem! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public event EventHandler OnShopClosed;

    private bool shopOpen = false;
    private int goldReward = 15;

    private ShopButtonUI selectedStatReward;
    private ShopButtonUI selectedItemReward;

    [SerializeField] private Button confirmButton;
    [SerializeField] private TextMeshProUGUI confirmButtonText;
    [SerializeField] private Color disabledColor;
    [SerializeField] private Color enabledColor;

    public void OpenShop()
    {
        shopOpen = true;
        Time.timeScale = 0f;
        ToggleConfirmButton();
    }

    public void CloseShop()
    {
        Time.timeScale = 1f;

        ClaimRewards();

        shopOpen = false;
        OnShopClosed?.Invoke(this, EventArgs.Empty);
    }

    private void ClaimRewards()
    {
        List<BaseReward> rewards = ShopSystemUI.Instance.GetRewardsToGiveList();

        foreach (BaseReward reward in rewards)
        {
            reward.Behaviour();
        }
    }

    private bool AreBothRewardsSelected()
    {
        if (selectedStatReward == null || selectedItemReward == null)
        {
            return false;
        }
        else 
            return true;
    }

    public bool IsShopOpen()
    {
        return shopOpen;
    }

    public int GetGoldReward()
    {
        return goldReward;
    }

    public void SetSelectedStatRewardButton(ShopButtonUI button)
    {
        selectedStatReward = button;
        UpdateSelectedVisual();
        ToggleConfirmButton();
    }

    public void SetSelectedItemRewardButton(ShopButtonUI button)
    {
        selectedItemReward = button;
        UpdateSelectedVisual();
        ToggleConfirmButton();
    }

    public ShopButtonUI GetSelectedStatReward()
    {
        return selectedStatReward;
    }

    public ShopButtonUI GetSelectedItemReward()
    {
        return selectedItemReward;
    }

    public void UpdateSelectedVisual()
    {
        foreach (GameObject button in ShopSystemUI.Instance.GetStatsButtonList())
        {
            ShopButtonUI shopButtonUI = button.GetComponent<ShopButtonUI>();

            if (shopButtonUI == selectedStatReward)
            {
                shopButtonUI.Select();
            }
            else
            {
                shopButtonUI.Deselect();
            }

            shopButtonUI.UpdateSelectedVisual();
        }

        foreach (GameObject button in ShopSystemUI.Instance.GetItemsButtonList())
        {
            ShopButtonUI shopButtonUI = button.GetComponent<ShopButtonUI>();

            if (shopButtonUI == selectedItemReward)
            {
                shopButtonUI.Select();
            }
            else
            {
                shopButtonUI.Deselect();
            }

            shopButtonUI.UpdateSelectedVisual();
        }
    }

    private void ToggleConfirmButton()
    {
        if (AreBothRewardsSelected())
        {
            confirmButtonText.color = enabledColor;
            confirmButton.interactable = true;
        }
        else
        {
            confirmButtonText.color = disabledColor;
            confirmButton.interactable = false;
        }
    }
}
