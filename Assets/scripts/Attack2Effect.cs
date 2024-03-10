using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack2Effect : MonoBehaviour
{
    public int damage = 10;
    private float damageDelay;

    public void InitializeDamage(int damageAmount, float delay)
    {
        damage = damageAmount;
        damageDelay = delay;
        StartCoroutine(EnableDamage());
    }

    IEnumerator EnableDamage()
    {
        yield return new WaitForSeconds(damageDelay);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            collider.GetComponent<PlayerMovement>().TakeDamage(damage);
        }
    }
}