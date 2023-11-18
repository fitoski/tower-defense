using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static Enums;
using TMPro;

public class TraderUIManager : MonoBehaviour
{
    public Button buyItem1Button;
    public Button buyItem2Button;
    public Button buyItem3Button;
    public Button buyItem4Button;
    public Button buyItem5Button;


    public GameObject shopPanel;
    public static TraderUIManager instance;
    public Button closeShopButton;

    public RuntimeAnimatorController item1Animator;
    public RuntimeAnimatorController item2Animator;
    public RuntimeAnimatorController item3Animator;
    public RuntimeAnimatorController item4Animator;
    public RuntimeAnimatorController item5Animator;

    public TextMeshProUGUI notificationText;

    public void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
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

    public void BuyItem2()
    {
        if (GameManager.main.currency >= 10)
        {
            GameManager.main.currency -= 10;
            PlayerMovement player = FindObjectOfType<PlayerMovement>();
            player.ChangeWeapon(15, 5f, 1, item2Animator);
        }
        else
        {
            Debug.Log("Not enough money to buy Item1");
        }
    }

    public void BuyItem3()
    {
        if (GameManager.main.currency >= 10)
        {
            GameManager.main.currency -= 10;
            PlayerMovement player = FindObjectOfType<PlayerMovement>();
            player.ChangeWeapon(15, 5f, 1, item3Animator);
        }
        else
        {
            Debug.Log("Not enough money to buy Item1");
        }
    }

    public void BuyItem4()
    {
        if (GameManager.main.currency >= 10)
        {
            GameManager.main.currency -= 10;
            PlayerMovement player = FindObjectOfType<PlayerMovement>();
            player.ChangeWeapon(15, 5f, 1, item4Animator);
        }
        else
        {
            Debug.Log("Not enough money to buy Item1");
        }
    }

    public void BuyItem5()
    {
        if (GameManager.main.currency >= 10)
        {
            GameManager.main.currency -= 10;
            PlayerMovement player = FindObjectOfType<PlayerMovement>();
            player.ChangeWeapon(15, 5f, 1, item5Animator);
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

}