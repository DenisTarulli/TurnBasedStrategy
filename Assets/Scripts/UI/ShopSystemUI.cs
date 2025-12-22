using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopSystemUI : MonoBehaviour
{
    public static ShopSystemUI Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one ShopSystemUI! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    [SerializeField] private Transform statsButtonsContainer;
    [SerializeField] private Transform itemsButtonsContainer;
    [SerializeField] private GameObject shopButtonPrefab;
    [SerializeField] private int totalOptions;

    [SerializeField] private BaseReward[] potionRewards;
    [SerializeField] private BaseReward[] statRewards;
    [SerializeField] private BaseReward goldReward;

    private List<GameObject> statsButtonList = new List<GameObject>();
    private List<GameObject> itemsButtonList = new List<GameObject>();

    private List<BaseReward> statsRewardsToGiveList = new List<BaseReward>();
    private List<BaseReward> itemsRewardsToGiveList = new List<BaseReward>();

    private List<BaseReward> rewardsToGiveList = new List<BaseReward>();

    private List<int> availableInts = new List<int>();

    private void Start()
    {
        PlayerStats.Instance.OnLevelUp += PlayerStats_OnLevelUp;
        ShopSystem.Instance.OnShopClosed += ShopSystem_OnShopClosed;

        Hide();
    }

    private void CreateShopButtons()
    {
        for (int i = 0; i < statRewards.Length; i++)
        {
            availableInts.Add(i);
        }

        for (int i = 0; i < totalOptions; i++)
        {
            GameObject buttonGameObject = Instantiate(shopButtonPrefab, statsButtonsContainer);
            ShopButtonUI shopButtonUI = buttonGameObject.GetComponent<ShopButtonUI>();
            statsButtonList.Add(buttonGameObject);

            if (i == 0)
            {
                int randomIndex = UnityEngine.Random.Range(0, availableInts.Count);
                Debug.Log(availableInts.Count);
                int randomStat = availableInts[randomIndex];
                availableInts.RemoveAt(randomIndex);

                shopButtonUI.AddAndSetImageInContainer(statRewards[randomStat].GetImage(), statRewards[randomStat].GetSpriteColor());

                shopButtonUI.GetButton().onClick.AddListener(() =>
                {
                    statsRewardsToGiveList.Clear();
                    ShopSystem.Instance.SetSelectedStatRewardButton(shopButtonUI);
                    statsRewardsToGiveList.Add(statRewards[randomStat]);
                });
            }
            else if (i == 1)
            {
                int randomIndex = UnityEngine.Random.Range(0, availableInts.Count);
                int randomStat = availableInts[randomIndex];
                availableInts.RemoveAt(randomIndex);

                shopButtonUI.AddAndSetImageInContainer(statRewards[randomStat].GetImage(), statRewards[randomStat].GetSpriteColor());

                shopButtonUI.GetButton().onClick.AddListener(() =>
                {
                    statsRewardsToGiveList.Clear();
                    ShopSystem.Instance.SetSelectedStatRewardButton(shopButtonUI);
                    statsRewardsToGiveList.Add(statRewards[randomStat]);
                });
            }
            else
            {
                int randomIndex = UnityEngine.Random.Range(0, availableInts.Count);
                int randomStat = availableInts[randomIndex];
                availableInts.RemoveAt(randomIndex);

                shopButtonUI.AddAndSetImageInContainer(statRewards[randomStat].GetImage(), statRewards[randomStat].GetSpriteColor());

                shopButtonUI.GetButton().onClick.AddListener(() =>
                {
                    statsRewardsToGiveList.Clear();
                    ShopSystem.Instance.SetSelectedStatRewardButton(shopButtonUI);
                    statsRewardsToGiveList.Add(statRewards[randomStat]);
                });
            }
        }

        availableInts.Clear();

        for (int i = 0; i < potionRewards.Length; i++)
        {
            availableInts.Add(i);
        }

        for (int i = 0; i < totalOptions; i++)
        {
            GameObject buttonGameObject = Instantiate(shopButtonPrefab, itemsButtonsContainer);
            ShopButtonUI shopButtonUI = buttonGameObject.GetComponent<ShopButtonUI>();
            itemsButtonList.Add(buttonGameObject);

            if (i == 0)
            {
                int randomIndex = UnityEngine.Random.Range(0, availableInts.Count);
                int randomPotion = availableInts[randomIndex];
                availableInts.RemoveAt(randomIndex);

                shopButtonUI.AddAndSetImageInContainer(potionRewards[randomPotion].GetImage(), potionRewards[randomPotion].GetSpriteColor());

                shopButtonUI.GetButton().onClick.AddListener(() =>
                {
                    itemsRewardsToGiveList.Clear();
                    ShopSystem.Instance.SetSelectedItemRewardButton(shopButtonUI);
                    itemsRewardsToGiveList.Add(potionRewards[randomPotion]);
                });
            }
            else if (i == 1)
            {
                int randomIndex = UnityEngine.Random.Range(0, availableInts.Count);
                int randomPotion = availableInts[randomIndex];
                availableInts.RemoveAt(randomIndex);

                shopButtonUI.AddAndSetImageInContainer(potionRewards[randomPotion].GetImage(), potionRewards[randomPotion].GetSpriteColor());

                shopButtonUI.GetButton().onClick.AddListener(() =>
                {
                    itemsRewardsToGiveList.Clear();
                    ShopSystem.Instance.SetSelectedItemRewardButton(shopButtonUI);
                    itemsRewardsToGiveList.Add(potionRewards[randomPotion]);
                });
            }
            else
            {
                shopButtonUI.AddAndSetImageInContainer(goldReward.GetImage(), goldReward.GetSpriteColor());

                shopButtonUI.GetButton().onClick.AddListener(() =>
                {
                    itemsRewardsToGiveList.Clear();
                    ShopSystem.Instance.SetSelectedItemRewardButton(shopButtonUI);
                    itemsRewardsToGiveList.Add(goldReward);
                });
            }
        }

        availableInts.Clear();
    }

    private void ShopSystem_OnShopClosed(object sender, System.EventArgs e)
    {
        rewardsToGiveList.Clear();

        Hide();

        // Another iterarion of change exp to check if the spare exp is enough to level up
        PlayerStats.Instance.ChangeExp(0);
    }

    private void PlayerStats_OnLevelUp(object sender, System.EventArgs e)
    {
        Show();
    }

    private void Show()
    {
        gameObject.SetActive(true);
        ShopSystem.Instance.OpenShop();
        CreateShopButtons();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    public List<GameObject> GetStatsButtonList()
    {
        return statsButtonList;
    }

    public List<GameObject> GetItemsButtonList()
    {
        return itemsButtonList;
    }

    public List<BaseReward> GetRewardsToGiveList()
    {
        foreach (BaseReward reward in statsRewardsToGiveList)
        {
            rewardsToGiveList.Add(reward);
        }

        foreach (BaseReward reward in itemsRewardsToGiveList)
        {
            rewardsToGiveList.Add(reward);
        }

        return rewardsToGiveList;
    }

    private void OnDisable()
    {
        foreach (GameObject button in statsButtonList)
        {
            Destroy(button);
        }

        foreach (GameObject button in itemsButtonList)
        {
            Destroy(button);
        }

        statsButtonList.Clear();
        itemsButtonList.Clear();
    }
}
