using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TraderUIManager : MonoBehaviour
{
    public Button buyItem1Button;
    public GameObject shopPanel;
    public static TraderUIManager instance;
    public Button closeShopButton;

    public void Awake()
    {
        instance = this;
    }

    public void OpenShopUI()
    {
        shopPanel.SetActive(true);
    }

    public void CloseShopUI()
    {
        shopPanel.SetActive(false);
    }

    public void BuyItem1()
    {
        if (GameManager.main.currency >= 10)  
        {
            GameManager.main.currency -= 10;
        }
        else
        {
            Debug.Log("Not enough money to buy Item1");
        }
    }
}