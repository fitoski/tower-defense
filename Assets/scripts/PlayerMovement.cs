using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Enums;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    public Transform sword;
    public float orbitRadius = 2f;
    public float orbitSpeed = 90f;
    public float attackRotationAmount = 15f;
    public float attackCooldown = 1f;
    private float attackCooldownTimer = 0f;
    public int playerDamage = 5;
    public float criticalHitChance = 0.0f;
    public float healthRegenerationRate = 0.0f;
    private bool isInvulnerable = false;
    public float invulnerabilityDuration = 1.5f;
    public float armorValue;
    public float flashDuration = 0.2f;
    public Color flashColor = Color.red;
    private Color originalColor;
    private SpriteRenderer playerSpriteRenderer;
    public SpriteRenderer swordSpriteRenderer;
    public List<Sprite> weaponSprites;
    private Animator playerAnimator;
    private bool isMoving = true;
    public int maxPlayerHealth = 100;
    public int playerHealth = 100;
    public float minimumAttackCooldown = 0.5f;
    public float maxMoveSpeed = 10f;
    public float maxOrbitRadius = 5f;
    public float maxHealthRegenRate;
    public float cooldownDecreasePercentage = 0.05f;
    public float criticalHitChanceIncreasePercentage = 0.02f;
    public float healthRegenIncreasePercentage = 0.03f;
    public float healthIncreasePercentage = 0.05f;
    public float damageIncreasePercentage = 0.10f;
    public float speedIncreasePercentage = 0.03f;
    public float orbitRadiusIncreasePercentage = 0.02f;
    public bool hasIncreasedHealthRegen = false;
    public bool hasIncreasedCritChance = false;
    public bool hasIncreasedDamage = false;
    public bool hasIncreasedHealth = false;
    public bool hasIncreasedSpeed = false;
    public bool hasIncreasedArmor = false;
    public bool hasIncreasedOrbitRadius = false;
    public bool hasDecreasedAttackCooldown = false;
    public Image playerHealthBar;
    private EnemySpawner enemySpawner;
    public Text waveDisplayText;
    private GameManager gameManager;
    private Animator swordAnimator;
    private Vector2 directionToMouse;
    private Vector2 movementDirection;

    void Start()
    {
        gameManager = GameManager.main;

        rb = GetComponent<Rigidbody2D>();
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = playerSpriteRenderer.color;
        UpdateHealthBars();
        enemySpawner = FindObjectOfType<EnemySpawner>();
        gameManager = FindObjectOfType<GameManager>();
        swordSpriteRenderer = sword.GetComponent<SpriteRenderer>();
        swordAnimator = sword.GetComponent<Animator>();
        playerAnimator = GetComponent<Animator>();
        maxHealthRegenRate = maxPlayerHealth * 0.5f;
        if (playerAnimator == null)
        {
            Debug.LogError("Animator component not found on the player!");
        }
    }

    void Update()
    {
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
            Vector2 direction = (mousePosition - playerPosition).normalized;
            sword.position = playerPosition + direction * orbitRadius;
            sword.up = direction;
            sword.RotateAround(playerPosition, Vector3.forward, orbitSpeed * Time.deltaTime);
            bool isAttacking = Input.GetMouseButton(0);
            swordAnimator.SetBool("isAttacking", isAttacking);
            directionToMouse = (mousePosition - playerPosition).normalized;
            movementDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

            if (playerAnimator != null)
            {
                playerAnimator.SetBool("dkAttack", isAttacking);
            }

            if (isAttacking && attackCooldownTimer <= 0f)
            {
                sword.Rotate(Vector3.forward, attackRotationAmount);
                attackCooldownTimer = attackCooldown;
                Attack();
            }

            if (attackCooldownTimer > 0f)
            {
                attackCooldownTimer -= Time.deltaTime;
            }
            RegenerateHealth();
        }
        
        if (waveDisplayText != null && enemySpawner != null)
        {
            waveDisplayText.text = "Wave: " + enemySpawner.GetCurrentWaveNumber().ToString();
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

    void Attack()
    {
        sword.Rotate(Vector3.forward, attackRotationAmount);
        attackCooldownTimer = attackCooldown;
        swordAnimator.SetBool("isAttacking", true);
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(sword.position, 2f);

        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.CompareTag("Enemy") || enemy.CompareTag("Boss"))
            {
                Enemy enemyComponent = enemy.GetComponent<Enemy>();
                if (enemyComponent != null)
                {
                    enemyComponent.TakeDamage(playerDamage);
                }
            }
        }
    }

    public void TakeDamage(int damage)
        {
            if (!isInvulnerable)
            {
                int actualDamage = (int)(10 * (1 - armorValue / 100));
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
                    Invoke("ResetInvulnerability", invulnerabilityDuration);
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
            criticalHitChance += criticalHitChanceIncreasePercentage;
            criticalHitChance = Mathf.Min(criticalHitChance, 1f);
            hasIncreasedCritChance = true;
        }

        public void IncreaseHealthRegeneration()
        {
            if (healthRegenerationRate < maxHealthRegenRate)
            {
                float healthRegenIncreaseAmount = (healthRegenerationRate > 0 ? healthRegenerationRate : maxPlayerHealth * 0.05f) * (healthRegenIncreasePercentage + 0.05f);
                healthRegenerationRate = Mathf.Min(healthRegenerationRate + healthRegenIncreaseAmount, maxHealthRegenRate);
                hasIncreasedHealthRegen = true;
            }
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

        void RegenerateHealth()
        {
            if (healthRegenerationRate > 0f)
            {
                playerHealth += (int)(healthRegenerationRate * Time.deltaTime);
                playerHealth = Mathf.Min(playerHealth, maxPlayerHealth);
                UpdateHealthBars();
            }
        }

        public void ChangeArmor(float newArmorValue)
        {
            armorValue = newArmorValue;
            hasIncreasedArmor = true;
        }

        public void AdjustAttackSpeedAndAnimation(float newAttackCooldown)
        {
            attackCooldown = newAttackCooldown;
            if (swordAnimator != null)
            {
                swordAnimator.speed = 1.0f / attackCooldown;
            }
        }

        public void DecreaseAttackCooldownPerLevel()
        {
            float decreaseAmount = attackCooldown * cooldownDecreasePercentage;
            float newCooldown = Mathf.Max(attackCooldown - decreaseAmount, minimumAttackCooldown);
            AdjustAttackSpeedAndAnimation(newCooldown);
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
        AdjustAttackSpeedAndAnimation(attackCooldown);
    }

    public void Die()
        {
            isMoving = false;
            if (playerAnimator != null)
            {
                playerAnimator.SetTrigger("dkDeath");
            }
            gameManager.GoToMainMenu();
        }
    }