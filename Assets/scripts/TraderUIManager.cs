using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static Enums;

public class TraderUIManager : MonoBehaviour
{
    public Button buyItem1Button;
    public GameObject shopPanel;
    public static TraderUIManager instance;
    public Button closeShopButton;

    public RuntimeAnimatorController item1Animator; 

    public void Awake()
    {
        instance = this;
    }

    public void OpenShopUI()
    {
        Time.timeScale = 0;
        shopPanel.SetActive(true);
    }

    public void CloseShopUI()
    {
        Time.timeScale = 1;
        shopPanel.SetActive(false);
    }

    public void BuyItem1()
    {
        if (GameManager.main.currency >= 10)
        {
            GameManager.main.currency -= 10;
            PlayerMovement player = FindObjectOfType<PlayerMovement>();
            player.ChangeWeapon(15, 5f, 1, item1Animator);
        }
        else
        {
            Debug.Log("Not enough money to buy Item1");
        }
    }

    public void UpdateUI(GameObject uiElement, Trader.ShopItem item)
    {
        Image itemIcon = uiElement.transform.Find("ItemIcon").GetComponent<Image>();
        Text itemName = uiElement.transform.Find("ItemName").GetComponent<Text>();
        Text itemPrice = uiElement.transform.Find("ItemPrice").GetComponent<Text>();

        itemIcon.sprite = item.itemIcon;
        itemName.text = item.itemName;
        itemPrice.text = item.price.ToString();
    }
}