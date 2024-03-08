using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    protected Rigidbody2D rb;
    private Vector2 lastKnownDirection;

    [Header("Attributes")]
    [SerializeField] protected float bulletSpeed = 5f;
    [SerializeField] protected int bulletDamage = 10;

    public int BulletDamage
    {
        get => bulletDamage;
        set => bulletDamage = value;
    }
    public float BulletSpeed
    {
        get => bulletSpeed;
        set => bulletSpeed = value;
    }

    private Transform target;
    private bool targetHit = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Invoke("DestroyBullet", 5f);
    }

    public virtual void SetTarget(Transform _target, Vector2 direction)
    {
        target = _target;
        rb.velocity = direction * bulletSpeed;
    }

    private void Update()
    {
        if (target == null && !targetHit)
        {
            Invoke("DestroyBullet", 5f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null && !enemy.IsDead)
            {
                HitToEnemy(enemy, bulletDamage);
                targetHit = true;
                CancelInvoke("DestroyBullet");
                Destroy(gameObject);
            }
        }
    }

    protected virtual void HitToEnemy(Enemy enemy, int bulletDamage)
    {
        enemy.TakeDamage(bulletDamage);
    }

    private void DestroyBullet()
    {
        Destroy(gameObject);
    }
}