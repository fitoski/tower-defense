using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TraderUIElement : MonoBehaviour
{
    public Image itemIcon;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemPriceText;

    public void UpdateUI(ShopItem shopItem)
    {
        itemIcon.sprite = shopItem.itemIcon;
        itemNameText.text = shopItem.itemName;
        itemPriceText.text = shopItem.price.ToString() + " Coins";
    }
}