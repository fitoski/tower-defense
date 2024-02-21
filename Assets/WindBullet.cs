using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindBullet : Bullet
{
    [SerializeField] private float knockbackForce = 5f; 

    protected override void HitToEnemy(Enemy enemy, int bulletDamage)
    {
        base.HitToEnemy(enemy, bulletDamage);

        Vector2 knockbackDirection = (enemy.transform.position - transform.position).normalized;
        enemy.ApplyKnockback(knockbackDirection, knockbackForce);

        Debug.Log($"WindBullet: Applying knockback to {enemy.gameObject.name} with force {knockbackForce}");
    }
}