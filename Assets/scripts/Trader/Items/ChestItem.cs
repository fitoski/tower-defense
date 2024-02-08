using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ChestplateItem", menuName = "Inventory/Chestplate")]
public class ChestItem : ShopItem
{
    public string material; // "Cloth", "Leather", "Plate"

    public float defenseMultiplier;
    public float moveSpeedMultiplier;

    void OnEnable()
    {
        switch (material)
        {
            case "Cloth":
                moveSpeedMultiplier = 2.1f;
                defenseMultiplier = 0; 
                break;
            case "Leather":
                moveSpeedMultiplier = 1; 
                defenseMultiplier = 2.4f;
                break;
            case "Plate":
                moveSpeedMultiplier = 1; 
                defenseMultiplier = 3f;
                break;
            default:
                Debug.LogError("Unknown material: " + material);
                moveSpeedMultiplier = 1;
                defenseMultiplier = 1;
                break;
        }
    }
}