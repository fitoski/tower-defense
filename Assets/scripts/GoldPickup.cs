using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GoldPickup : MonoBehaviour
{
    //public int goldAmount = 1;
    public TextMeshProUGUI goldText;

    private void Update()
    {
        UpdateGoldDisplay();
    }

    public void UpdateGoldDisplay()
    {
        if (goldText != null)
        {
            goldText.text = GameManager.main.Gold.ToString();
        }
    }

    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        GameManager.main.IncreaseGold(goldAmount);
    //        Destroy(gameObject);
    //    }
    //}
}
