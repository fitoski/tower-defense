using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Enums;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    // Player attributes
    public float moveSpeed = 5.0f; // Movement Speed
    public float pickupRange = 3.0f; // Pickup Range
    public float xpGain = 1.0f; // XP Gain Multiplier

    // Defensive attributes
    public int maxPlayerHealth = 500; // Max Health
    public float healthRegenerationRate = 0.2f; // Health Regeneration per second
    public float armorValue = 10; // Defense
    public float blockStrength = 10; // Block Strength
    public float defense = 10f; // Savunma değeri
    public float defenseBonus = 1f; // Savunma bonusu
    public bool hasIncreasedDefense = false;
    public bool hasIncreasedDefenseBonus = false;
    public bool hasIncreasedBlockStrength = false;

    // Offensive attributes
    public int playerDamage = 100; // Damage
    public float attackCooldown = 1.1f; // Attack Speed (attacks per second)
    public float multistrike = 1.00f; // Multistrike Chance
    public float criticalHitChance = 20f; // Crit Chance
    public float criticalHitBonus = 65f; // Crit Bonus
    public float range = 5.5f; // Attack Range
    public float area = 2.0f; // Area of Effect

    // Other attributes
    private Rigidbody2D rb;
    public Transform sword;
    public float orbitRadius = 2f;
    public float orbitSpeed = 90f;
    public float attackRotationAmount = 15f;
    private float attackCooldownTimer = 0f;
    private bool isInvulnerable = false;
    public float invulnerabilityDuration = 1.5f;
    public Color flashColor = Color.red;
    private Color originalColor;
    private SpriteRenderer playerSpriteRenderer;
    public SpriteRenderer swordSpriteRenderer;
    public List<Sprite> weaponSprites;
    private Animator playerAnimator;
    private bool isMoving = true;
    public int playerHealth;
    public Image playerHealthBar;
    private EnemySpawner enemySpawner;
    public TextMeshProUGUI waveDisplayText;
    private GameManager gameManager;
    private Animator swordAnimator;
    private Vector2 directionToMouse;
    private Vector2 movementDirection;
    private float attackAnimationLength = 0;
    public float deathAnimationLength = 2f;
    private float healthRegenTimer = 0f;
    public bool hasIncreasedDamage = false;
    public bool hasIncreasedHealth = false;
    public bool hasIncreasedSpeed = false;
    public bool hasIncreasedArmor = false;
    public bool hasIncreasedCritChance = false;
    public bool hasIncreasedHealthRegen = false;
    public bool hasIncreasedOrbitRadius = false;
    public bool hasDecreasedAttackCooldown = false;
    public const float healthRegenInterval = 5f;
    public const float flashDuration = 0.1f;
    public const float damageIncreasePercentage = 0.05f;
    public const float maxMoveSpeed = 10f;
    public const float speedIncreasePercentage = 0.05f;
    public const float healthIncreasePercentage = 0.05f;
    public const float criticalHitChanceFirstTimeIncrease = 0.05f;
    public const float criticalHitChanceMultiplier = 1.05f;
    public const float healthRegenFirstTimeValue = 1f;
    public const float healthRegenBaseMultiplier = 0.1f;
    public const float orbitRadiusIncreasePercentage = 0.05f;
    public const float cooldownDecreasePercentage = 0.05f;
    public const float maxOrbitRadius = 5f;
    public const float maxHealthRegenRate = 5f;
    public const float minimumAttackCooldown = 0.5f;
    public const float healthRegenIncreasePercentage = 0.05f;
    private bool autoAimEnabled = false;
    private bool autoAttackEnabled = false;
    private Transform nearestEnemy;

    // Level up bonuses
    private const float perLevelDamageIncrease = 0.5f; // Damage increase per level
    private const float perLevelHealthIncrease = 2.5f; // Health increase per level
    private const float per10LevelDamageIncrease = 0.05f; // Additional Damage increase every 10 levels




    void Start()
    {
        // Initialize values and components
        rb = GetComponent<Rigidbody2D>();
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = playerSpriteRenderer.color;
        playerHealth = maxPlayerHealth; // Set initial health
        UpdateHealthBars();
        enemySpawner = FindObjectOfType<EnemySpawner>();
        gameManager = FindObjectOfType<GameManager>();
        swordSpriteRenderer = sword.GetComponent<SpriteRenderer>();
        swordAnimator = sword.GetComponent<Animator>();
        playerAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        healthRegenTimer += Time.deltaTime;
        if (healthRegenTimer >= healthRegenInterval)
        {
            RegenerateHealth();
            healthRegenTimer = 0f;
        }
        if (isMoving)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 playerPosition = transform.position;
            if (mousePosition.x < playerPosition.x)
            {
                playerSpriteRenderer.flipX = true;
            }
            else if (mousePosition.x > playerPosition.x)
            {
                playerSpriteRenderer.flipX = false;
            }
            if (!autoAimEnabled)
            {
                Vector2 direction = (mousePosition - playerPosition).normalized;
                sword.position = playerPosition + direction * orbitRadius;
                sword.up = direction;
                sword.RotateAround(playerPosition, Vector3.forward, orbitSpeed * Time.deltaTime);
            }
            bool isAttacking = Input.GetMouseButton(0);
            directionToMouse = (mousePosition - playerPosition).normalized;
            movementDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
            if (isAttacking && attackCooldownTimer <= 0f)
            {
                StartAttackAnimation();
            }
            attackCooldownTimer -= Time.deltaTime;
        }
        if (waveDisplayText != null && enemySpawner != null)
        {
            waveDisplayText.text = "Wave: " + enemySpawner.GetCurrentWaveNumber().ToString();
        }
        if (Input.GetMouseButtonDown(1))
        {
            autoAimEnabled = !autoAimEnabled;
            autoAttackEnabled = autoAimEnabled; 
        }
        if (autoAimEnabled && autoAttackEnabled && attackCooldownTimer <= 0f)
        {
            AimAtNearestEnemy();
            AutoAttack();
        }
        else
        {
            UpdateSwordRotation();
        }
    }

    private void FixedUpdate()
    {
        float moveInputX = Input.GetAxisRaw("Horizontal");
        float moveInputY = Input.GetAxisRaw("Vertical");
        bool isWalking = moveInputX != 0 || moveInputY != 0;
        playerAnimator.SetBool("dkWalk", isWalking);
        Vector2 movement = new Vector2(moveInputX, moveInputY).normalized * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(movement + rb.position);
        bool isMovingBackwards = Vector2.Dot(directionToMouse, movementDirection) < 0;
        playerAnimator.SetBool("dkWalkBackwards", isMovingBackwards);
    }

    public void Attack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(sword.position, area);

        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.CompareTag("Enemy") || enemy.CompareTag("Boss"))
            {
                Enemy enemyComponent = enemy.GetComponent<Enemy>();
                if (enemyComponent != null)
                {
                    bool isDead = enemyComponent.TakeDamage(playerDamage);
                    if (isDead)
                    {
                        GameManager.main.EnemyKilled();
                    }
                }
            }
        }
    }

    void AimAtNearestEnemy()
    {
        Enemy nearestEnemy = FindNearestEnemy();
        if (nearestEnemy != null)
        {
            Vector2 enemyPosition = nearestEnemy.transform.position;
            Vector2 playerPosition = transform.position;

            Vector2 directionToEnemy = (enemyPosition - playerPosition).normalized;

            playerSpriteRenderer.flipX = enemyPosition.x < playerPosition.x;

            sword.up = directionToEnemy;
            sword.position = playerPosition + directionToEnemy * orbitRadius;

            StartAttackAnimation();
        }
    }

    Enemy FindNearestEnemy()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        Enemy nearestEnemy = null;
        float minDistance = Mathf.Infinity;
        Vector2 position = transform.position;

        foreach (Enemy enemy in enemies)
        {
            float distance = Vector2.Distance(enemy.transform.position, position);
            if (distance < minDistance)
            {
                nearestEnemy = enemy;
                minDistance = distance;
            }
        }
        return nearestEnemy;
    }

    void UpdateSwordRotation()
    {
        if (!autoAimEnabled)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 playerPosition = transform.position;

            Vector2 direction = (mousePosition - playerPosition).normalized;

            sword.up = direction;

            sword.position = playerPosition + direction * orbitRadius;
        }
    }

    void AttackNearestEnemy()
    {
        if (nearestEnemy != null)
        {
            Attack();
        }
    }

    void AutoAttack()
    {
        if (attackCooldownTimer <= 0f)
        {
            AimAtNearestEnemy();  
            Attack();
            attackCooldownTimer = attackCooldown; 
        }
    }

    void StartAttackAnimation()
    {
        attackCooldownTimer = attackCooldown;
        playerAnimator.SetTrigger("dkAttack");
        swordAnimator.SetTrigger("Attack");
    }

    public void TakeDamage(int damage)
    {
        if (!isInvulnerable)
        {
            float blockChance = CalculateBlockChance(damage);
            if (UnityEngine.Random.Range(0f, 100f) <= blockChance)
            {
                return;
            }

            float damageReduction = CalculateDamageReduction(defense);
            int actualDamage = Mathf.Max(1, damage - Mathf.FloorToInt(damage * damageReduction / 100f));
            playerHealth -= actualDamage;

            UpdateHealthBars();
            StartCoroutine(FlashPlayer());
            StartCoroutine(PausePlayer());

            if (playerHealth <= 0)
            {
                Die();
            }
            else
            {
                isInvulnerable = true;
                Invoke("ResetInvulnerability", 0.2f); 
            }
        }
    }

    IEnumerator FlashPlayer()
    {
        float flashEndTime = Time.time + flashDuration;

        while (Time.time < flashEndTime)
        {
            playerSpriteRenderer.color = flashColor;
            yield return new WaitForSeconds(0.05f);
            playerSpriteRenderer.color = originalColor;
            yield return new WaitForSeconds(0.05f);
        }

        playerSpriteRenderer.color = originalColor;
    }

    IEnumerator PausePlayer()
    {
        isMoving = false;
        yield return new WaitForSeconds(0.2f); 
        isMoving = true;
    }

    void ResetInvulnerability()
    {
        isInvulnerable = false;
    }

    private float CalculateBlockChance(int incomingDamage)
    {
        return Mathf.Min(0.5f, blockStrength / (float)incomingDamage) * 100f;
    }

    private float CalculateDamageReduction(float defense)
    {
        float inverseHyperbolicEffect = Mathf.Sign(defense) * (0.6f - 24f / (Mathf.Abs(defense) + 40f));
        float clippedLinearEffect = Mathf.Min(0.4f, 0.004f * defense);
        return (inverseHyperbolicEffect + clippedLinearEffect) * 100f;
    }

    void UpdateHealthBars()
    {
        float playerHealthRatio = (float)playerHealth / maxPlayerHealth;
        playerHealthBar.fillAmount = playerHealthRatio;
    }

    public void IncreaseDamagePerLevel()
    {
        int damageIncreaseAmount = Mathf.FloorToInt(playerDamage * damageIncreasePercentage);
        playerDamage += damageIncreaseAmount;
        hasIncreasedDamage = true;
    }

    public void IncreaseMoveSpeedPerLevel()
    {
        if (moveSpeed < maxMoveSpeed)
        {
            float speedIncreaseAmount = moveSpeed * speedIncreasePercentage;
            moveSpeed = Mathf.Min(moveSpeed + speedIncreaseAmount, maxMoveSpeed);
            hasIncreasedSpeed = true;
        }
    }

    public void IncreaseMaxHealthPerLevel()
    {
        int healthIncreaseAmount = Mathf.FloorToInt(maxPlayerHealth * healthIncreasePercentage);
        maxPlayerHealth += healthIncreaseAmount;
        playerHealth = maxPlayerHealth;
        hasIncreasedHealth = true;
        UpdateHealthBars();
    }

    public void IncreaseCriticalHitChance()
    {
        if (criticalHitChance == 0)
        {
            criticalHitChance = criticalHitChanceFirstTimeIncrease;
        }
        else
        {
            criticalHitChance *= criticalHitChanceMultiplier;
            criticalHitChance = Mathf.Min(criticalHitChance, 1f);
        }

        hasIncreasedCritChance = true;
    }


    void RegenerateHealth()
    {
        if (healthRegenerationRate > 0f)
        {
            playerHealth = Mathf.Min(playerHealth + (int)healthRegenerationRate, maxPlayerHealth);
            UpdateHealthBars();
        }
    }

    public void IncreaseHealthRegeneration()
    {
        if (healthRegenerationRate <= 0f)
        {
            healthRegenerationRate = Mathf.Max(healthRegenFirstTimeValue, maxHealthRegenRate * healthRegenBaseMultiplier);
        }
        else if (healthRegenerationRate < maxHealthRegenRate)
        {
            float healthRegenIncreaseAmount = healthRegenerationRate * healthRegenIncreasePercentage;
            healthRegenerationRate = Mathf.Min(healthRegenerationRate + healthRegenIncreaseAmount, PlayerMovement.maxHealthRegenRate);
        }

        hasIncreasedHealthRegen = true;
    }

    public void IncreaseOrbitRadiusPerLevel()
    {
        if (orbitRadius < maxOrbitRadius)
        {
            float orbitRadiusIncreaseAmount = orbitRadius * orbitRadiusIncreasePercentage;
            orbitRadius = Mathf.Min(orbitRadius + orbitRadiusIncreaseAmount, maxOrbitRadius);
            hasIncreasedOrbitRadius = true;
        }
    }

    public void IncreaseBlockStrength()
    {
        float blockStrengthIncrease = 0.1f; 
        blockStrength += blockStrengthIncrease;
        hasIncreasedBlockStrength = true;
    }

    public void IncreaseDefense()
    {
        float defenseIncrease = 0.2f; 
        defense += defenseIncrease;
        hasIncreasedDefense = true;
    }

    public void IncreaseDefenseBonus()
    {
        float defenseBonusIncrease = 0.05f; 
        defenseBonus += defenseBonusIncrease;
        hasIncreasedDefenseBonus = true;
    }

    public void ChangeArmor(float newArmorValue)
    {
        armorValue = newArmorValue;
        hasIncreasedArmor = true;
    }

    public void AdjustAttackSpeedAndAnimation(float newAttackCooldown)
    {
        AnimationClip[] clips = swordAnimator.runtimeAnimatorController.animationClips;

        bool isFound = false;
        foreach (AnimationClip clip in clips)
        {
            switch (clip.name)
            {
                case "sword3Attack":
                    attackAnimationLength = clip.length;
                    isFound = true;
                    break;
                case "sword2Attack":
                    attackAnimationLength = clip.length;
                    isFound = true;
                    break;
            }

            if (isFound)
            {
                break;
            }
        }

        attackCooldown = newAttackCooldown;
        if (swordAnimator != null)
        {
            swordAnimator.speed = attackAnimationLength / attackCooldown;
        }
    }

    public void DecreaseAttackCooldownPerLevel()
    {
        float decreaseAmount = attackCooldown * cooldownDecreasePercentage;
        float newCooldown = Mathf.Max(attackCooldown - decreaseAmount, minimumAttackCooldown);
        hasDecreasedAttackCooldown = true;
    }

    public void ChangeWeapon(int newDamage, float newRange, int weaponIndex, RuntimeAnimatorController weaponAnimator)
    {
        playerDamage = newDamage;
        orbitRadius = newRange;
        swordSpriteRenderer.sprite = weaponSprites[weaponIndex];

        if (swordAnimator != null)
        {
            swordAnimator.runtimeAnimatorController = weaponAnimator;
        }
        else
        {
            Debug.LogError("swordAnimator component not found on the sword object!");
        }
    }

    public Vector2 MovementDirection
    {
        get { return movementDirection; } 
    }

    public void Die()
    {
        isMoving = false;
        if (playerAnimator != null)
        {
            playerAnimator.SetTrigger("dkDeath");
        }
        StartCoroutine(WaitForDeathAnimation());
    }

    public void TriggerDeathScreen()
    {
        GameManager.main.ShowDeathScreen();
    }

    private IEnumerator WaitForDeathAnimation()
    {
        yield return new WaitForSeconds(deathAnimationLength);
        GameManager.main.ShowDeathScreen();
    }
}

