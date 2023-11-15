using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Attributes")]
    public int maxHealth = 10;
    public int baseDamage = 1;
    public float damageMultiplierPerWave = 1.5f;
    protected int currentHealth;
    protected EnemyMovement enemyMovement;
    public float speed;
    public int scoreValue = 10;
    public int experiencePointsValue = 5;
    private Vector2 previousPosition;
    private float movementThreshold = 0.01f;
    private Animator animator;

    protected void Start()
    {
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        enemyMovement = GetComponent<EnemyMovement>();
        previousPosition = transform.position;

    }

    protected virtual void Update()
    {
        Vector2 currentPosition = transform.position;
        bool isEnemyMoving = Vector2.Distance(previousPosition, currentPosition) > movementThreshold;
        animator.SetBool("isMoving", isEnemyMoving);
        previousPosition = currentPosition;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            PlayerMovement player = collider.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.TakeDamage(baseDamage);
            }
        }
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

    public virtual void Die()
    {
        animator.SetTrigger("Die");
        animator.SetBool("isDead", true);

        if (enemyMovement != null)
        {
            enemyMovement.StopMovement();
        }
        GameManager.main.IncreaseExperiencePoints(experiencePointsValue);
        EnemySpawner.Instance.ActiveEnemies--;
        EnemySpawner.Instance.EnemyKilled();
    }

    public void OnDeathAnimationComplete()
    {
        Destroy(gameObject);
    }
}