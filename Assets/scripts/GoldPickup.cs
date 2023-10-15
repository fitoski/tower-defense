using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoldPickup : MonoBehaviour
{
    public int goldAmount = 10;
    public Text currencyText;

    private void Update()
    {
        UpdateCurrencyDisplay();
    }

    public void UpdateCurrencyDisplay()
    {
        if (currencyText != null)
        {
            currencyText.text = "Currency: " + GameManager.main.currency.ToString();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.main.IncreaseCurrency(goldAmount);
            Destroy(gameObject);
        }
    }
}