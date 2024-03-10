using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricBullet : Bullet
{
    public float stunDuration = 1f;
    public float chainStunDuration = 0.5f;
    public int chainAmount = 2;
    public float chainRange = 5f;
    public GameObject chainBulletPrefab;
    public int stunDurationLevel = 0;
    public int chainAmountLevel = 0;
    public int bulletDamageLevel = 0;

    void Start()
    {
        stunDurationLevel = PlayerPrefs.GetInt("ElectricBullet_stunDurationLevel", 0);
        chainAmountLevel = PlayerPrefs.GetInt("ElectricBullet_chainAmountLevel", 0);
        bulletDamageLevel = PlayerPrefs.GetInt("ElectricBullet_BulletDamageLevel", 0);

        stunDuration += stunDurationLevel * 0.1f;
        chainAmount += chainAmountLevel;
        bulletDamage += bulletDamageLevel;
    }

    protected override void HitToEnemy(Enemy enemy, int bulletDamage)
    {
        base.HitToEnemy(enemy, bulletDamage);
        StunEnemy(enemy, stunDuration);
        ChainEffect(enemy);
    }

    protected void StunEnemy(Enemy enemy, float duration)
    {
        if (enemy != null)
        {
            enemy.Stun(duration);
        }
    }

    protected void ChainEffect(Enemy hitEnemy)
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(hitEnemy.transform.position, chainRange);
        int chainCount = 0;

        foreach (var hitCollider in hitColliders)
        {
            if (chainCount >= chainAmount) break;

            Enemy enemy = hitCollider.GetComponent<Enemy>();
            if (enemy != null && enemy != hitEnemy && !enemy.IsDead)
            {
                Vector2 direction = (enemy.transform.position - hitEnemy.transform.position).normalized;
                GameObject chainBulletObject = Instantiate(chainBulletPrefab, hitEnemy.transform.position, Quaternion.identity);
                ChainElectricBullet chainBullet = chainBulletObject.GetComponent<ChainElectricBullet>();
                if (chainBullet != null)
                {
                    chainBullet.Initialize(direction, bulletDamage / 2, chainStunDuration);
                }
                chainCount++;
            }
        }
    }

    public override void SetTarget(Transform _target, Vector2 direction)
    {
        base.SetTarget(_target, direction);
        rb.velocity = direction * bulletSpeed;
    }
}