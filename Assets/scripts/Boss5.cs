using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss5 : Enemy, IBoss
{
    private Transform playerTransform;
    private Animator bossAnimator;
    public GameObject chestPrefab;
    private float attackTimer;
    public int attackDamage = 10;
    public float attackCooldown = 5f;
    public float teleportTriggerHealthPercentage = 5f;
    public float minTeleportDistance = 1f;
    public float maxTeleportDistance = 5f;
    private float lastHealth;
    public float meleeAttackRange = 12.5f;
    private float rangedAttackTimer = 2f;
    private bool isAttacking = false;
    [SerializeField] private GameObject tentaclePrefab;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private GameObject attack3Prefab;
    [SerializeField] private GameObject attack4Prefab;
    [SerializeField] private GameObject fireBreathPrefab;
    private float meleeAttackCooldown = 2f;
    private float lastMeleeAttackTime = 0;
    private GameObject activeMeleeDamageArea;
    [SerializeField] private GameObject minePrefab;
    public float mineSpawnRadius = 5f;
    private Vector3 startPosition;
    public float frequency = 5f;
    public float magnitude = 0.5f;
    public bool isidleWorking = true;
    private Coroutine infinityMovementCoroutine;

    protected override void Start()
    {
        base.Start();
        maxHealth = 10000;
        speed = 2f;
        baseDamage = 10;
        scoreValue = 15;
        experiencePointsValue = 10;
        damageMultiplierPerWave = 1.5f;
        currentHealth = maxHealth;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        bossAnimator = GetComponent<Animator>();
        lastHealth = currentHealth;
        startPosition = transform.position;
        infinityMovementCoroutine = StartCoroutine(InfinityMovementRoutine());
    }

    IEnumerator InfinityMovementRoutine()
    {
        while (true)
        {
            if (isidleWorking)
            {
                float x = Mathf.Sin(Time.time * frequency) * magnitude;
                float y = Mathf.Cos(Time.time * frequency * 2) * magnitude;
                transform.position = startPosition + new Vector3(x, y, 0);
            }
            yield return null;
        }
    }

    private new void Update()
    {
        base.Update();
        CheckHealthForTeleport();

        if (currentHealth <= 0)
        {
            bossAnimator.SetTrigger("Die");
            return;
        }

        UpdateOrientationTowardsPlayer();
        AttackPlayer();
    }

    void UpdateOrientationTowardsPlayer()
    {
        bool shouldFaceRight = playerTransform.position.x > transform.position.x;
        GetComponent<SpriteRenderer>().flipX = !shouldFaceRight;
    }

    void CheckHealthForTeleport()
    {
        float healthPercentageDamageTaken = (lastHealth - currentHealth) / maxHealth * 100;
        if (healthPercentageDamageTaken >= 10)
        {
            Teleport();
            lastHealth = currentHealth;
        }
        else
        {
        }
    }

    void Teleport()
    {
        isidleWorking = false;
        bossAnimator.SetTrigger("Teleport1");
    }

    public void OnTeleportEnd()
    {
        if (currentHealth > 0)
        {
            float distance = Random.Range(5f, 10f);
            Vector3 direction = Random.insideUnitCircle.normalized;
            Vector3 teleportPosition = playerTransform.position + direction * distance;

            while (Vector3.Distance(teleportPosition, playerTransform.position) < 5f)
            {
                direction = Random.insideUnitCircle.normalized;
                teleportPosition = playerTransform.position + direction * distance;
            }

            transform.position = teleportPosition;
            startPosition = transform.position;

            bossAnimator.SetBool("isTeleporting", false);

            if (infinityMovementCoroutine != null)
            {
                StopCoroutine(infinityMovementCoroutine);
            }

            infinityMovementCoroutine = StartCoroutine(InfinityMovementRoutine());

            isidleWorking = true;
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

    public void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        CheckHealthForTeleport();
        RetreatAfterDamage();
    }

    void RetreatAfterDamage()
    {
        float retreatDistance = 1f;
        float retreatDuration = 0.5f;

        Vector2 retreatDirection = (transform.position - playerTransform.position).normalized;
        Vector2 retreatVelocity = retreatDirection * retreatDistance / retreatDuration;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = retreatVelocity;
            Invoke("ResetVelocity", retreatDuration);
        }
    }

    void ResetVelocity()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
        }
    }

    public void AttackPlayer()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer <= meleeAttackRange)
        {
            if (!isAttacking && Time.time - lastMeleeAttackTime >= meleeAttackCooldown)
            {
                MeleeAttack();
                lastMeleeAttackTime = Time.time;
            }
        }
        else
        {
            if (!isAttacking && rangedAttackTimer <= 0)
            {
                PerformRandomRangedAttack();
                rangedAttackTimer = attackCooldown;
            }
            else
            {
                rangedAttackTimer -= Time.deltaTime;
            }
        }
    }

    void PerformRandomRangedAttack()
    {
        int attackNumber = Random.Range(1, 6);
        switch (attackNumber)
        {
            case 1:
                attack1();
                break;
            case 2:
                attack2();
                break;
            case 3:
                attack3();
                break;
            case 4:
                attack4();
                break;
            case 5:
                attack6();
                break;
        }
    }

    private void MeleeAttack()
    {
        if (!isAttacking && Time.time - lastMeleeAttackTime >= meleeAttackCooldown)
        {
            isAttacking = true;
            bossAnimator.SetBool("isAttacking", true);
            bossAnimator.SetTrigger("MeleeAttack");
            lastMeleeAttackTime = Time.time;
            StartCoroutine(ResetIsAttacking());
        }
    }

    public void CreateMeleeDamageArea()
    {
        Vector3 spawnPosition = transform.position + (GetComponent<SpriteRenderer>().flipX ? Vector3.left : Vector3.right) * 1.5f;
        activeMeleeDamageArea = Instantiate(fireBreathPrefab, spawnPosition, Quaternion.identity);
    }

    public void MeleeAttackEnd()
    {
        if (activeMeleeDamageArea != null)
        {
            Destroy(activeMeleeDamageArea);
            activeMeleeDamageArea = null;
        }
    }

    IEnumerator ResetIsAttacking()
    {
        yield return new WaitForSeconds(1.6f);
        isAttacking = false;
        bossAnimator.SetBool("isAttacking", false);
    }

    public void attack1()
    {
        isAttacking = true;
        bossAnimator.SetBool("isAttacking", true);
        bossAnimator.SetTrigger("Attack1");
        //if (tentaclePrefab != null)
        //    {
        //        Instantiate(tentaclePrefab, playerTransform.position, Quaternion.identity);
        //    }
    }
    
    public void TriggerAttack1Effect()
    {
        Instantiate(tentaclePrefab, playerTransform.position, Quaternion.identity);
    }

    public void attack2()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            bossAnimator.SetBool("isAttacking", true);
            bossAnimator.SetTrigger("Attack2");
        }
    }

    public void FireProjectile()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
        FlippedFollowProjectile projectileScript = projectileObject.GetComponent<FlippedFollowProjectile>();
        if (projectileScript != null)
        {
            projectileScript.Initialize(playerTransform, 5f);
        }
        else
        {
        }
    }

    public void OnProjectileAttackEnd()
    {
        isAttacking = false;
        bossAnimator.SetBool("isAttacking", false);
    }

    public void attack3()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            bossAnimator.SetBool("isAttacking", true);
            bossAnimator.SetTrigger("Attack3");
            //Instantiate(attack3Prefab, playerTransform.position, Quaternion.identity);
        }
    }

    public void TriggerAttack3Effect()
    {
        Instantiate(attack3Prefab, playerTransform.position, Quaternion.identity);
    }

    void attack4()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            bossAnimator.SetBool("isAttacking", true);
            bossAnimator.SetTrigger("Attack4");
        }
    }

    public void OnAttack4End()
    {
        isAttacking = false;
        bossAnimator.SetBool("isAttacking", false);
    }

    public void LaunchAttack4Projectile()
    {
        GameObject projectileObject = Instantiate(attack4Prefab, projectileSpawnPoint.position, Quaternion.identity);
        FollowProjectile projectileScript = projectileObject.GetComponent<FollowProjectile>();
        if (projectileScript != null)
        {
            projectileScript.Initialize(playerTransform, 5f);
        }
        else
        {
        }
    }

    void attack6()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            bossAnimator.SetBool("isAttacking", true);
            bossAnimator.SetTrigger("Attack6");
            Vector2 spawnPoint = (Vector2)transform.position + Random.insideUnitCircle.normalized * mineSpawnRadius;
            Instantiate(minePrefab, spawnPoint, Quaternion.identity);
            StartCoroutine(ResetIsAttacking());
        }
    }

    public void OnAttackEnd()
    {
        bossAnimator.ResetTrigger("Attack1");
        bossAnimator.ResetTrigger("Attack2");
        bossAnimator.ResetTrigger("Attack6");
        bossAnimator.SetBool("isAttacking", false);
        isAttacking = false;
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

    void SpawnChest()
    {
        Instantiate(chestPrefab, transform.position, Quaternion.identity);
    }

    void DropSkillOnDeath()
    {
        SkillsManager.Instance.PrepareSkillRewardPanel();
    }

    void DropExperience()
    {
        if (experiencePrefab != null)
        {
            Instantiate(experiencePrefab, transform.position, Quaternion.identity);
        }
        else
        {
        }
    }
}