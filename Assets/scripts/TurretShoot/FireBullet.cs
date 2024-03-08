using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBullet : Bullet
{
    [SerializeField] private int initialDamage = 5;
    [SerializeField] private int burnDamage = 2;
    [SerializeField] private float burningInterval = 2f;
    [SerializeField] private float burnTime = 5f;
    public int burnTimeLevel = 0;
    public int bulletDamageLevel = 0;

    void Start()
    {
        burnTimeLevel = PlayerPrefs.GetInt("FireBullet_burnTimeLevel", 0);
        bulletDamageLevel = PlayerPrefs.GetInt("FireBullet_BulletDamageLevel", 0);

        burnTime += burnTimeLevel * 1f;
        bulletDamage += bulletDamageLevel;
    }

    protected override void HitToEnemy(Enemy enemy, int bulletDamage)
    {
        Debug.Log($"FireBullet: Applying initial damage of {initialDamage} to {enemy.gameObject.name}");
        enemy.TakeDamage(initialDamage);
        Debug.Log($"FireBullet: Starting burning effect on {enemy.gameObject.name} with {burnDamage} damage every {burningInterval} seconds for {burnTime} seconds.");
        enemy.StartBurning(burnDamage, burningInterval, burnTime);
    }
}