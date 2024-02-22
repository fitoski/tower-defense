using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackEffect : MonoBehaviour
{
    public int damage = 20;
    public float activeTime = 0.5f;

    private void Start()
    {
        StartCoroutine(DeactivateAfterTime());
    }

    private IEnumerator DeactivateAfterTime()
    {
        yield return new WaitForSeconds(activeTime);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var player = collision.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
    }
}