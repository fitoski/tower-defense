using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New WeaponItem", menuName = "Inventory/Weapons")]
public class WeaponItem : ShopItem
{
    public string material;
    public float moveSpeedMultiplier;
    public float defenseBonusMultiplier;
    public RuntimeAnimatorController weaponAnimator;

    void OnEnable()
    {
        switch (material)
        {
            case "Steel":
                moveSpeedMultiplier = 1.05f;
                defenseBonusMultiplier = 1.10f;
                break;
            case "Iron":
                moveSpeedMultiplier = 1.00f;
                defenseBonusMultiplier = 1.05f;
                break;
            default:
                Debug.LogWarning("Unknown material: " + material);
                break;
        }
    }
}