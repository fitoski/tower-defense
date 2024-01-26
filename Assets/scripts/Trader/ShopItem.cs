using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShopItem : ScriptableObject
{
    public string itemName;
    public int price;
    public Sprite itemIcon;
    public Enums.ItemType itemType;
    public float itemDamageBonus;
    public float itemRangeBonus;
    public float armorBonus;
}
