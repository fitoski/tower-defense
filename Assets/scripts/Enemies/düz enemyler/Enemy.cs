using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Enemy : MonoBehaviour
{
    private bool isDead = false;
    [Header("Attributes")]
    public int maxHealth;
    public int baseDamage = 1;
    public int baseMaxHealth = 60;
    public float healthMultiplierPerWave = 1.3f;
    public float damageMultiplierPerWave = 1.5f;
    protected int currentHealth;
    protected EnemyMovement enemyMovement;
    public float speed;
    public int scoreValue = 10;
    public int experiencePointsValue = 5;
    private Vector2 previousPosition;
    private float movementThreshold = 0.01f;
    private Animator animator;
    public GameObject floatingTextPrefab;
    public Transform canvasTransform;
    public GameObject experiencePrefab;
    public int goldValue = 1;

    protected void Start()
    {
        int currentWave = EnemySpawner.Instance.GetCurrentWaveNumber();
        maxHealth = Mathf.RoundToInt(baseMaxHealth * Mathf.Pow(healthMultiplierPerWave, currentWave - 1));
        currentHealth = maxHealth;
        speed = 2f;
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

    //void OnTriggerEnter2D(Collider2D collider)
    //{
    //    if (collider.gameObject.CompareTag("Core") && !isDead)
    //    {
    //        Die();
    //    }
    //}

    public void TakeDamage(int damage)
    {
        if (isDead)
        {
            Debug.Log(gameObject.name + " zaten ölü.");

            return;
        }

        currentHealth -= damage;
        Debug.Log(gameObject.name + " hasar aldı, yeni canı: " + currentHealth);

        if (floatingTextPrefab != null && canvasTransform != null)
        {
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
            Vector3 offset = new Vector3(0, 50, 0);
            GameObject floatingText = Instantiate(floatingTextPrefab, screenPosition + offset, Quaternion.identity, canvasTransform);
            floatingText.GetComponent<FloatingText>().SetText(damage.ToString());
        }

        else
        {
            Debug.LogError("FloatingTextPrefab or CanvasTransform is null.");
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

    public int burnDamageAmount;
    private float burningInterval;
    private float nextBurnDamageTime = 0;
    private float burnStopTime;

    public void StartBurning(int damageAmount,float burningInterval, float burnTime)
    {
        if (burnDamageAmount < damageAmount)
        {
            burnDamageAmount = damageAmount;
            this.burningInterval = burningInterval;
        }

        burnStopTime = Time.time + burningInterval;
    }

    public void OnDeathAnimationComplete()
    {
        Destroy(gameObject);
    }
}