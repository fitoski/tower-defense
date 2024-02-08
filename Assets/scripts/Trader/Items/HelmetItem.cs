using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Helmet", menuName = "Inventory/Helmet")]
public class HelmetItem : ShopItem
{
    public string material; // "Cloth", "Leather", "Plate"

    public float moveSpeedMultiplier;
    public float attackCooldownMultiplier;
    public float defenseMultiplier;

    void OnEnable()
    {
        switch (material)
        {
            case "Cloth":
                moveSpeedMultiplier = 1.1f;
                attackCooldownMultiplier = 0.9f;
                defenseMultiplier = 0; 
                break;
            case "Leather":
                moveSpeedMultiplier = 1; 
                defenseMultiplier = 1.2f;
                attackCooldownMultiplier = 1; 
                break;
            case "Plate":
                moveSpeedMultiplier = 1; 
                defenseMultiplier = 1.5f;
                attackCooldownMultiplier = 1; 
                break;
            default:
                Debug.LogError("Unknown material: " + material);
                break;
        }
    }
}