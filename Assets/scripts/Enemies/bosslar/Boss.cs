﻿using System.Collections;
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
    public float secondAttackCooldown = 10f; 
    private float secondAttackTimer;
    public Collider2D attackEffectCollider1;
    public Collider2D attackEffectCollider2;
    public GameObject groundSmashEffectPrefab;
    public GameObject landingEffectPrefab;
    public Vector3 effectSpawnOffset;

    protected override void Start()
    {
        base.Start();
        maxHealth = 5000;
        speed = 2f;
        baseDamage = 1;
        scoreValue = 15;
        experiencePointsValue = 10;
        damageMultiplierPerWave = 1.5f;
        currentHealth = maxHealth;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        bossAnimator = GetComponent<Animator>();
        attackTimer = 0;
        secondAttackTimer = secondAttackCooldown;
        effectSpawnOffset = new Vector3(5f, -1.5f, 0f);
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

        if (secondAttackTimer <= 0)
        {
            PerformSecondAttack();
            secondAttackTimer = secondAttackCooldown;
        }
        else
        {
            secondAttackTimer -= Time.deltaTime;
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

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.TakeDamage(attackDamage);
            }
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

    public void TriggerGroundSmashEffect()
    {
        Vector3 adjustedEffectSpawnOffset = effectSpawnOffset;
        if (playerTransform.position.x < transform.position.x)
        {
            adjustedEffectSpawnOffset.x = -effectSpawnOffset.x;
        }

        Vector3 effectSpawnPosition = transform.position + adjustedEffectSpawnOffset;
        GameObject effectInstance = Instantiate(groundSmashEffectPrefab, effectSpawnPosition, Quaternion.identity);

        if (playerTransform.position.x < transform.position.x)
        {
            effectInstance.transform.localScale = new Vector3(1, 1, 1); 
        }
        else
        {
            effectInstance.transform.localScale = new Vector3(-1, 1, 1); 
        }
    }

    void PerformSecondAttack()
    {
        if (!isAttacking)
        {
            Debug.Log("Boss performs the second attack");
            Vector2 targetPosition = playerTransform.position;
            isAttacking = true;  
            StartCoroutine(JumpToPosition(targetPosition));
        }
    }

    IEnumerator JumpToPosition(Vector2 targetPosition)
    {
        bossAnimator.SetTrigger("Jump");
        yield return new WaitForSeconds(0.15f);
        float jumpDuration = 1.5f;
        float elapsedTime = 0;
        Vector2 startPosition = transform.position;
        Vector2 peakPosition = new Vector2(startPosition.x, startPosition.y + 10f);
        while (elapsedTime < jumpDuration)
        {
            transform.position = Vector2.Lerp(startPosition, peakPosition, elapsedTime / jumpDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(2f);
        elapsedTime = 0;
        startPosition = transform.position;
        while (elapsedTime < jumpDuration)
        {
            transform.position = Vector2.Lerp(startPosition, targetPosition, elapsedTime / jumpDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        bossAnimator.SetTrigger("Land");

        TriggerLandingEffect();

        yield return new WaitForSeconds(0.8f);
        isAttacking = false;
    }

    public void TriggerLandingEffect()
    {
        Vector3 effectOffset = new Vector3(0f, -3f, 0f);

        Vector3 effectSpawnPosition = transform.position + effectOffset;

        Instantiate(landingEffectPrefab, effectSpawnPosition, Quaternion.identity);
    }

    public void JumpUp()
    {
        float jumpHeight = 10f; 
        transform.position = new Vector2(transform.position.x, transform.position.y + jumpHeight);
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

    public new void Die()
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