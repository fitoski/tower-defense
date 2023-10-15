using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private Transform target;
    public float speed = 2f;
    public int baseDamage = 1;
    public float damageMultiplierPerWave = 1.5f;

    public int currentWave = 1;

    public GameObject goldPrefab;

    //[SerializeField] private int currencyWorth = 50;

    void Update()
    {
        if (target != null)
        {
            Vector2 direction = (target.position - transform.position).normalized;
            transform.Translate(direction * speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, target.position) < 0.1f)
            {
                AttackTarget();
            }
        }
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    void AttackTarget()
    {
        Core core = target.GetComponent<Core>();
        if (core != null)
        {
            int damage = Mathf.CeilToInt(baseDamage * Mathf.Pow(damageMultiplierPerWave, currentWave - 1));
            core.TakeDamage(damage);
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //GameManager.main.IncreaseCurrency(currencyWorth);
            Destroy(gameObject);
        }
    }

    public void Die()
    {
        if (goldPrefab != null)
        {
            Instantiate(goldPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }


}