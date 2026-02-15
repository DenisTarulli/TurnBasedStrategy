using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private int goldReward = 16;

    private ShopButtonUI selectedStatReward;
    private ShopButtonUI selectedItemReward;

    public void OpenShop()
    {
        shopOpen = true;
        Time.timeScale = 0f;
        SoundManager.Instance.PauseSFX();
    }

    public void CloseShop()
    {
        Time.timeScale = 1f;
        SoundManager.Instance.ResumeSFX();

        ClaimRewards();

        shopOpen = false;
        goldReward -= 2;
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
    }

    public void SetSelectedItemRewardButton(ShopButtonUI button)
    {
        selectedItemReward = button;
        UpdateSelectedVisual();
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
}
