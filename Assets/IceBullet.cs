using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBullet : Bullet
{
    [SerializeField] private float slowDuration = 5f;
    [SerializeField] private float slowFactor = 0.5f;

    protected override void HitToEnemy(Enemy enemy, int bulletDamage)
    {
        base.HitToEnemy(enemy, bulletDamage);
        enemy.SlowDown(slowFactor, slowDuration);
    }
}