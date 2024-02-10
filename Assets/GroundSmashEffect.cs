using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSmashEffect : MonoBehaviour
{
    public int damage = 15;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerMovement>().TakeDamage(damage);
        }
    }

    IEnumerator Start()
    {
        Animator effectAnimator = GetComponent<Animator>();
        yield return new WaitForSeconds(0.3f);
        Destroy(gameObject); 
    }
}