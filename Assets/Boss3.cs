using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss3 : Enemy, IBoss
{
    public float meleeAttackRange = 3f;
    public int meleeAttackDamage = 20;
    public float meleeAttackCooldown = 2f;
    private float meleeAttackTimer;
    private bool isAttacking = false;
    public GameObject meleeAttackPrefab;
    private Animator bossAnimator;
    private Transform playerTransform;
    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;
    public float projectileCooldown = 5f;
    private float projectileTimer;

    protected override void Start()
    {
        base.Start();
        bossAnimator = GetComponent<Animator>();
        maxHealth = 50;
        speed = 2f;
        baseDamage = 10;
        scoreValue = 15;
        experiencePointsValue = 10;
        damageMultiplierPerWave = 1.5f;
        currentHealth = maxHealth;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        meleeAttackTimer = 0f;
        projectileTimer = projectileCooldown;
    }

    protected override void Update()
    {
        base.Update();
        projectileTimer -= Time.deltaTime;

        if (isDead) return;

        meleeAttackTimer -= Time.deltaTime;

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        if (!isAttacking && distanceToPlayer <= meleeAttackRange && meleeAttackTimer <= 0)
        {
            AttackPlayer();
        }
        else if (!isAttacking && distanceToPlayer > meleeAttackRange)
        {
            FollowPlayer();
        }
        if (!isAttacking && distanceToPlayer > meleeAttackRange && projectileTimer <= 0)
        {
            PerformProjectileAttack();
            projectileTimer = projectileCooldown;
        }
    }

    public void AttackPlayer()
    {
        if (!isAttacking && meleeAttackTimer <= 0)
        {
            Debug.Log("Performing melee attack");
            isAttacking = true;
            bossAnimator.SetBool("isAttacking", true);
            bossAnimator.SetTrigger("MeleeAttack");
            meleeAttackTimer = meleeAttackCooldown;

            StartCoroutine(InitiateMeleeAttack());
        }
    }

    void PerformProjectileAttack()
    {
        Debug.Log("Performing projectile attack");
        isAttacking = true;
        bossAnimator.SetBool("isAttacking", true);
        bossAnimator.SetTrigger("ProjectileAttack");
        GameObject projectileObject = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
        Projectile projectileScript = projectileObject.GetComponent<Projectile>();
        projectileScript.Initialize(playerTransform.position);
    }

    IEnumerator InitiateMeleeAttack()
    {
        yield return new WaitForSeconds(0.5f);
        Vector3 spawnPosition = transform.position + (GetComponent<SpriteRenderer>().flipX ? new Vector3(-1, -1, 0) : new Vector3(1, -1, 0));
        Instantiate(meleeAttackPrefab, spawnPosition, Quaternion.identity);
    }

    IEnumerator HandleAttackEffect()
    {
        yield return new WaitForSeconds(0.56f);
        DealDamageToPlayer();
        OnAttackEnd();
    }

    public void OnMeleeAttackEnd()
    {
        isAttacking = false;
        bossAnimator.SetBool("isAttacking", false);
        FollowPlayer();
    }

    public void OnProjectileAttackEnd()
    {
        isAttacking = false;
        bossAnimator.SetBool("isAttacking", false);
        bossAnimator.ResetTrigger("ProjectileAttack");
        FollowPlayer();
    }

    public void OnAttackEnd()
    {
        isAttacking = false;
        bossAnimator.SetBool("isAttacking", false);
    }

    public void DealDamageToPlayer()
    {
        if (Vector2.Distance(transform.position, playerTransform.position) <= meleeAttackRange)
        {
            Debug.Log("Boss3 melee attack dealt");
        }
    }

    private void FollowPlayer()
    {
        if (!isAttacking)
        {
            UpdateOrientationTowardsPlayer();
            transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, speed * Time.deltaTime);
        }
    }

    private void AdjustColliderOffset(bool shouldFaceRight)
    {
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            if ((shouldFaceRight && collider.offset.x < 0) || (!shouldFaceRight && collider.offset.x > 0))
            {
                collider.offset = new Vector2(-collider.offset.x, collider.offset.y);
            }
        }
        else
        {
            Debug.LogError("Collider2D component not found on " + gameObject.name);
        }
    }

    private void UpdateOrientationTowardsPlayer()
    {
        bool shouldFaceRight = playerTransform.position.x > transform.position.x;
        GetComponent<SpriteRenderer>().flipX = shouldFaceRight;

        AdjustColliderOffset(shouldFaceRight);

        if (projectileSpawnPoint != null)
        {
            projectileSpawnPoint.localPosition = new Vector3(
            Mathf.Abs(projectileSpawnPoint.localPosition.x) * (shouldFaceRight ? 1 : -1), projectileSpawnPoint.localPosition.y, projectileSpawnPoint.localPosition.z);
        }
    }
}