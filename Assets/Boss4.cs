using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss4 : Enemy, IBoss

{
    public float meleeAttackRange = 3f;
    public int meleeAttackDamage = 20;
    public float meleeAttackCooldown = 2f;
    private float meleeAttackTimer;
    public GameObject meleeAttackPrefab;
    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;
    public float projectileCooldown = 5f;
    private float projectileTimer;
    public GameObject currentMeleeAttackZone;
    private Animator bossAnimator;
    private Transform playerTransform;
    private bool isAttacking = false;
    private float attackDelay = 2f; 
    private float lastAttackTime = -2f; 

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

    private void HandleTimers()
    {
        meleeAttackTimer -= Time.deltaTime;
        projectileTimer -= Time.deltaTime;
    }

    private void HandleMovementAndAttacks(float distanceToPlayer)
    {
        if (!isAttacking && Time.time >= lastAttackTime + attackDelay)
        {
            if (distanceToPlayer <= meleeAttackRange && meleeAttackTimer <= 0)
            {
                PerformMeleeAttack();
            }
            else if (distanceToPlayer > meleeAttackRange * 2 && projectileTimer <= 0)
            {
                PerformProjectileAttack();
            }
        }

        if (!isAttacking)
        {
            FollowPlayer();
        }
    }

    private void PerformMeleeAttack()
    {
        Debug.Log("Performing melee attack");
        isAttacking = true;
        bossAnimator.SetBool("isAttacking", true);
        bossAnimator.SetTrigger("MeleeAttack");
        meleeAttackTimer = meleeAttackCooldown;
        lastAttackTime = Time.time;
    }

    private void PerformProjectileAttack()
    {
        Debug.Log("Performing projectile attack");
        if (!isAttacking)
        {
            isAttacking = true;
            bossAnimator.SetBool("isAttacking", true);
            bossAnimator.SetTrigger("ProjectileAttack");
            lastAttackTime = Time.time;

            GameObject projectileObject = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
            FollowProjectile projectileScript = projectileObject.GetComponent<FollowProjectile>();
            if (projectileScript != null)
            {
                projectileScript.Initialize(playerTransform, 5f);
            }
            else
            {
                Debug.LogError("Projectile prefab does not have a FollowProjectile component.");
            }
        }
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
        if (!isAttacking)
        {
            UpdateOrientationTowardsPlayer();
            transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, speed * Time.deltaTime);
        }
    }

    private void UpdateOrientationTowardsPlayer()
    {
        bool shouldFaceRight = playerTransform.position.x > transform.position.x;
        GetComponent<SpriteRenderer>().flipX = !shouldFaceRight;

        AdjustColliderOffset(shouldFaceRight);

        if (projectileSpawnPoint != null)
        {
            projectileSpawnPoint.localPosition = new Vector3(
                Mathf.Abs(projectileSpawnPoint.localPosition.x) * (shouldFaceRight ? 1 : -1),
                projectileSpawnPoint.localPosition.y,
                projectileSpawnPoint.localPosition.z
            );

            projectileSpawnPoint.localRotation = Quaternion.Euler(0, shouldFaceRight ? 0 : 180, 0);
        }
    }

    private void AdjustColliderOffset(bool shouldFaceRight)
    {
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            var offset = collider.offset;
            collider.offset = new Vector2(Mathf.Abs(offset.x) * (shouldFaceRight ? 1 : -1), offset.y);
        }
    }

    public void CreateMeleeAttackZone()
    {
        if (meleeAttackPrefab != null)
        {
            Vector3 attackZonePosition = transform.position + (GetComponent<SpriteRenderer>().flipX ? Vector3.left : Vector3.right) * 0.5f;
            Instantiate(meleeAttackPrefab, attackZonePosition, Quaternion.identity, transform);
        }
    }

    public void AttackPlayer()
    {

    }
}