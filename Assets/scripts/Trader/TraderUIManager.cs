using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Enums;
using TMPro;
using System;

public class TraderUIManager : MonoBehaviour
{
    [SerializeField] private Transform buttonsParent;
    [SerializeField] private GameObject buttonPrefab;
    //[SerializeField] private Button stashBuyButton;
    [SerializeField] private GameObject purchasedItemPanel;
    public GameObject shopPanel;
    public static TraderUIManager instance;
    public Button closeShopButton;
    public List<TraderUIElement> shopUIElements = new List<TraderUIElement>();
    public TextMeshProUGUI notificationText;
    private List<ShopItem> currentShopItems;
    private Trader trader;

    public void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        trader = FindObjectOfType<Trader>();
    }

    public void OpenShopUI()
    {
        Time.timeScale = 0;
        shopPanel.SetActive(true);
    }

    public void SetShopUIElements(List<TraderUIElement> uiElements)
    {
        shopUIElements = uiElements;
    }

    public void UpdateShopButtonsUI(List<ShopItem> items)
    {
        for (int i = 0; i < shopUIElements.Count; i++)
        {
            if (i < items.Count)
            {
                shopUIElements[i].UpdateUI(items[i]);
            }
            else
            {
                shopUIElements[i].gameObject.SetActive(false);
            }
        }
    }

    public void CloseShopUI()
    {
        Time.timeScale = 1;
        shopPanel.SetActive(false);
    }

    public void SetCurrentShopItems(List<ShopItem> items)
    {
        if (items == null)
        {
            Debug.LogError("SetCurrentShopItems called with null items list.");
            return; 
        }

        currentShopItems = items;

        foreach (Transform button in buttonsParent.transform)
        {
            Destroy(button.gameObject);
        }

        for (int i = 0; i < items.Count; i++)
        {
            Debug.Log(i);
            ShopItem item = currentShopItems[i];
            TraderUIElement traderButton = Instantiate(buttonPrefab, buttonsParent).GetComponent<TraderUIElement>();
            if (traderButton != null && traderButton.GetComponent<Button>() != null)
            {
                traderButton.GetComponent<Button>().onClick.AddListener(() => BuyItem(item));
                traderButton.UpdateUI(item);
            }
            else
            {
                Debug.LogError("TraderUIElement or Button component not found on instantiated prefab.");
            }
        }
    }

    public void BuyItem(ShopItem item)
    {
        ShopItem itemToBuy = item;
        Debug.Log($"Attempting to buy item: {itemToBuy.itemName}");

        if (GameManager.main.SpendGold(itemToBuy.price))
        {
            ApplyItemEffects(itemToBuy);
            ShowPurchasedItemIcon(itemToBuy.itemType, itemToBuy.itemIcon);
        }
        else
        {
            Debug.Log("Not enough currency or item not found.");
        }
    }

    private void ApplyItemEffects(ShopItem item)
    {
        PlayerMovement playerMovement = FindObjectOfType<PlayerMovement>();

        switch (item.itemType)
        {
            case ItemType.Helmet:
                HelmetItem helmet = item as HelmetItem;
                if (helmet != null)
                {
                    playerMovement.EquipHelmet(helmet);
                }
                break;

            case ItemType.Amulet:
                AmuletItem amulet = item as AmuletItem;
                if (amulet != null)
                {
                    playerMovement.EquipAmulet(amulet);
                }
                break;

            case ItemType.Ring:
                RingItem ring = item as RingItem;
                if (ring != null)
                {
                    playerMovement.EquipRing(ring);
                }
                break;

            case ItemType.Boots:
                BootsItem boots = item as BootsItem;
                if (boots != null)
                {
                    playerMovement.EquipBoots(boots);
                }
                break;

            case ItemType.Chestplate:
                ChestItem chestplate = item as ChestItem;
                if (chestplate != null)
                {
                    playerMovement.EquipChestplate(chestplate);
                }
                break;

            case ItemType.Gloves:
                GlovesItem gloves = item as GlovesItem;
                if (gloves != null)
                {
                    playerMovement.EquipGloves(gloves);
                }
                break;

            default:
                Debug.LogError("Unrecognized item type: " + item.itemType);
                break;
        }
    }

    private string GetIconNameFromItemType(ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.Helmet:
                Debug.Log("Helmet icon name is requested.");
                return "Helmet";

            case ItemType.Amulet:
                Debug.Log("Amulet icon name is requested.");
                return "Amulet";

            case ItemType.Ring:
                Debug.Log("Ring icon name is requested.");
                return "Ring";

            case ItemType.Gloves:
                Debug.Log("Gloves icon name is requested.");
                return "Gloves";

            case ItemType.Chestplate:
                Debug.Log("Chestplate icon name is requested.");
                return "Chestplate";

            case ItemType.Boots:
                Debug.Log("Boots icon name is requested.");
                return "Boots";

            case ItemType.Weapon:
                Debug.Log("Weapon icon name is requested.");
                return "Weapon";

            case ItemType.Ability:
                Debug.Log("Ability icon name is requested.");
                return "Ability";

            default:
                Debug.LogError($"Unrecognized ItemType: {itemType}");
                return null;
        }
    }

    private void ShowPurchasedItemIcon(ItemType itemType, Sprite itemIcon)
    {
        string iconName = GetIconNameFromItemType(itemType);
        Debug.Log($"Looking for UI element with name: {iconName}");

        if (string.IsNullOrEmpty(iconName))
        {
            Debug.LogError("Invalid item type for: " + itemType.ToString());
            return;
        }

        Image[] allItemIcons = purchasedItemPanel.GetComponentsInChildren<Image>(true);
        Debug.Log($"uzunluk: {allItemIcons.Length}");
        for (int i = 1; i < allItemIcons.Length; i++)
        {
            Image img = allItemIcons[i];
            if (img.gameObject.name == iconName)
            {
                img.sprite = itemIcon;
                img.gameObject.SetActive(true);
            }

            Debug.Log($"Found UI element with name: {img.gameObject.name}, active: {img.gameObject.activeSelf}");
        }
    }

    public void ShowNotification(string message, float duration)
    {
        if (notificationText != null)
        {
            notificationText.text = message;
            notificationText.gameObject.SetActive(true);
            StartCoroutine(HideNotificationAfterDuration(duration));
        }
        else
        {
            Debug.LogError("NotificationText is not assigned in the TraderUIManager.");
        }
    }

    private IEnumerator HideNotificationAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        if (notificationText != null)
        {
            notificationText.gameObject.SetActive(false);
        }
    }

    //public void UpdateUI()
    //{
    //    if (stashBuyButton != null)
    //    {
    //        stashBuyButton.gameObject.SetActive(trader.ShouldShowStashButton());
    //    }
    //    else
    //    {
    //        Debug.LogError("Stash buy button not assigned in TraderUIManager.");
    //    }
    //}
}
