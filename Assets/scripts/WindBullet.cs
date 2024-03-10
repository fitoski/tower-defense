using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindBullet : Bullet
{
    [SerializeField] private float knockbackForce = 5f;
    public int knockbackForceLevel = 0;
    public int bulletDamageLevel = 0;

    void Start()
    {
        knockbackForceLevel = PlayerPrefs.GetInt("WindBullet_knockbackForceLevel", 0);
        knockbackForce += knockbackForceLevel * 1f;

        bulletDamageLevel = PlayerPrefs.GetInt("WindBullet_BulletDamageLevel", 0);
        bulletDamage += bulletDamageLevel;
    }

    protected override void HitToEnemy(Enemy enemy, int bulletDamage)
    {
        base.HitToEnemy(enemy, bulletDamage);
        Vector2 knockbackDirection = (enemy.transform.position - transform.position).normalized;
        enemy.ApplyKnockback(knockbackDirection, knockbackForce);
    }
}