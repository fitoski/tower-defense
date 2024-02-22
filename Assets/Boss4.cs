using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss4 : Enemy, IBoss

{
    public float meleeAttackRange = 3f;
    public int meleeAttackDamage = 20;
    public float meleeAttackCooldown = 2f;
    private float meleeAttackTimer;

    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;
    public float projectileCooldown = 5f;
    private float projectileTimer;

    private Animator bossAnimator;
    private Transform playerTransform;
    private bool isAttacking = false;

    protected override void Start()
    {
        base.Start();
        bossAnimator = GetComponent<Animator>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        meleeAttackTimer = meleeAttackCooldown;
        projectileTimer = projectileCooldown;
    }

    protected override void Update()
    {
        base.Update();
        HandleTimers();

        if (isDead) return;

        var distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        HandleMovementAndAttacks(distanceToPlayer);
    }

    public void AttackPlayer()
    {
        PerformMeleeAttack();
    }

    private void HandleTimers()
    {
        meleeAttackTimer -= Time.deltaTime;
        projectileTimer -= Time.deltaTime;
    }

    private void HandleMovementAndAttacks(float distanceToPlayer)
    {
        if (!isAttacking && distanceToPlayer <= meleeAttackRange && meleeAttackTimer <= 0)
        {
            PerformMeleeAttack();
        }
        else if (!isAttacking && distanceToPlayer > meleeAttackRange && projectileTimer <= 0)
        {
            PerformProjectileAttack();
        }
        else if (!isAttacking)
        {
            FollowPlayer();
        }
    }

    private void PerformMeleeAttack()
    {
        if (!isAttacking && meleeAttackTimer <= 0)
        {
            Debug.Log("Performing melee attack");
            isAttacking = true;
            bossAnimator.SetBool("isAttacking", true);
            bossAnimator.SetTrigger("MeleeAttack");
            meleeAttackTimer = meleeAttackCooldown;
        }
    }

    private void PerformProjectileAttack()
    {
        isAttacking = true;
        bossAnimator.SetBool("isAttacking", true);
        bossAnimator.SetTrigger("ProjectileAttack");
        projectileTimer = projectileCooldown;

        var projectileObject = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
        var projectileScript = projectileObject.GetComponent<FollowProjectile>();
        projectileScript.Initialize(playerTransform, 5f);
    }

    public void OnMeleeAttackEnd()
    {
        isAttacking = false;
        bossAnimator.SetBool("isAttacking", false);
    }

    public void OnProjectileAttackEnd()
    {
        isAttacking = false;
        bossAnimator.SetBool("isAttacking", false);
    }

    private void FollowPlayer()
    {
        UpdateOrientationTowardsPlayer();
        transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, speed * Time.deltaTime);
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

        GetComponent<SpriteRenderer>().flipX = !shouldFaceRight;

        if (projectileSpawnPoint != null)
        {
            projectileSpawnPoint.localPosition = new Vector3(
                Mathf.Abs(projectileSpawnPoint.localPosition.x) * (shouldFaceRight ? 1 : -1),
                projectileSpawnPoint.localPosition.y,
                projectileSpawnPoint.localPosition.z
            );

            projectileSpawnPoint.localRotation = Quaternion.Euler(0, 0, 0); 
        }
    }
}