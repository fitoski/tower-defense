using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingEffect : MonoBehaviour
{
    public int damage = 25;

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
        yield return new WaitForSeconds(0.2f); 
        Destroy(gameObject);
    }
}