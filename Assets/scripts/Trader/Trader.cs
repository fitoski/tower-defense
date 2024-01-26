using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enums;
using TMPro;

public class Trader : MonoBehaviour
{
    public float timeToStay = 10f;
    public string[] inventory;

    private float timer;

    private TraderUIManager traderUIManager;
    private Animator animator;
    public TextMeshProUGUI notificationText;
    private float notificationDuration = 3f;
    private List<ShopItem> currentShopItems = new List<ShopItem>();
    public List<TraderUIElement> shopUIElements = new List<TraderUIElement>();
    public List<Transform> spawnPoints;

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

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        traderUIManager = TraderUIManager.instance;
        timer = timeToStay;
        animator.SetBool("IsIdle", true);

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
        transform.position = spawnPoint.position;

        if (traderUIManager != null)
        {
            traderUIManager.ShowNotification("Trader has arrived!", notificationDuration);
        }
    }

    public ShopItem GetItem(int index)
    {
        if (index >= 0 && index < allItems.Count)
        {
            return allItems[index];
        }
        else
        {
            Debug.LogError("Index out of range for allItems");
            return null;
        }
    }

    IEnumerator EnableTraderAfterIntro(float delay)
    {
        yield return new WaitForSeconds(delay);

        animator.SetBool("IsIdle", true);
    }

    void NotifyPlayer(string message)
    {
        if (notificationText != null)
        {
            notificationText.text = message;
            notificationText.gameObject.SetActive(true);
            StartCoroutine(DisableNotificationAfterTime(notificationDuration));
        }
        else
        {
            Debug.LogError("Notification TextMeshProUGUI is not set in the Inspector.");
        }
    }

    IEnumerator DisableNotificationAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        notificationText.gameObject.SetActive(false);
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            animator.SetTrigger("Disappear");
            Destroy(gameObject, 1f);
        }
    }

    void OnMouseDown()
    {
        OpenShop();
        timer = 0;
    }

    public void OpenShop()
    {
        currentShopItems = GetRandomItems(4);

        for (int i = 0; i < currentShopItems.Count; i++)
        {
            Debug.Log($"Item {i}: {currentShopItems[i].itemName} of type {currentShopItems[i].itemType}");
        }

        if (traderUIManager != null)
        {
            traderUIManager.SetCurrentShopItems(currentShopItems);
            traderUIManager.OpenShopUI();
        }
    }

    //public bool ShouldShowStashButton()
    //{
    //    return Random.value < 0.1f; 
    //}

}