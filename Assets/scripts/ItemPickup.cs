using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public int weaponDamage;
    public float weaponRange;
    public int weaponSpriteIndex;
    public RuntimeAnimatorController weaponAnimator; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement player = other.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.ChangeWeapon(weaponDamage, weaponRange, weaponSpriteIndex, weaponAnimator);
                Destroy(gameObject);
            }
        }
    }
}
