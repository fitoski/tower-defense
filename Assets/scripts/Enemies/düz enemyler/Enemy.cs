using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Enemy : MonoBehaviour
{
    protected bool isDead = false;
    [Header("Attributes")]
    public int maxHealth;
    public int baseDamage = 1;
    public int baseMaxHealth = 60;
    public float healthMultiplierPerWave = 1.3f;
    public float damageMultiplierPerWave = 1.5f;
    protected int currentHealth;
    protected EnemyMovement enemyMovement;
    public float speed = 2f;
    private float originalSpeed;
    public int scoreValue = 10;
    public int experiencePointsValue = 5;
    private Vector2 previousPosition;
    private float movementThreshold = 0.01f;
    private Animator animator;
    public GameObject floatingTextPrefab;
    public Transform canvasTransform;
    public GameObject experiencePrefab;
    public int goldValue = 1;
    public static int bossesKilled = 0;

    protected virtual void Start()
    {
        int currentWave = EnemySpawner.Instance.GetCurrentWaveNumber();
        maxHealth = Mathf.RoundToInt(baseMaxHealth * Mathf.Pow(healthMultiplierPerWave, currentWave - 1));
        currentHealth = maxHealth;
        originalSpeed = speed; 
        animator = GetComponent<Animator>();
        enemyMovement = GetComponent<EnemyMovement>();
        previousPosition = transform.position;
        GameObject canvasObject = GameObject.FindWithTag("FloatingTextCanvas");
        if (canvasObject != null)
        {
            canvasTransform = canvasObject.transform;
        }
        else
        {
            Debug.LogError("Canvas for floating text not found.");
        }
        if (floatingTextPrefab == null)
        {
            Debug.LogError("FloatingTextPrefab is not assigned.");
        }
    }

    protected virtual void Update()
    {
        Vector2 currentPosition = transform.position;
        bool isEnemyMoving = Vector2.Distance(previousPosition, currentPosition) > movementThreshold;
        animator.SetBool("isMoving", isEnemyMoving);
        previousPosition = currentPosition;

        if (Time.time >= nextBurnDamageTime && Time.time <= burnStopTime)
        {
            TakeDamage(burnDamageAmount);
            nextBurnDamageTime = Time.time + burningInterval;
        }

        else
        {
            burnDamageAmount = 0;
            burningInterval = 999;
            nextBurnDamageTime = 0;
            burnStopTime = 0;
        }
    }

    public void TakeDamage(int damage, bool isCriticalHit = false)
    {
        if (isDead)
        {
            Debug.Log(gameObject.name + " is already dead.");
            return;
        }

        currentHealth -= damage;
        Debug.Log(gameObject.name + " took damage, new health: " + currentHealth);

        if (floatingTextPrefab != null && canvasTransform != null)
        {
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
            Vector3 offset = new Vector3(0, 50, 0);
            GameObject floatingText = Instantiate(floatingTextPrefab, screenPosition + offset, Quaternion.identity, canvasTransform);
            floatingText.GetComponent<FloatingText>().SetText(damage.ToString(), isCriticalHit ? Color.yellow : Color.red);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public bool IsDead
    {
        get { return isDead; }
    }

    public int CurrentHealth
    {
        get { return currentHealth; }
    }

    public int GetScoreValue()
    {
        return scoreValue;
    }

    protected virtual void Die()
    {
        if (isDead) return;

        isDead = true;
        animator.SetTrigger("Die");
        animator.SetBool("isDead", true);
        GetComponent<Collider2D>().enabled = false;

        if (this is Boss)
        {
            bossesKilled++;
        }

        if (enemyMovement != null)
        {
            enemyMovement.StopMovement();
        }

        if (experiencePrefab != null)
        {
            Instantiate(experiencePrefab, transform.position, Quaternion.identity);
        }

        GameManager.main.IncreaseGold(goldValue);
        EnemySpawner.Instance.ActiveEnemies--;
        EnemySpawner.Instance.EnemyKilled();

        StartCoroutine(RemoveEnemyAfterDelay());
    }

    private IEnumerator RemoveEnemyAfterDelay()
    {
        yield return new WaitForSeconds(1.0f);

        Destroy(gameObject);
    }

    // FIRE TURRET EFFECTS
    public int burnDamageAmount;
    private float burningInterval;
    private float nextBurnDamageTime = 0;
    private float burnStopTime;
    public void StartBurning(int damageAmount, float interval, float duration)
    {
        Debug.Log($"Enemy {gameObject.name}: Starting to burn with {damageAmount} damage every {interval} seconds for {duration} seconds.");

        StopCoroutine("ApplyBurningDamage");
        StartCoroutine(ApplyBurningDamage(damageAmount, interval, duration));
    }
    private IEnumerator ApplyBurningDamage(int damageAmount, float interval, float duration)
    {
        float endTime = Time.time + duration;

        while (Time.time < endTime)
        {
            Debug.Log($"Enemy {gameObject.name}: Burn damage applied: {damageAmount}");

            TakeDamage(damageAmount);
            yield return new WaitForSeconds(interval);
        }
    }

    // ICE TURRET EFFECTS
    public void SlowDown(float slowFactor, float duration)
    {
        float newSpeed = speed * slowFactor;
        if (newSpeed < originalSpeed * 0.1f)
        {
            slowFactor = originalSpeed * 0.1f / speed;
        }
        StartCoroutine(SlowCoroutine(slowFactor, duration));
    }
    private IEnumerator SlowCoroutine(float slowFactor, float duration)
    {
        float originalSpeed = speed;
        speed *= slowFactor;

        yield return new WaitForSeconds(duration);

        speed = originalSpeed;
    }

    // WIND TURRET EFFECTS
    public void ApplyKnockback(Vector2 direction, float force)
    {
        StartCoroutine(KnockbackCoroutine(direction, force));
    }
    private IEnumerator KnockbackCoroutine(Vector2 direction, float force)
    {
        float duration = 0.2f;
        float timer = 0;

        while (timer < duration)
        {
            transform.position += (Vector3)(direction * force * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }
    }

    // ELECTRIC TURRET EFFECTS
    public void Stun(float duration)
    {
        StartCoroutine(StunCoroutine(duration));
    }

    private IEnumerator StunCoroutine(float duration)
    {
        enemyMovement.enabled = false;
        yield return new WaitForSeconds(duration);
        enemyMovement.enabled = true;
    }

    public void OnDeathAnimationComplete()
    {
        Destroy(gameObject);
    }
}