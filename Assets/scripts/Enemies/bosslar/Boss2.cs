using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2 : Enemy, IBoss
{
    public float attackRange = 5f;
    public int attackDamage = 10;
    public float attackCooldown = 5f;
    private float attackTimer;
    private Transform playerTransform;
    private Animator bossAnimator;
    private bool isAttacking = false;
    public GameObject chestPrefab;
    private SpriteRenderer spriteRenderer;
    private Collider2D bossCollider;
    private float attackForwardDistance = 5f;
    public GameObject attackEffectPrefab;
    public Vector3 attackEffectOffset;
    private float attackType1Cooldown = 10f;
    private float attackType1Timer = 0f;
    private Vector2 strikeTargetPosition;

    protected override void Start()
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
        spriteRenderer = GetComponent<SpriteRenderer>();
        bossCollider = GetComponent<Collider2D>();

        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer component not found on " + gameObject.name);
        }

        if (bossCollider == null)
        {
            Debug.LogError("Collider2D component not found on " + gameObject.name);
        }
    }

    private void FlipCharacterDirection()
    {
        spriteRenderer.flipX = !spriteRenderer.flipX;

        var offset = bossCollider.offset;
        bossCollider.offset = new Vector2(-offset.x, offset.y);
    }

    private new void Update()
    {
        if (currentHealth <= 0)
        {
            bossAnimator.SetTrigger("Die");
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if (attackType1Timer > 0)
        {
            attackType1Timer -= Time.deltaTime;
        }

        if (!isAttacking && distanceToPlayer > attackRange && attackType1Timer <= 0)
        {
            AttackType1();
            attackType1Timer = attackType1Cooldown;
        }
        else if (!isAttacking && distanceToPlayer <= attackRange && attackTimer <= 0)
        {
            AttackType2();
            attackTimer = attackCooldown;
        }

        FollowPlayer();

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

    public void AttackPlayer()
    {
        //UpdateOrientationTowardsPlayer();
        //float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        //if (distanceToPlayer > attackRange && !isAttacking)
        //{
        //    AttackType1();
        //}
        //else if (distanceToPlayer <= attackRange && attackTimer <= 0 && !isAttacking)
        //{
        //    AttackType2();
        //    attackTimer = 5f;
        //}
    }

    void AttackType1()
    {
        Debug.Log("Performing Attack Type 1");
        UpdateOrientationTowardsPlayer();
        bossAnimator.SetTrigger("Attack1Leap");
        isAttacking = true;
        bossAnimator.SetBool("isAttacking", true);
        StartCoroutine(WaitForLeapAnimation());
    }

    IEnumerator WaitForLeapAnimation()
    {
        yield return new WaitForSeconds(0.6f);
        PerformLeap();
        yield return new WaitForSeconds(0.25f);
        strikeTargetPosition = playerTransform.position;
        yield return new WaitForSeconds(0.25f);
        PerformStrike();
    }

    IEnumerator WaitForStrikeAnimation()
    {
        yield return new WaitForSeconds(0.6f);

        transform.position = playerTransform.position;

        PerformStrike();
        OnAttackEnd();
    }

    public void PerformLeap()
    {
        StartCoroutine(LeapToHeight(2f, 0.5f));
    }

    IEnumerator LeapToHeight(float height, float duration)
    {
        float startY = transform.position.y;
        float endY = startY + height;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            float newY = Mathf.Lerp(startY, endY, elapsedTime / duration);
            transform.position = new Vector2(transform.position.x, newY);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        bossAnimator.SetTrigger("Attack1Strike");
    }

    public void PerformStrike()
    {
        StartCoroutine(StrikeMoveTowardsTarget(strikeTargetPosition));
    }

    IEnumerator StrikeMoveTowardsTarget(Vector2 target)
    {
        float duration = 0.25f;
        float elapsedTime = 0;
        Vector2 startPosition = transform.position;
        while (elapsedTime < duration)
        {
            transform.position = Vector2.Lerp(startPosition, target, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        ApplyStrikeDamage();
        OnAttackEnd();
    }

    void ApplyStrikeDamage()
    {
        float strikeRadius = 2f;
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(transform.position, strikeRadius, LayerMask.GetMask("Player"));
        foreach (Collider2D hit in hitPlayers)
        {
            Debug.Log("Hit player: " + hit.name);
            if (hit.GetComponent<PlayerMovement>() != null)
            {
                hit.GetComponent<PlayerMovement>().TakeDamage(attackDamage);
            }
        }
    }

    void AttackType2()
    {
        Debug.Log("Performing Attack Type 2");
        UpdateOrientationTowardsPlayer();
        bossAnimator.SetTrigger("Attack2");
        isAttacking = true;
        bossAnimator.SetBool("isAttacking", true);
        StartCoroutine(HandleAttackEffect());
    }

    IEnumerator HandleAttackEffect()
    {
        bool isPlayerRightSide = playerTransform.position.x > transform.position.x;
        Vector3 spawnOffset = isPlayerRightSide ? attackEffectOffset : new Vector3(-attackEffectOffset.x, attackEffectOffset.y, attackEffectOffset.z);
        for (int i = 0; i < 3; i++)
        {
            Vector3 spawnPosition = transform.position + spawnOffset;
            GameObject effectInstance = Instantiate(attackEffectPrefab, spawnPosition, Quaternion.identity);
            effectInstance.GetComponent<Attack2Effect>().InitializeDamage(attackDamage, 0.56f);
            if (!isPlayerRightSide)
            {
                effectInstance.transform.localScale = new Vector3(-1 * effectInstance.transform.localScale.x, effectInstance.transform.localScale.y, effectInstance.transform.localScale.z);
            }

            yield return new WaitForSeconds(0.56f);
            Destroy(effectInstance);
        }
        OnAttackEnd();
    }

    IEnumerator PerformShortStep()
    {
        float shortStepDistance = 1f;
        Vector2 targetPosition = new Vector2(playerTransform.position.x, transform.position.y);
        float stepTime = 0.5f;
        float elapsedTime = 0;

        while (elapsedTime < stepTime)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, shortStepDistance * (Time.deltaTime / stepTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator PerformLongStep()
    {
        float longStepDistance = 3f;
        Vector2 targetPosition = new Vector2(playerTransform.position.x, transform.position.y);
        float stepTime = 1f;
        float elapsedTime = 0;

        while (elapsedTime < stepTime)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, longStepDistance * (Time.deltaTime / stepTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator PerformAttackType2()
    {
        yield return StartCoroutine(PerformShortStep());
        yield return StartCoroutine(PerformLongStep());
        OnAttackEnd();
    }

    void UpdateOrientationTowardsPlayer()
    {
        bool shouldFaceRight = playerTransform.position.x > transform.position.x;
        if (spriteRenderer.flipX != shouldFaceRight)
        {
            FlipCharacterDirection();
        }
    }

    public void OnAttackEnd()
    {
        isAttacking = false;
        bossAnimator.SetBool("isAttacking", false);
        attackTimer = attackCooldown;
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

    protected override void Die()
    {
        if (isDead) return;
        isDead = true;
        bossAnimator.SetTrigger("Die");
        ScoreManager.Instance.IncreaseBossKills();
    }

    private new void OnDeathAnimationComplete()
    {
        SpawnChest();
        DropExperience();
        Destroy(gameObject);
    }

    void DropExperience()
    {
        if (experiencePrefab != null)
        {
            Instantiate(experiencePrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Debug.LogError("Experience prefab is not set!");
        }
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