using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEnemyDamageController : MonoBehaviour
{
    public int damage = 30;
    public float interval = 15f;
    private float lastAttackTime = -999f;

    void Update()
    {
        if (Time.time >= lastAttackTime + interval)
        {
            lastAttackTime = Time.time;
            AttackRandomEnemies();
        }
    }

    void AttackRandomEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length > 0)
        {
            for (int i = 0; i < 2; i++)
            {
                GameObject enemy = enemies[UnityEngine.Random.Range(0, enemies.Length)];
                if (enemy != null)
                {
                    enemy.GetComponent<Enemy>().TakeDamage(damage);
                }
            }
        }
    }
}