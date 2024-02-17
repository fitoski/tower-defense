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

        }
    }
}