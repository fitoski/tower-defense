using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tentacle : MonoBehaviour
{
    public int damage = 100;
    public float damageDelay = 0.4f;
    public float lifetime = 0.5f;
    public float damageRadius = 5f;

    private void Start()
    {
        Invoke("DealDamage", damageDelay);
        Destroy(gameObject, lifetime);
    }

    void DealDamage()
    {
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(transform.position, damageRadius, LayerMask.GetMask("Player"));

        foreach (var hitPlayer in hitPlayers)
        {
            var player = hitPlayer.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 4f);
    }
}