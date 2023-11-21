using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Attributes")]
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] public int bulletDamage = 10;

    private Transform target;


    void Start()
    {

    }

    public void SetTarget(Transform _target)
    {
        target = _target;
    }

    private void FixedUpdate()
    {
        if (!target)
        {
            Destroy(gameObject);
            return;
        }

        Vector2 direction = (target.position - transform.position);

        rb.velocity = direction.normalized * bulletSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            if (!enemy.IsDead)
            {
                enemy.TakeDamage(bulletDamage);
            }
            Destroy(gameObject);
        }
    }
}