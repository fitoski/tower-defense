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
            //Debug.Log("Teleport koşulu sağlandı, teleport işlemi başlatılıyor.");
            Teleport();
            lastHealth = currentHealth;
        }
        else
        {
            //Debug.Log($"Teleport koşulu sağlanmadı: {healthPercentageDamageTaken} < 10");
        }
    }

    void Teleport()
    {
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

            bossAnimator.SetBool("isTeleporting", false);
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

    }

    public void OnAttackEnd()
    {

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
        Debug.Log("skill item düştü");
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
            Debug.LogError("Experience prefab is not set!");
        }
    }
}
