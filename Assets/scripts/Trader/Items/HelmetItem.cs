using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Helmet", menuName = "Inventory/Helmet")]
public class HelmetItem : ShopItem
{
    public float headProtection;
    public string material; // "Cloth", "Leather", "Plate"
    // Diğer özel özellikler
}
