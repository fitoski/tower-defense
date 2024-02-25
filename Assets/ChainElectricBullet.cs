using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ChainElectricBullet : ElectricBullet
{
    public void Initialize(Vector2 direction, int damage, float stunDuration)
    {
        this.bulletDamage = damage;
        this.stunDuration = stunDuration;
        rb.velocity = direction * bulletSpeed;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 180f;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        Destroy(gameObject, 5f);
    }

    //public override void SetTarget(Transform _target, Vector2 direction)
    //{
    //    base.SetTarget(_target, direction);
    //    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    //    transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    //    rb.velocity = direction * bulletSpeed;
    //}

    protected override void HitToEnemy(Enemy enemy, int bulletDamage)
    {
        base.HitToEnemy(enemy, bulletDamage);
        Destroy(gameObject);
    }
}