using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New AmuletItem", menuName = "Inventory/Amulet")]
public class AmuletItem : ShopItem
{
    public string material; // "Topaz", "Ruby", "Sapphire"

    public float attackCooldownReductionFactor = 0.9f; 
    public float healthRegenerationBonus = 0f; 
    public float maxHealthMultiplier = 1f; 

    void OnEnable()
    {
        switch (material)
        {
            case "Topaz":
                attackCooldownReductionFactor = 0.9f;
                healthRegenerationBonus = 0f;
                maxHealthMultiplier = 1f;
                break;
            case "Ruby":
                attackCooldownReductionFactor = 0.9f;
                healthRegenerationBonus = 0.01f; 
                maxHealthMultiplier = 1f;
                break;
            case "Sapphire":
                attackCooldownReductionFactor = 0.9f;
                healthRegenerationBonus = 0.01f; 
                maxHealthMultiplier = 1.1f; 
                break;
            default:
                Debug.LogError("Unknown material: " + material);
                break;
        }
    }
}
