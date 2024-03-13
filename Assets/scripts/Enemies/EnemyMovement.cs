using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private Transform target;
    public float speed = 2f;
    public int baseDamage = 1;
    public float damageMultiplierPerWave = 1.5f;
    private bool isMoving = true;
    public int currentWave = 1;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    public bool canInteractWithCore = true;
    private Enemy enemy;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemy = GetComponent<Enemy>();
        if (enemy == null)
        {
            Debug.LogError("Enemy component not found on the GameObject");
        }
        SetRandomTarget();
    }

    public void Update()
    {
        if (isMoving && target != null && enemy != null && !(enemy is IBoss))
        {
            Vector3 targetPositionWithOffset = target.position;
            Vector2 direction = (targetPositionWithOffset - transform.position).normalized;
            if (direction.x > 0)
            {
                spriteRenderer.flipX = true;
            }
            else if (direction.x < 0)
            {
                spriteRenderer.flipX = false;
            }

            transform.Translate(direction * enemy.speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, target.position) < 0.1f)
            {
                AttackTarget();
            }
        }
    }

    public virtual void SetRandomTarget()
    {
        float chance = Random.Range(0f, 1f);
        if (chance <= 0.7f)
        {
            target = GameObject.FindGameObjectWithTag("Core")?.transform;
        }
        else
        {
            target = GameObject.FindGameObjectWithTag("Player")?.transform;
        }

        if (target == null)
        {
            Debug.LogError("Target not found. Setting default target as Core.");
            target = GameObject.FindGameObjectWithTag("Core")?.transform;
        }
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();
            Enemy enemy = GetComponent<Enemy>();
            if (player != null && enemy != null && !enemy.IsDead)
            {
                int damage = Mathf.CeilToInt(enemy.baseDamage * Mathf.Pow(damageMultiplierPerWave, currentWave - 1));
                player.TakeDamage(damage);
            }
        }
        else if (collision.gameObject.CompareTag("Core") && canInteractWithCore)
        {
            Core core = collision.gameObject.GetComponent<Core>();
            if (core != null)
            {
                core.TakeDamage(baseDamage);
                StopMovement();
                SetAppropriateTrigger("SpecialAnimation", "Die");
            }
        }
    }

    public void AttackTarget()
    {
        Core core = target.GetComponent<Core>();
        if (core != null)
        {
            int damage = Mathf.CeilToInt(baseDamage * Mathf.Pow(damageMultiplierPerWave, currentWave - 1));
            core.TakeDamage(damage);
            StopMovement();
            SetAppropriateTrigger("SpecialAnimation", "Die");
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Core") && canInteractWithCore)
        {
            Core core = collider.gameObject.GetComponent<Core>();
            if (core != null)
            {
                core.TakeDamage(baseDamage);
                StopMovement();
                SetAppropriateTrigger("SpecialAnimation", "Die");
            }
        }
    }

    void SetAppropriateTrigger(string specialTrigger, string defaultTrigger)
    {
        bool hasSpecialTrigger = false;
        foreach (var param in animator.parameters)
        {
            if (param.name == specialTrigger && param.type == AnimatorControllerParameterType.Trigger)
            {
                hasSpecialTrigger = true;
                break;
            }
        }

        if (hasSpecialTrigger)
        {
            animator.SetTrigger(specialTrigger);
        }
        else
        {
            animator.SetTrigger(defaultTrigger);
        }
    }

    public void StopMovement()
    {
        isMoving = false;
    }

    public void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    public void ResetEnemies()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            Destroy(enemy.gameObject);
        }
    }
}