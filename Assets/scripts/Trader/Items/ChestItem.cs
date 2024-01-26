using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ChesplateItem", menuName = "Inventory/Chestplate")]
public class ChestItem : ShopItem
{
    public float chestProtection;
    public string material; // "Cloth", "Leather", "Plate"
    // Diğer özel özellikler ekleyebilirsiniz.
}
