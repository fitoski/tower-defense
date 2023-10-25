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
    public float attackCooldown = 0.5f; 
    private float attackCooldownTimer = 0f;  
    public int playerDamage = 5;
    private bool isInvulnerable = false; 
    public float invulnerabilityDuration = 1.5f;
    public float armorValue;
    public float flashDuration = 0.2f;
    public Color flashColor = Color.red;
    private Color originalColor;
    private SpriteRenderer playerSpriteRenderer;
    public SpriteRenderer swordSpriteRenderer;
    public List<Sprite> weaponSprites;

    private bool isMoving = true;

    public int maxPlayerHealth = 100;
    public int playerHealth = 100;

    public Image playerHealthBar;
    private EnemySpawner enemySpawner;
    public Text waveDisplayText;

    private GameManager gameManager;

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

    }

    void Update()
    {
        if (isMoving)
        {
            float moveInputX = Input.GetAxisRaw("Horizontal");
            float moveInputY = Input.GetAxisRaw("Vertical");
            Vector2 movement = new Vector2(moveInputX, moveInputY).normalized * moveSpeed * Time.deltaTime;
            transform.Translate(movement, Space.World);

            Vector3 playerPosition = transform.position;

         //playerPosition.x = Mathf.Clamp(playerPosition.x, minX, maxX);
         //playerPosition.y = Mathf.Clamp(playerPosition.y, minY, maxY);
         //transform.position = playerPosition;

            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (mousePosition - (Vector2)transform.position).normalized;

            sword.position = (Vector2)transform.position + direction * orbitRadius;
            sword.up = direction;
            sword.RotateAround(transform.position, Vector3.forward, orbitSpeed * Time.deltaTime);

            if (Input.GetButton("Fire1") && attackCooldownTimer <= 0f)
            {
                sword.Rotate(Vector3.forward, attackRotationAmount);
                attackCooldownTimer = attackCooldown;
                Attack();
            }

            if (attackCooldownTimer > 0f)
            {
                attackCooldownTimer -= Time.deltaTime;
            }
        }
        if (!isInvulnerable)
        {
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, 0.5f);
            foreach (Collider2D enemy in hitEnemies)
            {
                if (enemy.CompareTag("Enemy"))
                {
                    TakeDamage();
                    break;
                }
            }
        }
        if (waveDisplayText != null && enemySpawner != null)
        {
            waveDisplayText.text = "Wave: " + enemySpawner.GetCurrentWaveNumber().ToString();
        }
    }


    void Attack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, 2f);

        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                Enemy enemyComponent = enemy.GetComponent<Enemy>();
                if (enemyComponent != null)
                {
                    enemyComponent.TakeDamage(playerDamage);
                }
            }
        }
    }

    void TakeDamage()
    {
        if (!isInvulnerable)
        {
            playerHealth -= 10;
            UpdateHealthBars();
            StartCoroutine(FlashPlayer());
            StartCoroutine(PausePlayer());
            if (playerHealth <= 0)
            {
                
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

    public void IncreaseDamagePerLevel(int increaseAmount)
    {
        playerDamage += increaseAmount;
    }

    public void IncreaseMoveSpeedPerLevel(float speedIncrase)
    {
        moveSpeed += speedIncrase;
    }

    public void DecreaseAttackCooldownPerLevel(float cooldownDecrease)
    {
        attackCooldown -= cooldownDecrease;
    }

    public void IncreaseMaxHealthPerLevel(int healthIncrease)
    {
        maxPlayerHealth += healthIncrease;
        playerHealth = maxPlayerHealth;
        UpdateHealthBars();
    }

    public void ChangeArmor(float newArmorValue)
    {
        armorValue = newArmorValue;
    }

    public void ChangeWeapon(int newDamage, float newRange, int weaponIndex)
    {
        playerDamage = newDamage;
        orbitRadius = newRange;
        swordSpriteRenderer.sprite = weaponSprites[weaponIndex];
    }

}