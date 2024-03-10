using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBullet : Bullet
{
    [SerializeField] private float slowDuration = 5f;
    [SerializeField] private float slowFactor = 0.5f;
    public int slowFactorLevel = 0;
    public int bulletDamageLevel = 0;

    void Start()
    {
        slowFactorLevel = PlayerPrefs.GetInt("IceBullet_slowFactorLevel", 0);
        slowFactor += slowFactorLevel * 0.05f;

        bulletDamageLevel = PlayerPrefs.GetInt("IceBullet_BulletDamageLevel", 0);
        bulletDamage += bulletDamageLevel;
    }

    protected override void HitToEnemy(Enemy enemy, int bulletDamage)
    {
        base.HitToEnemy(enemy, bulletDamage);
        enemy.SlowDown(slowFactor, slowDuration);
    }
}