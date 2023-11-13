using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableProjectileControl : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 firstPos = Vector2.zero;
    private int damage;
    private float speed;
    public float distance;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void InitObject(float distance, float speed, int damage)
    {
        this.distance = distance;
        this.speed = speed;
        this.damage = damage;
    }
    void Start()
    {
        Vector2? target = findTargetPosition();

        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        firstPos = transform.position;
        Vector2 dir = ((Vector2) target - rb.position).normalized;

        rb.velocity = dir * speed;
    }

    private void Update()
    {
        if (Vector2.Distance(rb.position, firstPos) > distance)
        {
            Destroy(gameObject);
        }
    }

    private Vector2? findTargetPosition()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject cloesetEnemy = null;
        float minDistance = Mathf.Infinity;
        Vector2 currentPosition = transform.position;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(enemy.transform.position, currentPosition);
            if (distance < minDistance)
            {
                cloesetEnemy = enemy;
                minDistance = distance;
            }
        }

        if (cloesetEnemy != null)
        {
            return cloesetEnemy.transform.position;
        }

        return null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
            collision.GetComponent<Enemy>().TakeDamage(damage);
        }
    }
}
