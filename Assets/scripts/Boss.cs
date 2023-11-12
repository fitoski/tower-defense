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

    //void UseSpecialAttack()
    //{
    //    if (bossAbilities != null)
    //    {
    //        bossAbilities.SpecialAttack();
    //    }
    //}

    //void UseAnotherAbility()
    //{
    //    if (bossAbilities != null)
    //    {
    //        bossAbilities.AnotherAbility();
    //    }
    //}

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

//void DropItemOnDeath()
//{
//    List<DropItem> filteredItems = new List<DropItem>();

//    foreach (DropItem item in droppableItems)
//    {
//        float randomValue = Random.Range(0f, 100f);
//        if (randomValue <= item.dropChance)
//        {
//            filteredItems.Add(item);
//        }
//    }

//    if (filteredItems.Count > 0)
//    {
//        int randomIndex = Random.Range(0, filteredItems.Count);
//        DropItem selectedItem = filteredItems[randomIndex];

//        GameObject droppedItem = Instantiate(itemDropPrefab, transform.position, Quaternion.identity);
//        SpriteRenderer sr = droppedItem.GetComponent<SpriteRenderer>();
//        if (sr)
//        {
//            sr.sprite = selectedItem.itemIcon;
//        }
//        Debug.Log("eşya: " + selectedItem.itemName);
//    }
//}