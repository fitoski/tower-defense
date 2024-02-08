using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New GlovesItem", menuName = "Inventory/Gloves")]
public class GlovesItem : ShopItem
{
    public string material; // "Cloth", "Leather", "Plate"

    public float attackSpeedMultiplier;
    public float defenseBonusMultiplier;

    void OnEnable()
    {
        switch (material)
        {
            case "Cloth":
                attackSpeedMultiplier = 0.8f; 
                defenseBonusMultiplier = 1; 
                break;
            case "Leather":
                attackSpeedMultiplier = 1; 
                defenseBonusMultiplier = 1.1f;
                break;
            case "Plate":
                attackSpeedMultiplier = 1; 
                defenseBonusMultiplier = 1.4f;
                break;
            default:
                Debug.LogError("Unknown material: " + material);
                break;
        }
    }
}