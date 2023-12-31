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
    //public GameObject goldPrefab;
    private SpriteRenderer spriteRenderer;
    public bool canInteractWithCore = true;
    //[SerializeField] private int currencyWorth = 50;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Update()
    {
        if (isMoving && target != null)
        {
            Vector2 direction = (target.position - transform.position).normalized;

            if (direction.x > 0)  
            {
                spriteRenderer.flipX = true;  
            }
            else if (direction.x < 0)  
            {
                spriteRenderer.flipX = false;  
            }

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

    //public void AttackTarget()
    //{
    //    Core core = target.GetComponent<Core>();
    //    if (core != null)
    //    {
    //        int damage = Mathf.CeilToInt(baseDamage * Mathf.Pow(damageMultiplierPerWave, currentWave - 1));
    //        core.TakeDamage(damage);

    //        StopMovement();
    //        animator.SetTrigger("SpecialAnimation"); 
    //    }
    //}

    //void OnTriggerEnter2D(Collider2D collider)
    //{
    //    if (collider.gameObject.CompareTag("Core"))
    //    {
    //        StopMovement(); 
    //        animator.SetTrigger("SpecialAnimation"); 
    //    }
    //}

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();
            Enemy enemy = GetComponent<Enemy>();
            if (player != null && enemy != null && !enemy.IsDead) 
            {
                player.TakeDamage(baseDamage);
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
            StopMovement();
            SetAppropriateTrigger("SpecialAnimation", "Die");
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
}