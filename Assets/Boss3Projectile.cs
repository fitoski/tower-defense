using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 10;
    public float lifetime = 5f;
    private Vector2 moveDirection;

    public void SetMoveDirection(Vector2 dir)
    {
        moveDirection = dir.normalized;
        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public void Initialize(Vector2 targetPosition)
    {
        Destroy(gameObject, lifetime);

        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 180f;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        Rigidbody2D rb = gameObject.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.velocity = direction * speed;
    }

    void Update()
    {
        transform.Translate(moveDirection * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.gameObject.CompareTag("Player"))
        {
            PlayerMovement player = hitInfo.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}