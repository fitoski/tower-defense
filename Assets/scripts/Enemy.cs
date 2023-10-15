using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Attributes")]
    public int maxHealth = 10;
    public int baseDamage = 1;
    public float damageMultiplierPerWave = 1.5f;

    private int currentHealth;

    private EnemyMovement enemyMovement;

    public int scoreValue = 10;
    public int experiencePointsValue = 5;

    void Start()
    {
        currentHealth = maxHealth;
        enemyMovement = GetComponent<EnemyMovement>();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    public int GetScoreValue()
    {
        return scoreValue;
    }

    void Die()
    {
        if (enemyMovement != null)
        {
            enemyMovement.Die();
        }
        GameManager.main.IncreaseExperiencePoints(experiencePointsValue);
    }
}
