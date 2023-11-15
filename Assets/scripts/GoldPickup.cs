using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GoldPickup : MonoBehaviour
{
    public int goldAmount = 10;
    public TextMeshProUGUI currencyText;

    private void Update()
    {
        UpdateCurrencyDisplay();
    }

    public void UpdateCurrencyDisplay()
    {
        if (currencyText != null)
        {
            currencyText.text = GameManager.main.currency.ToString();
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