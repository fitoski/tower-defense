using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineScript : MonoBehaviour
{
    public int damage = 20;
    private Animator animator;
    private bool hasExploded = false;
    private bool hasDamaged = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasExploded)
        {
            hasExploded = true;
            Explode();
        }
    }

    void Explode()
    {
        animator.SetTrigger("Explode");
    }

    public void DamagePlayer()
    {
        if (!hasDamaged)
        {
            Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(transform.position, 0.5f);
            foreach (Collider2D player in hitPlayers)
            {
                if (player.CompareTag("Player"))
                {
                    player.GetComponent<PlayerMovement>().TakeDamage(damage);
                    hasDamaged = true;
                }
            }
        }
    }

    public void DestroyMine()
    {
        Destroy(gameObject);
    }
}