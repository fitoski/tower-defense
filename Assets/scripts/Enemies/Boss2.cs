﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2 : Enemy
{
    public float attackRange = 5f;
    public int attackDamage = 10;
    public float attackCooldown = 5f;
    private float attackTimer;
    private float attack1Timer = 4f;

    private Transform playerTransform;
    private Animator bossAnimator;
    private bool isAttacking = false;

    private float attackForwardDistance = 5f;

    public List<DropItem> droppableItems;

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
        attackTimer = attackCooldown;
    }

    private new void Update()
    {
        if (currentHealth <= 0)
        {
            bossAnimator.SetTrigger("Die");
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if (!isAttacking && distanceToPlayer <= attackRange)
        {
            attack1Timer -= Time.deltaTime;
            if (attack1Timer <= 0)
            {
                AttackType1();
                attack1Timer = 10f;
            }
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
        isAttacking = true;
        attackTimer = attackCooldown;

        int attackType = UnityEngine.Random.Range(0, 2);
        if (attackType == 0)
        {
            AttackType1();
        }
        else
        {
            //AttackType2();
            return;
        }
    }

    void AttackType1()
    {
        Debug.Log("attack type 1");
        bossAnimator.SetTrigger("Attack1");
        isAttacking = true;
        bossAnimator.SetBool("isAttacking", true);
    }

    //void AttackType2()
    //{
    //    Debug.Log("attack type 2");
    //    bossAnimator.SetTrigger("Attack2");
    //}

    void UpdateOrientationTowardsPlayer()
    {
        bool shouldFaceRight = playerTransform.position.x > transform.position.x;
        GetComponent<SpriteRenderer>().flipX = shouldFaceRight;
    }

    public void OnAttackEnd()
    {
        Debug.Log("Attack ended");
        isAttacking = false;
        bossAnimator.SetBool("isAttacking", false);
        AdjustBossPosition();
    }

    private void OnAnimatorMove()
    {
        if (isAttacking)
        {
            transform.position += bossAnimator.deltaPosition;
        }
    }

    public void AdjustBossPosition()
    {
        Vector2 desiredPosition = new Vector2(transform.position.x + attackForwardDistance, transform.position.y);

        transform.position = desiredPosition;
    }

    public override void Die()
    {
        base.Die();
        DropItemOnDeath();
    }

    void DropItemOnDeath()
    {
        Debug.Log("item düştü");
        foreach (DropItem item in droppableItems)
        {
            int randomChance = UnityEngine.Random.Range(0, 100);
            Debug.Log("random chance: " + randomChance + "item drop chance: " + item.dropChance);

            if (randomChance < item.dropChance)
            {
                Debug.Log("dropped item: " + item.itemName);
                Instantiate(item.itemPrefab, transform.position, Quaternion.identity);
                break;
            }
        }
    }
}