using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New BootsItem", menuName = "Inventory/Boots")]
public class BootsItem : ShopItem
{
    public string material; // "Cloth", "Leather", "Plate"
    public float moveSpeedMultiplier;
    public float defenseBonusMultiplier;

    void OnEnable()
    {
        switch (material)
        {
            case "Cloth":
                moveSpeedMultiplier = 1.5f;
                defenseBonusMultiplier = 0; 
                break;
            case "Leather":
                moveSpeedMultiplier = 1; 
                defenseBonusMultiplier = 2.8f;
                break;
            case "Plate":
                moveSpeedMultiplier = 1; 
                defenseBonusMultiplier = 3.4f;
                break;
            default:
                Debug.LogError("Unknown material: " + material);
                break;
        }
    }
}