using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss4MeleeDamageZone : MonoBehaviour
{
    public int damage = 20;
    public float duration = 0.5f;

    void Start()
    {
        Destroy(gameObject, duration);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var player = collision.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
    }
}