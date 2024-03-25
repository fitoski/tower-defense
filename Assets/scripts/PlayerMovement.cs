using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Enums;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    public float multistrike = 1.00f;
    public float criticalHitBonus = 65f;
    public float area = 2.0f;
    public float doubleAttackChance = 0f;

    public float moveSpeed = 5.0f;
    public float pickupRange = 3.0f;
    //public float xpGain = 1.0f;
    public int maxPlayerHealth = 500;
    public float healthRegenerationRate = 0.2f;
    public float blockChance = 10;
    public bool hasIncreasedBlockChance = false;
    public int playerDamage = 100;
    public float attackCooldown = 1.1f;
    public float criticalHitChance = 20f;
    public float range = 5.5f;
    private Rigidbody2D rb;
    public Transform sword;
    public float orbitRadius = 2f;
    public float orbitSpeed = 90f;
    public float attackRotationAmount = 15f;
    private float attackCooldownTimer = 0f;
    public float invulnerabilityDuration = 1.5f;
    public Color flashColor = Color.red;
    private Color originalColor;
    private SpriteRenderer playerSpriteRenderer;
    public SpriteRenderer swordSpriteRenderer;
    private Sprite currentWeaponSprite;
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
    private bool isDead = false;
    private const float perLevelDamageIncrease = 0.5f;
    private const float perLevelHealthIncrease = 2.5f;
    private const float per10LevelDamageIncrease = 0.05f;
    private bool isAttacking = false;
    public Transform attackPoint;
    public float healthRegenTickInterval = 10f;
    public int pickupRangeLevel = 0;
    public const float maxPickupColliderRadius = 0.5f;
    public const int maxPickupRangeLevel = 5;
    private bool isStunned = false;

    void Start()
    {
        //PlayerPrefs.DeleteAll(); // SİL
        rb = GetComponent<Rigidbody2D>();
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = playerSpriteRenderer.color;
        playerHealth = maxPlayerHealth;
        UpdateHealthBars();
        enemySpawner = FindObjectOfType<EnemySpawner>();
        gameManager = FindObjectOfType<GameManager>();
        swordSpriteRenderer = sword.GetComponent<SpriteRenderer>();
        currentWeaponSprite = swordSpriteRenderer.sprite;
        swordAnimator = sword.GetComponent<Animator>();
        playerAnimator = GetComponent<Animator>();
        orbitRadius = 3f;
        blockChance = 0;
        LoadUpgrades();

    }

    void LoadUpgrades()
    {
        criticalHitBonus += PlayerPrefs.GetInt("CriticalHitDamage_Level", 0) * 5f;
        doubleAttackChance += PlayerPrefs.GetInt("DoubleAttackChance_Level", 0) * 0.05f;
        ExperiencePickup.IncreaseBaseExperienceAmount(Mathf.Pow(1.2f, PlayerPrefs.GetInt("IncreaseBaseExperienceAmount_Level", 0)));
    }

    void Update()
    {
        if (isStunned)
        {
            return;
        }

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 playerPosition = transform.position;

        healthRegenTimer += Time.deltaTime;
        if (healthRegenTimer >= healthRegenTickInterval)
        {
            RegenerateHealth();
            healthRegenTimer -= healthRegenTickInterval;
        }

        if (isMoving)
        {
            Vector2 direction = (mousePosition - playerPosition).normalized;
            sword.position = playerPosition + direction * orbitRadius;
            bool shouldFlipSword = mousePosition.x < playerPosition.x;
            swordSpriteRenderer.flipX = shouldFlipSword;
            if (shouldFlipSword)
            {
                playerSpriteRenderer.flipX = true;
            }
            else
            {
                playerSpriteRenderer.flipX = false;
            }
            sword.RotateAround(playerPosition, Vector3.forward, orbitSpeed * Time.deltaTime);
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
        if (autoAimEnabled && autoAttackEnabled)
        {
            bool hasEnemyInRange = AimAtNearestEnemy(mousePosition, playerPosition);
            if (hasEnemyInRange && attackCooldownTimer <= 0f)
            {
                AutoAttack();
            }
        }
        else
        {
            UpdateSwordRotation();
        }

        if (Input.GetMouseButton(0) && attackCooldownTimer <= 0f)
        {
            isAttacking = true;
            StartAttackAnimation();
        }
        else if (!Input.GetMouseButton(0))
        {
            isAttacking = false;
        }
        if (!isAttacking)
        {
            swordSpriteRenderer.sprite = currentWeaponSprite;
        }
        UpdatePickupColliderSize();
    }

    public void Stun(float duration)
    {
        StartCoroutine(StunCoroutine(duration));
    }

    IEnumerator StunCoroutine(float duration)
    {
        isStunned = true;
        yield return new WaitForSeconds(duration);
        isStunned = false;
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
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, area);

        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.CompareTag("Enemy") || enemy.CompareTag("Boss"))
            {
                Enemy enemyComponent = enemy.GetComponent<Enemy>();
                if (enemyComponent != null)
                {
                    bool isCriticalHit = UnityEngine.Random.Range(0f, 100f) <= criticalHitChance;
                    int finalDamage = playerDamage;
                    if (isCriticalHit)
                    {
                        finalDamage = Mathf.RoundToInt(playerDamage * (1 + criticalHitBonus / 100f));
                    }
                    if (UnityEngine.Random.Range(0f, 1f) <= doubleAttackChance)
                    {
                        finalDamage *= 2;
                        Debug.Log("Double Attack!");
                    }
                    enemyComponent.TakeDamage(finalDamage, isCriticalHit);
                }
            }
        }
    }

    bool AimAtNearestEnemy(Vector2 mousePosition, Vector2 playerPosition)
    {
        Enemy nearestEnemy = FindNearestEnemy();
        if (nearestEnemy != null)
        {
            Vector2 enemyPosition = nearestEnemy.transform.position;
            Vector2 directionToEnemy = (enemyPosition - playerPosition).normalized;

            bool shouldFlipSword = enemyPosition.x < playerPosition.x;
            swordSpriteRenderer.flipX = shouldFlipSword;
            if (shouldFlipSword)
            {
                playerSpriteRenderer.flipX = true;
            }
            else
            {
                playerSpriteRenderer.flipX = false;
            }

            sword.up = directionToEnemy;
            sword.position = playerPosition + directionToEnemy * orbitRadius;

            StartAttackAnimation();
        }
        else
        {
            if (mousePosition.x < playerPosition.x)
            {
                playerSpriteRenderer.flipX = true;
            }
            else if (mousePosition.x > playerPosition.x)
            {
                playerSpriteRenderer.flipX = false;
            }
        }

        return nearestEnemy != null;
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
            bool attackStarted = StartAttackAnimation();
            if (attackStarted)
            {
                Attack();
            }
            attackCooldownTimer = attackCooldown;
        }
    }

    bool StartAttackAnimation()
    {
        if (attackCooldownTimer <= 0f)
        {
            attackCooldownTimer = attackCooldown;
            playerAnimator.SetTrigger("dkAttack");
            swordAnimator.SetTrigger("Attack");
            return true;
        }
        return false;
    }

    public void TakeDamage(int damage)
    {
        bool isBlocked = UnityEngine.Random.Range(0f, 100f) < blockChance;
        if (isBlocked)
        {
            Debug.Log("Hasar bloklandı!");
        }
        else
        {
            playerHealth -= damage;
            UpdateHealthBars();
            if (playerHealth <= 0)
            {
                Die();
            }
            else
            {
                StartCoroutine(FlashPlayer());
            }
        }
    }

    IEnumerator FlashPlayer()
    {
        playerSpriteRenderer.color = flashColor;
        yield return new WaitForSeconds(flashDuration);
        playerSpriteRenderer.color = originalColor;
    }

    IEnumerator PausePlayer()
    {
        isMoving = false;
        yield return new WaitForSeconds(0.5f);
        isMoving = true;
    }

    private float CalculateBlockChance(int incomingDamage)
    {
        return Mathf.Min(0.5f, blockChance / (float)incomingDamage) * 100f;
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
        float healthRatioBeforeIncrease = (float)playerHealth / maxPlayerHealth;
        int healthIncreaseAmount = Mathf.FloorToInt(maxPlayerHealth * healthIncreasePercentage);
        maxPlayerHealth += healthIncreaseAmount;
        playerHealth = Mathf.FloorToInt(maxPlayerHealth * healthRatioBeforeIncrease);
        UpdateHealthBars();
        hasIncreasedHealth = true;
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
        if (playerHealth < maxPlayerHealth)
        {
            playerHealth = Mathf.Min(playerHealth + 1, maxPlayerHealth);
            UpdateHealthBars();
        }
    }

    public void IncreaseHealthRegeneration()
    {
        healthRegenTickInterval = Mathf.Max(1f, healthRegenTickInterval - 0.5f);
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

    public void IncreaseBlockChance()
    {
        float blockChanceIncrease = 0.1f;
        blockChance += blockChanceIncrease;
        hasIncreasedBlockChance = true;
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
        attackCooldown = Mathf.Max(newAttackCooldown, minimumAttackCooldown);
        if (swordAnimator != null)
        {
            swordAnimator.speed = attackAnimationLength / attackCooldown;
        }
    }

    public void DecreaseAttackCooldownPerLevel()
    {
        float attackSpeedIncreaseFactor = 0.99f;
        attackCooldown *= attackSpeedIncreaseFactor;

        attackCooldown = Mathf.Max(attackCooldown, minimumAttackCooldown);

        Debug.Log($"Yeni saldırı bekleme süresi: {attackCooldown}");
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

    public void UpdateOrbitRadius(float newRadius)
    {
        orbitRadius = newRadius;
    }

    public Vector2 MovementDirection
    {
        get { return movementDirection; }
    }

    public void IncreasePickupRange()
    {
        IncreasePickupRangeLevel();
    }

    public void IncreasePickupRangeLevel()
    {
        pickupRangeLevel++;
    }

    void UpdatePickupColliderSize()
    {
        Transform pickupColliderTransform = transform.Find("PickupCollider");
        if (pickupColliderTransform != null)
        {
            CircleCollider2D pickupCollider = pickupColliderTransform.GetComponent<CircleCollider2D>();
            if (pickupCollider != null)
            {
                float newRadius = 0.13f + (pickupRangeLevel * 0.02f);
                pickupCollider.radius = newRadius;
            }
            else
            {
                Debug.LogError("CircleCollider2D component not found on the PickupCollider GameObject.");
            }
        }
        else
        {
            Debug.LogError("PickupCollider child GameObject not found.");
        }
    }

    public void EquipHelmet(HelmetItem helmet)
    {
        if (helmet.material == "Cloth")
        {
            moveSpeed *= 1.1f;
            attackCooldown *= 0.9f;
        }
        else if (helmet.material == "Leather")
        {
            playerDamage = (int)(playerDamage * 1.1f);
        }
        else if (helmet.material == "Plate")
        {
            blockChance *= 1.5f;
        }
    }

    public void EquipAmulet(AmuletItem amulet)
    {
        if (amulet.material == "Topaz")
        {
            attackCooldown *= 0.9f;
        }
        else if (amulet.material == "Ruby")
        {
            attackCooldown *= 0.9f;
            healthRegenerationRate += maxPlayerHealth * 0.01f;
        }
        else if (amulet.material == "Sapphire")
        {
            attackCooldown *= 0.9f;
            healthRegenerationRate += maxPlayerHealth * 0.01f;
            maxPlayerHealth = (int)(maxPlayerHealth * 1.1f);
            playerHealth = maxPlayerHealth;
            UpdateHealthBars();
        }
    }

    public void EquipRing(RingItem ring)
    {
        if (ring.material == "Silver")
        {
            range += range * 0.05f;
        }
        else if (ring.material == "Gold")
        {
            range += range * 0.05f;
            playerDamage = (int)(playerDamage * 1.1f);
        }
        else if (ring.material == "Diamond")
        {
            range += range * 0.05f;
            playerDamage = (int)(playerDamage * 1.1f);
        }
    }

    public void EquipGloves(GlovesItem gloves)
    {
        if (gloves.material == "Cloth")
        {
            moveSpeed *= 1f;
            attackCooldown *= 0.8f;
        }
        else if (gloves.material == "Leather")
        {
            playerDamage = (int)(playerDamage * 1f);
        }
        else if (gloves.material == "Plate")
        {
            blockChance *= 1.4f;
        }
    }

    public void EquipChestplate(ChestItem chestplate)
    {
        if (chestplate.material == "Cloth")
        {
            moveSpeed *= 2.1f;
            attackCooldown *= 1.8f;
        }
        else if (chestplate.material == "Leather")
        {
            playerDamage = (int)(playerDamage * 2.2f);
        }
        else if (chestplate.material == "Plate")
        {
            blockChance *= 3f;
        }
    }

    public void EquipBoots(BootsItem boots)
    {
        if (boots.material == "Cloth")
        {
            moveSpeed *= 1.5f;
            attackCooldown *= 1.3f;
        }
        else if (boots.material == "Leather")
        {
            playerDamage = (int)(playerDamage * 2.6f);
        }
        else if (boots.material == "Plate")
        {
            blockChance *= 3.4f;
        }
    }

    public void EquipWeapon(WeaponItem weapon)
    {
        playerDamage = (int)weapon.itemDamageBonus;
        range = weapon.itemRangeBonus;
        currentWeaponSprite = weapon.itemIcon;
        swordSpriteRenderer.sprite = currentWeaponSprite;

        if (swordAnimator != null)
        {
            swordAnimator.runtimeAnimatorController = weapon.weaponAnimator;
        }
        else
        {
            Debug.LogError("swordAnimator component not found on the sword object!");
        }

        orbitRadius = weapon.customOrbitRadius;

        if (weapon.weaponMaterial == "weapon1")
        {
            orbitRadius = 3f;
        }
        if (weapon.weaponMaterial == "weapon2")
        {
            orbitRadius = 3f;
        }
        if (weapon.weaponMaterial == "weapon3")
        {
            orbitRadius = 3f;
        }
        if (weapon.weaponMaterial == "weapon4")
        {
            orbitRadius = 3.5f;
        }
        if (weapon.weaponMaterial == "weapon5")
        {
            orbitRadius = 5f;
            attackPoint.localPosition = new Vector2(0f, 1f); // bi tık geri alınabilir.
        }
        if (weapon.weaponMaterial == "weapon6")
        {
            orbitRadius = 3f;
        }
        if (weapon.weaponMaterial == "weapon7")
        {
            orbitRadius = -1.5f;
            attackPoint.localPosition = new Vector2(0f, 2f);
        }
        if (weapon.weaponMaterial == "weapon8")
        {
            orbitRadius = -0.5f;
            attackPoint.localPosition = new Vector2(0f, 2f);
        }
        if (weapon.weaponMaterial == "weapon9")
        {
            orbitRadius = 3f;
        }
        if (weapon.weaponMaterial == "weapon10")
        {
            orbitRadius = 3f;
            attackPoint.localPosition = new Vector2(0f, -0.5f);
        }

    }

    public void Die()
    {
        isMoving = false;
        isDead = true;
        this.enabled = false;
        if (playerAnimator != null)
        {
            playerAnimator.SetTrigger("dkDeath");
        }
        StartCoroutine(WaitForDeathAnimation());
    }

    private void DisablePlayerControls()
    {
        this.enabled = false;
        if (swordAnimator != null)
        {
            swordAnimator.enabled = false;
        }
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Enemy") && !isDead)
        {
            Enemy enemy = collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                TakeDamage(enemy.baseDamage);
            }
        }
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

    private void EnablePlayerControls()
    {
        this.enabled = true;
        if (swordAnimator != null)
        {
            swordAnimator.enabled = true;
        }
    }

    public void IncreaseCriticalHitDamage(float increaseAmount)
    {
        int currentLevel = PlayerPrefs.GetInt("CriticalHitDamage_Level", 0);
        currentLevel++;
        PlayerPrefs.SetInt("CriticalHitDamage_Level", currentLevel);
        criticalHitBonus = 65 + (currentLevel * increaseAmount);
    }

    public void IncreaseDoubleAttackChance()
    {
        int currentLevel = PlayerPrefs.GetInt("DoubleAttackChance_Level", 0);
        currentLevel++;
        PlayerPrefs.SetInt("DoubleAttackChance_Level", currentLevel);
        doubleAttackChance = Mathf.Min(doubleAttackChance + (0.01f * currentLevel), 1f);
    }
}