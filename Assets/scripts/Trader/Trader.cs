using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enums;

public class Trader : MonoBehaviour
{
    public float timeToStay = 10f;
    public string[] inventory;

    private float timer;

    public TraderUIManager traderUIManager;

    public List<TraderUIElement> shopUIElements = new List<TraderUIElement>();

    [System.Serializable]
    public class ShopItem
    {
        public string itemName;
        public int price;
        public Sprite itemIcon;
        public ItemType itemType;  
        public float itemDamageBonus;
        public float itemRangeBonus;
        public float armorBonus;
    }

    public List<ShopItem> allItems = new List<ShopItem>();

    public List<ShopItem> GetRandomItems(int numberOfItems)
    {
        List<ShopItem> randomItems = new List<ShopItem>();
        List<ShopItem> availableItems = new List<ShopItem>(allItems); 

        for (int i = 0; i < numberOfItems && availableItems.Count > 0; i++)
        {
            int randomIndex = Random.Range(0, availableItems.Count);
            randomItems.Add(availableItems[randomIndex]);
            availableItems.RemoveAt(randomIndex); 
        }

        return randomItems;
    }

    public void UpdateShopUI(List<ShopItem> itemsToShow)
    {
        if (shopUIElements == null)
        {
            Debug.LogError("shopUIElements is null");
            return;
        }

        if (itemsToShow == null)
        {
            Debug.LogError("itemsToShow is null");
            return;
        }

        foreach (var uiElement in shopUIElements)
        {
            if (uiElement == null)
            {
                Debug.LogWarning("A uiElement in shopUIElements is null");
                continue;
            }
            uiElement.gameObject.SetActive(false);
        }

        int maxItemsToShow = Mathf.Min(4, itemsToShow.Count);
        for (int i = 0; i < maxItemsToShow; i++)
        {
            if (shopUIElements[i] == null)
            {
                Debug.LogError($"shopUIElements[{i}] is null");
                continue;
            }

            shopUIElements[i].gameObject.SetActive(true);
            shopUIElements[i].UpdateUI(itemsToShow[i]);
        }
    }

    void Awake()
    {
        traderUIManager = TraderUIManager.instance;
    }

    void Start()
    {
        timer = timeToStay;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnMouseDown()
    {
            OpenShop();  
            timer = 0;  
    }

    public void OpenShop()
    {
        List<ShopItem> randomShopItems = GetRandomItems(4);
        UpdateShopUI(randomShopItems);
        traderUIManager.OpenShopUI();
        Debug.Log("Shop opened");
    }
}