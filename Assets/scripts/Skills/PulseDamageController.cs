using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseDamageController : MonoBehaviour
{
    public int damage = 50;
    public float radius = 3f;
    public int pulseCount = 2;
    private int currentPulse = 0;

    void Update()
    {
        if (currentPulse < pulseCount)
        {
            PulseDamage();
            currentPulse++;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void PulseDamage()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                enemy.GetComponent<Enemy>().TakeDamage(damage);
            }
        }
    }
}
