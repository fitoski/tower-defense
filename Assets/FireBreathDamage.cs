using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBreathDamage : MonoBehaviour
{
    public int damage = 10;
    public float duration = 0.9f;
    public float damageInterval = 0.1f;
    private float lastDamageTime = -Mathf.Infinity;

    void Start()
    {
        Destroy(gameObject, duration);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && Time.time - lastDamageTime >= damageInterval)
        {
            other.GetComponent<PlayerMovement>()?.TakeDamage(damage);
            lastDamageTime = Time.time;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}