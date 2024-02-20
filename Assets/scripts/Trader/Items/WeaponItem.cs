using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New WeaponItem", menuName = "Inventory/Weapons")]
public class WeaponItem : ShopItem
{
    public string weaponMaterial;
    public RuntimeAnimatorController weaponAnimator;
    public float customOrbitRadius;

    void OnEnable()
    {
        switch (weaponMaterial)
        {
            case "weapon1":
                itemDamageBonus = 100;
                customOrbitRadius = 2.0f;
                break;
            case "weapon2":
                itemDamageBonus = 100;
                customOrbitRadius = 2.0f;
                break;
            case "weapon3":
                itemDamageBonus = 100;
                customOrbitRadius = 2.0f;
                break;
            case "weapon4":
                itemDamageBonus = 100;
                customOrbitRadius = 2.0f;
                break;
            case "weapon5":
                itemDamageBonus = 100;
                customOrbitRadius = 2.0f;
                break;
            case "weapon6":
                itemDamageBonus = 100;
                customOrbitRadius = 2.0f;
                break;
            case "weapon7":
                itemDamageBonus = 100;
                customOrbitRadius = 2.0f;
                break;
            case "weapon8":
                itemDamageBonus = 100;
                customOrbitRadius = 2.0f;
                break;
            case "weapon9":
                itemDamageBonus = 100;
                customOrbitRadius = 2.0f;
                break;
            case "weapon10":
                itemDamageBonus = 100;
                customOrbitRadius = 2.0f;
                break;
            default:
                Debug.LogWarning("Unknown material: " + weaponMaterial);
                break;
        }
    }
}