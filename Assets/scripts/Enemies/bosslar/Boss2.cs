using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2 : Enemy
{
    public float attackRange = 5f;
    public int attackDamage = 10;
    public float attackCooldown = 5f;
    private float attackTimer;
    //private float attack1Timer = 4f;

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

        if (!isAttacking)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
            if (distanceToPlayer <= attackRange && attackTimer <= 0)
            {
                AttackPlayer();
            }
            else
            {
                FollowPlayer();
            }
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
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer > attackRange && !isAttacking)
        {
            AttackType1();
        }
        else if (distanceToPlayer <= attackRange && attackTimer <= 0 && !isAttacking)
        {
            AttackType2();
            attackTimer = 5f; 
        }
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
        bossAnimator.SetTrigger("Attack1Strike");
    }

    IEnumerator WaitForStrikeAnimation()
    {
        yield return new WaitForSeconds(0.6f); 

        transform.position = playerTransform.position;

        PerformStrike();
        OnAttackEnd();
    }

    void AttackType2()
    {
        Debug.Log("Performing Attack Type 2");
        UpdateOrientationTowardsPlayer();
        bossAnimator.SetTrigger("Attack2");
        isAttacking = true;
        bossAnimator.SetBool("isAttacking", true);
        StartCoroutine(PerformAttackType2());
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

    public void PerformLeap()
    {
        //float upwardMovement = 2f; 
        //float leapSpeed = 10f; 

        //Vector2 newPosition = new Vector2(transform.position.x, transform.position.y + upwardMovement);
        //transform.position = Vector2.Lerp(transform.position, newPosition, leapSpeed * Time.deltaTime);

        float upwardMovement = 2f; 
        transform.position = new Vector2(transform.position.x, transform.position.y + upwardMovement);
    }

    public void PerformStrike()
    {
        Debug.Log("Performing strike");
        float strikeRadius = 10f;
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

    void UpdateOrientationTowardsPlayer()
    {
        bool shouldFaceRight = playerTransform.position.x > transform.position.x;
        GetComponent<SpriteRenderer>().flipX = shouldFaceRight;
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