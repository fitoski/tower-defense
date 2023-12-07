using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    public float attackRange = 5f;
    public int attackDamage = 10;
    public float attackCooldown = 5f;
    private float attackTimer;

    private Transform playerTransform;
    private Animator bossAnimator;
    private bool isAttacking = false;
    public GameObject chestPrefab; 

    private new void Start()
    {
        base.Start();
        maxHealth = 10;
        speed = 2f;
        baseDamage = 1;
        scoreValue = 15;
        experiencePointsValue = 10;
        damageMultiplierPerWave = 1.5f;
        currentHealth = maxHealth;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        bossAnimator = GetComponent<Animator>();
        attackTimer = 0;
    }
       
    private new void Update()
    {
        if (currentHealth <= 0)
        {
            bossAnimator.SetTrigger("Die");
            return; 
        }

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if (!isAttacking && distanceToPlayer <= attackRange && attackTimer <= 0)
        {
            AttackPlayer();
        }
        else if (!isAttacking)
        {
            FollowPlayer();
        }

        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }
    }

    void FollowPlayer()
    {
        if (!isAttacking)
        {
            UpdateOrientationTowardsPlayer();
            transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, speed * Time.deltaTime);
        }
    }

    void AttackPlayer()
    {
        UpdateOrientationTowardsPlayer();
        Debug.Log("Boss is attacking");
        bossAnimator.SetBool("isAttacking", true);
        isAttacking = true;
        attackTimer = attackCooldown;
    }

    void UpdateOrientationTowardsPlayer()
    {
        bool shouldFaceRight = playerTransform.position.x > transform.position.x;
        GetComponent<SpriteRenderer>().flipX = shouldFaceRight;
    }

    public void OnAttackEnd()
    {
        Debug.Log("Attack ended");
        bossAnimator.SetBool("isAttacking", false);
        isAttacking = false;
    }

    public override void Die()
    {
        base.Die();
        SpawnChest();
    }

    void SpawnChest()
    {
        Instantiate(chestPrefab, transform.position, Quaternion.identity);
    }

    void DropSkillOnDeath()
    {
        Debug.Log("skill item düştü");
        SkillsManager.Instance.PrepareSkillRewardPanel();
    }
}