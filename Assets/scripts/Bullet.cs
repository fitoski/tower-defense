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
        if (!target) return;

        Vector2 direction = (target.position - transform.position);

        rb.velocity = direction * bulletSpeed;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject != null)
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(bulletDamage);
            }
        }

        Destroy(gameObject);
    }

}
