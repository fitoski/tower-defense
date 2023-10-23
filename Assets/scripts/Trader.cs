using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trader : MonoBehaviour
{
    public float timeToStay = 10f;
    public string[] inventory;

    private float timer;

    public TraderUIManager traderUIManager;

    void Awake()
    {
        traderUIManager = TraderUIManager.instance;
    }

    void Start()
    {
        timer = timeToStay;
    }

    // Update is called once per frame
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
        traderUIManager.OpenShopUI();
        Debug.Log("Shop opened");
    }
}