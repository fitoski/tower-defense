using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance;
    public GameObject enemyPrefab;
    public Transform protectionObject;
    public float initialSpawnInterval = 5f;
    public int initialEnemiesPerWave = 5;
    public float waveDelay = 10f;
    public int enemiesIncreasePerWave = 1;

    private float spawnInterval;
    private int enemiesPerWave;
    private float spawnTimer;
    private int enemiesToSpawn;
    private int activeEnemies = 0;

    private int currentEnemyCountPerSpawn = 1;

    private int currentWaveNumber = 1;
    public bool allowSpawn = true;

    public GameObject EnemyFastPrefab;
    public GameObject enemy4Prefab;
    public GameObject enemy5Prefab;
    public GameObject enemy6Prefab;
    public GameObject enemy7Prefab;
    public GameObject enemy8Prefab;
    public GameObject EnemyTankPrefab;
    public GameObject Boss1Prefab;
    public GameObject Boss2Prefab;

    private bool bossSpawnedThisWave = false;

    private Dictionary<int, List<GameObject>> waveEnemyTypes = new Dictionary<int, List<GameObject>>();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public int ActiveEnemies
    {
        get { return activeEnemies; }
        set { activeEnemies = Mathf.Max(0, value); } 
    }

    void Start()
    {
        spawnInterval = initialSpawnInterval;
        enemiesPerWave = initialEnemiesPerWave;
        spawnTimer = spawnInterval;
        enemiesToSpawn = enemiesPerWave;
        waveEnemyTypes.Add(1, new List<GameObject> { enemyPrefab });
        waveEnemyTypes.Add(2, new List<GameObject> { enemyPrefab, EnemyFastPrefab });
        waveEnemyTypes.Add(3, new List<GameObject> { enemyPrefab, EnemyFastPrefab, EnemyTankPrefab });
        waveEnemyTypes.Add(4, new List<GameObject> { enemyPrefab, EnemyFastPrefab, EnemyTankPrefab, enemy4Prefab });
        waveEnemyTypes.Add(5, new List<GameObject> { Boss1Prefab, enemyPrefab, EnemyFastPrefab, EnemyTankPrefab,enemy4Prefab });
        waveEnemyTypes.Add(6, new List<GameObject> { enemyPrefab, EnemyFastPrefab, EnemyTankPrefab, enemy4Prefab, enemy5Prefab });
        waveEnemyTypes.Add(7, new List<GameObject> { enemyPrefab, EnemyFastPrefab, EnemyTankPrefab, enemy4Prefab, enemy5Prefab, enemy6Prefab });
        waveEnemyTypes.Add(8, new List<GameObject> { enemyPrefab, EnemyFastPrefab, EnemyTankPrefab, enemy4Prefab, enemy5Prefab, enemy6Prefab, enemy7Prefab });
        waveEnemyTypes.Add(9, new List<GameObject> { enemyPrefab, EnemyFastPrefab, EnemyTankPrefab, enemy4Prefab, enemy5Prefab, enemy6Prefab, enemy7Prefab, enemy8Prefab });
        waveEnemyTypes.Add(10, new List<GameObject> { Boss2Prefab, enemyPrefab, EnemyFastPrefab, EnemyTankPrefab, enemy4Prefab, enemy5Prefab, enemy6Prefab, enemy7Prefab, enemy8Prefab });
        // 15 - 20 - 25 so on
    }

    void Update()
    {
        if (allowSpawn) 
        {
            if (spawnTimer <= 0f && enemiesToSpawn > 0)
            {
                for (int i = 0; i < currentEnemyCountPerSpawn; i++)
                {
                    SpawnEnemy();
                }
                spawnTimer = spawnInterval;
                enemiesToSpawn--;
            }
            else if (spawnTimer <= 0f && enemiesToSpawn == 0)
            {
                if (!IsInvoking("StartNextWave"))
                {
                    Invoke("StartNextWave", waveDelay);
                }
            }
        }

        spawnTimer -= Time.deltaTime;
    }

    void SpawnEnemy()
    {
        if (!allowSpawn) return;

        Vector2 spawnPosition = GetRandomSpawnPosition();

        List<GameObject> enemiesForThisWave;
        if (waveEnemyTypes.ContainsKey(currentWaveNumber))
        {
            enemiesForThisWave = waveEnemyTypes[currentWaveNumber];
        }
        else
        {
            enemiesForThisWave = GetWaveEnemies(currentWaveNumber);
        }

        GameObject chosenEnemyPrefab;
        if (currentWaveNumber % 5 == 0 && !bossSpawnedThisWave)
        {
            if (currentWaveNumber == 5)
                chosenEnemyPrefab = Boss1Prefab;
            else
                chosenEnemyPrefab = Boss2Prefab;
            bossSpawnedThisWave = true;
        }
        else
        {
            enemiesForThisWave.RemoveAll(e => e == Boss1Prefab || e == Boss2Prefab);
            chosenEnemyPrefab = enemiesForThisWave[Random.Range(0, enemiesForThisWave.Count)];
        }

        GameObject enemy = Instantiate(chosenEnemyPrefab, spawnPosition, Quaternion.identity);
        EnemyMovement enemyMovement = enemy.GetComponent<EnemyMovement>();
        if (enemyMovement != null)
            enemyMovement.SetTarget(protectionObject);

        activeEnemies++;
    }

    void StartNextWave()
    {
        if (activeEnemies > 0) return;

        bossSpawnedThisWave = false;

        int nextWave = currentWaveNumber + 1;

        if (GetWaveEnemies(nextWave).Count == 0) 
        {
            return;
        }

        currentWaveNumber = nextWave;

        if (currentWaveNumber % 5 == 0)
        {
            enemiesToSpawn = enemiesPerWave + 1; 
        }

        else
        {
            enemiesPerWave += enemiesIncreasePerWave;
            spawnInterval *= 0.9f;
            if (currentEnemyCountPerSpawn < 20) currentEnemyCountPerSpawn++;
        }

        enemiesToSpawn = enemiesPerWave;
    }

    public int GetCurrentWaveNumber()
    {
        return currentWaveNumber;
    }

    public void SetSpawnPermission(bool permission)
    {
        allowSpawn = permission;
    }

    public void IntroduceNewEnemy(int maxHealth, float speed, int baseDamage, int scoreValue, int experiencePointsValue, float damageMultiplier)
    {
        GameObject enemy;
        // FAST ENEMY 
        enemy = Instantiate(EnemyFastPrefab, GetRandomSpawnPosition(), Quaternion.identity);
        EnemyFast enemyFastComponent = enemy.GetComponent<EnemyFast>();
        if (enemyFastComponent != null)
        {
            enemyFastComponent.maxHealth = maxHealth;
            enemyFastComponent.speed = speed;
            enemyFastComponent.baseDamage = baseDamage;
            enemyFastComponent.scoreValue = scoreValue;
            enemyFastComponent.experiencePointsValue = experiencePointsValue;
            enemyFastComponent.damageMultiplierPerWave = damageMultiplier;
        }

        // TANK ENEMY
        enemy = Instantiate(EnemyTankPrefab, GetRandomSpawnPosition(), Quaternion.identity);
        EnemyTank enemyTankComponent = enemy.GetComponent<EnemyTank>();
        if (enemyTankComponent != null)
        {
            enemyTankComponent.maxHealth = maxHealth;
            enemyTankComponent.speed = speed;
            enemyTankComponent.baseDamage = baseDamage;
            enemyTankComponent.scoreValue = scoreValue;
            enemyTankComponent.experiencePointsValue = experiencePointsValue;
            enemyTankComponent.damageMultiplierPerWave = damageMultiplier;
        }

        // ENEMY4 
        enemy = Instantiate(enemy4Prefab, GetRandomSpawnPosition(), Quaternion.identity);
        Enemy4 enemy4Component = enemy.GetComponent<Enemy4>();
        if (enemy4Component != null)
        {
            enemy4Component.maxHealth = maxHealth;
            enemy4Component.speed = speed;
            enemy4Component.baseDamage = baseDamage;
            enemy4Component.scoreValue = scoreValue;
            enemy4Component.experiencePointsValue = experiencePointsValue;
            enemy4Component.damageMultiplierPerWave = damageMultiplier;
        }

        // ENEMY5
        enemy = Instantiate(enemy5Prefab, GetRandomSpawnPosition(), Quaternion.identity);
        Enemy5 enemy5Component = enemy.GetComponent<Enemy5>();
        if (enemy5Component != null)
        {
            enemy5Component.maxHealth = maxHealth;
            enemy5Component.speed = speed;
            enemy5Component.baseDamage = baseDamage;
            enemy5Component.scoreValue = scoreValue;
            enemy5Component.experiencePointsValue = experiencePointsValue;
            enemy5Component.damageMultiplierPerWave = damageMultiplier;
        }

        // ENEMY6 
        enemy = Instantiate(enemy6Prefab, GetRandomSpawnPosition(), Quaternion.identity);
        Enemy6 enemy6Component = enemy.GetComponent<Enemy6>();
        if (enemy6Component != null)
        {
            enemy6Component.maxHealth = maxHealth;
            enemy6Component.speed = speed;
            enemy6Component.baseDamage = baseDamage;
            enemy6Component.scoreValue = scoreValue;
            enemy6Component.experiencePointsValue = experiencePointsValue;
            enemy6Component.damageMultiplierPerWave = damageMultiplier;
        }

        // ENEMY7 
        enemy = Instantiate(enemy7Prefab, GetRandomSpawnPosition(), Quaternion.identity);
        Enemy7 enemy7Component = enemy.GetComponent<Enemy7>();
        if (enemy7Component != null)
        {
            enemy7Component.maxHealth = maxHealth;
            enemy7Component.speed = speed;
            enemy7Component.baseDamage = baseDamage;
            enemy7Component.scoreValue = scoreValue;
            enemy7Component.experiencePointsValue = experiencePointsValue;
            enemy7Component.damageMultiplierPerWave = damageMultiplier;
        }

        // ENEMY8 
        enemy = Instantiate(enemy8Prefab, GetRandomSpawnPosition(), Quaternion.identity);
        Enemy8 enemy8Component = enemy.GetComponent<Enemy8>();
        if (enemy8Component != null)
        {
            enemy8Component.maxHealth = maxHealth;
            enemy8Component.speed = speed;
            enemy8Component.baseDamage = baseDamage;
            enemy8Component.scoreValue = scoreValue;
            enemy8Component.experiencePointsValue = experiencePointsValue;
            enemy8Component.damageMultiplierPerWave = damageMultiplier;
        }

        EnemyMovement enemyMovement = enemy.GetComponent<EnemyMovement>();
        if (enemyMovement != null)
            enemyMovement.SetTarget(protectionObject);
    }
    private Vector2 GetRandomSpawnPosition()
    {
        Vector2 spawnPosition = (Vector2)transform.position + Random.insideUnitCircle * 30;

        if (Vector2.Distance(transform.position, spawnPosition) < 10f)
        {
            return GetRandomSpawnPosition();
        }

        return spawnPosition;
    }

    private List<GameObject> GetWaveEnemies(int waveNumber)
    {
        List<GameObject> waveEnemies = new List<GameObject>();

        waveEnemies.Add(enemyPrefab);

        if (waveNumber >= 4)
        {
            waveEnemies.Add(EnemyFastPrefab);
        }

        if (waveNumber >= 6)
        {
            waveEnemies.Add(EnemyTankPrefab);
        }

        if (waveNumber % 5 == 0)
        {
            if (waveNumber == 5) waveEnemies.Add(Boss1Prefab);
            else if (waveNumber == 10) waveEnemies.Add(Boss2Prefab);
        }

        return waveEnemies;
    }
}