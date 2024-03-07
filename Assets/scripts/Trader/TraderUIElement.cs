using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TraderUIElement : MonoBehaviour
{
    public Image itemIcon;
    public TMP_Text itemNameText;
    public TMP_Text itemPriceText;
    public TMP_Text itemDescriptionText;

    public void UpdateUI(ShopItem shopItem)
    {
        itemIcon.sprite = shopItem.itemIcon;
        itemNameText.text = shopItem.itemName;
        itemPriceText.text = $"{shopItem.price} Coins";
        itemDescriptionText.text = shopItem.description;
    }
}