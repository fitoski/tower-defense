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
    private BossAbilities bossAbilities;

    public List<DropItem> droppableItems;

    private new void Start()
    {
        base.Start();
        maxHealth = 10;
        speed = 5f;
        baseDamage = 1;
        scoreValue = 15;
        experiencePointsValue = 10;
        damageMultiplierPerWave = 1.5f;
        currentHealth = maxHealth;
        base.Start();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        bossAnimator = GetComponent<Animator>();
        attackTimer = attackCooldown;
        bossAbilities = GetComponent<BossAbilities>();

        if (bossAbilities != null)
        {
            InvokeRepeating("UseSpecialAttack", 0f, bossAbilities.specialAttackCooldown);
            InvokeRepeating("UseAnotherAbility", 0f, bossAbilities.anotherAbilityCooldown);
        }
    }

    private new void Update()
    {
        if (playerTransform != null)
        {
            FollowPlayer();
        }

        if (attackTimer > 0)
        {
            base.Start();
            maxHealth = 10;
            speed = 5f;
            baseDamage = 1;
            scoreValue = 15;
            experiencePointsValue = 10;
            damageMultiplierPerWave = 1.5f;
            currentHealth = maxHealth;
            attackTimer -= Time.deltaTime;
        }
        else
        {
            if (Vector2.Distance(transform.position, playerTransform.position) <= attackRange)
            {
                AttackPlayer();
            }
        }
    }

    void FollowPlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, speed * Time.deltaTime);
    }

    void AttackPlayer()
    {
        bossAnimator.SetTrigger("Attack");
        attackTimer = attackCooldown;
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