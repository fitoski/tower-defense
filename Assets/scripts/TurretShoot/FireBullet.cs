using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBullet : Bullet
{
    [SerializeField] private float burnPercent = 0.5f;
    [SerializeField] private int burnDamage = 5;
    [SerializeField] private float burningInterval = 0.5f;
    [SerializeField] private float burnTime = 5f;
    protected override void HitToEnemy(Enemy enemy, int bulletDamage)
    {
        base.HitToEnemy(enemy, bulletDamage);

        if (Random.value < burnPercent)
        {
            enemy.StartBurning(burnDamage, burningInterval, burnTime);
        }
    }
}
