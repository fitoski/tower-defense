using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New BootsItem", menuName = "Inventory/Boots")]
public class BootsItem : ShopItem
{
    public float bootsProtection;
    public string material; // "Cloth", "Leather", "Plate"
    // Diğer özel özellikler ekleyebilirsiniz.
}
