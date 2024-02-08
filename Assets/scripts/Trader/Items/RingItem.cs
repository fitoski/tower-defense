using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New RingItem", menuName = "Inventory/Ring")]
public class RingItem : ShopItem
{
    public string material; // "Silver", "Gold", "Diamond"

    public float rangeMultiplier;
    public float damageMultiplier;

    void OnEnable()
    {
        switch (material)
        {
            case "Silver":
                rangeMultiplier = 1.05f; 
                damageMultiplier = 1; 
                break;
            case "Gold":
                rangeMultiplier = 1.05f; 
                damageMultiplier = 1.1f; 
                break;
            case "Diamond":
                rangeMultiplier = 1.05f; 
                damageMultiplier = 1.1f; 
                break;
            default:
                Debug.LogError("Unknown material: " + material);
                rangeMultiplier = 1;
                damageMultiplier = 1;
                break;
        }
    }
}