using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack3Effect : MonoBehaviour
{
    public int damage = 20;
    public float stunDuration = 1.0f;

    public void TriggerEffect()
    {
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(transform.position, 3f);

        foreach (var hitPlayer in hitPlayers)
        {
            if (hitPlayer.tag == "Player")
            {
                var player = hitPlayer.GetComponent<PlayerMovement>();
                if (player != null)
                {
                    player.TakeDamage(damage);
                    player.Stun(stunDuration);
                }
            }
        }
        Destroy(gameObject);
    }

    public void DestroyEffect()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 3f);
    }
}