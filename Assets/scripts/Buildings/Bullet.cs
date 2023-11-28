using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb;

    [Header("Attributes")]
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] public int bulletDamage = 10;

    private Transform target;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void SetTarget(Transform _target)
    {
        target = _target;
        Vector2 direction = (target.position - transform.position);
        rb.velocity = direction.normalized * bulletSpeed;
    }

    private void FixedUpdate()
    {
        if (!target)
        {
            Destroy(gameObject);
            return;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            if (!enemy.IsDead)
            {
                HitToEnemy(enemy, bulletDamage);
            }
            Destroy(gameObject);
        }
    }

    protected virtual void HitToEnemy(Enemy enemy, int bulletDamage)
    {
        enemy.TakeDamage(bulletDamage);
    }
}