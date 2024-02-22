using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance;
    public Transform protectionObject;
    public float initialSpawnInterval = 5f;
    public int initialEnemiesPerWave = 5;
    public float waveDelay = 10f;
    public int enemiesIncreasePerWave = 1;
    private int totalEnemiesKilled = 0;
    public TextMeshProUGUI enemyCountText;
    private const float waveDuration = 60f;
    private float spawnInterval;
    private int enemiesPerWave;
    private float spawnTimer;
    private int enemiesToSpawn;
    private int activeEnemies = 0;
    private float waveStartTime;
    private int currentEnemyCountPerSpawn = 1;
    private int currentWaveNumber = 1;
    public bool allowSpawn = true;
    private float nextWaveTime;

    public GameObject enemyDefaultPrefab;
    public GameObject EnemyFastPrefab;
    public GameObject enemy4Prefab;
    public GameObject enemy5Prefab;
    public GameObject enemy6Prefab;
    public GameObject enemy7Prefab;
    public GameObject enemy8Prefab;
    public GameObject enemy9Prefab;
    public GameObject enemy10Prefab;
    public GameObject enemy11Prefab;
    public GameObject enemy12Prefab;
    public GameObject enemy13Prefab;
    public GameObject enemy14Prefab;
    public GameObject enemy15Prefab;
    public GameObject enemy16Prefab;
    public GameObject enemy17Prefab;
    public GameObject enemy18Prefab;
    public GameObject enemy19Prefab;
    public GameObject enemy20Prefab;
    public GameObject enemy21Prefab;
    public GameObject enemy22Prefab;
    public GameObject enemy23Prefab;
    public GameObject enemy24Prefab;
    public GameObject enemy25Prefab;


    public GameObject Boss1Prefab, Boss2Prefab, Boss3Prefab, Boss4Prefab, Boss5Prefab;

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
        InitializeWaveEnemyTypes();
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
    }

    private void InitializeWaveEnemyTypes()
    {
        waveEnemyTypes = new Dictionary<int, List<GameObject>>();

        waveEnemyTypes.Add(1, new List<GameObject> { enemyDefaultPrefab });
        waveEnemyTypes.Add(2, new List<GameObject> { enemyDefaultPrefab, EnemyFastPrefab });
        waveEnemyTypes.Add(3, new List<GameObject> { enemyDefaultPrefab, EnemyFastPrefab, enemy4Prefab });
        waveEnemyTypes.Add(4, new List<GameObject> { enemyDefaultPrefab, EnemyFastPrefab, enemy4Prefab, enemy5Prefab });
        waveEnemyTypes.Add(5, new List<GameObject> { enemyDefaultPrefab, EnemyFastPrefab, enemy4Prefab, enemy5Prefab, enemy6Prefab });
        waveEnemyTypes.Add(6, new List<GameObject> { enemyDefaultPrefab, EnemyFastPrefab, enemy4Prefab, enemy5Prefab, enemy6Prefab, enemy7Prefab });
        waveEnemyTypes.Add(7, new List<GameObject> { enemyDefaultPrefab, EnemyFastPrefab, enemy4Prefab, enemy5Prefab, enemy6Prefab, enemy7Prefab, enemy8Prefab });
        waveEnemyTypes.Add(8, new List<GameObject> { enemyDefaultPrefab, EnemyFastPrefab, enemy4Prefab, enemy5Prefab, enemy6Prefab, enemy7Prefab, enemy8Prefab, enemy9Prefab });
        waveEnemyTypes.Add(9, new List<GameObject> { enemyDefaultPrefab, EnemyFastPrefab, enemy4Prefab, enemy5Prefab, enemy6Prefab, enemy7Prefab, enemy8Prefab, enemy9Prefab, enemy10Prefab });
        waveEnemyTypes.Add(10, new List<GameObject> { enemyDefaultPrefab, EnemyFastPrefab, enemy4Prefab, enemy5Prefab, enemy6Prefab, enemy7Prefab, enemy8Prefab, enemy9Prefab, enemy10Prefab, enemy11Prefab });
        waveEnemyTypes.Add(11, new List<GameObject> { enemyDefaultPrefab, EnemyFastPrefab, enemy4Prefab, enemy5Prefab, enemy6Prefab, enemy7Prefab, enemy8Prefab, enemy9Prefab, enemy10Prefab, enemy11Prefab, enemy12Prefab });
        waveEnemyTypes.Add(12, new List<GameObject> { enemyDefaultPrefab, EnemyFastPrefab, enemy4Prefab, enemy5Prefab, enemy6Prefab, enemy7Prefab, enemy8Prefab, enemy9Prefab, enemy10Prefab, enemy11Prefab, enemy12Prefab, enemy13Prefab });
        waveEnemyTypes.Add(13, new List<GameObject> { enemyDefaultPrefab, EnemyFastPrefab, enemy4Prefab, enemy5Prefab, enemy6Prefab, enemy7Prefab, enemy8Prefab, enemy9Prefab, enemy10Prefab, enemy11Prefab, enemy12Prefab, enemy13Prefab, enemy14Prefab });
        waveEnemyTypes.Add(14, new List<GameObject> { enemyDefaultPrefab, EnemyFastPrefab, enemy4Prefab, enemy5Prefab, enemy6Prefab, enemy7Prefab, enemy8Prefab, enemy9Prefab, enemy10Prefab, enemy11Prefab, enemy12Prefab, enemy13Prefab, enemy14Prefab, enemy15Prefab });
        waveEnemyTypes.Add(15, new List<GameObject> { enemyDefaultPrefab, EnemyFastPrefab, enemy4Prefab, enemy5Prefab, enemy6Prefab, enemy7Prefab, enemy8Prefab, enemy9Prefab, enemy10Prefab, enemy11Prefab, enemy12Prefab, enemy13Prefab, enemy14Prefab, enemy15Prefab, enemy16Prefab });
        waveEnemyTypes.Add(16, new List<GameObject> { enemyDefaultPrefab, EnemyFastPrefab, enemy4Prefab, enemy5Prefab, enemy6Prefab, enemy7Prefab, enemy8Prefab, enemy9Prefab, enemy10Prefab, enemy11Prefab, enemy12Prefab, enemy13Prefab, enemy14Prefab, enemy15Prefab, enemy16Prefab, enemy17Prefab });
        waveEnemyTypes.Add(17, new List<GameObject> { enemyDefaultPrefab, EnemyFastPrefab, enemy4Prefab, enemy5Prefab, enemy6Prefab, enemy7Prefab, enemy8Prefab, enemy9Prefab, enemy10Prefab, enemy11Prefab, enemy12Prefab, enemy13Prefab, enemy14Prefab, enemy15Prefab, enemy16Prefab, enemy17Prefab, enemy18Prefab });
        waveEnemyTypes.Add(18, new List<GameObject> { enemyDefaultPrefab, EnemyFastPrefab, enemy4Prefab, enemy5Prefab, enemy6Prefab, enemy7Prefab, enemy8Prefab, enemy9Prefab, enemy10Prefab, enemy11Prefab, enemy12Prefab, enemy13Prefab, enemy14Prefab, enemy15Prefab, enemy16Prefab, enemy17Prefab, enemy18Prefab, enemy19Prefab });
        waveEnemyTypes.Add(19, new List<GameObject> { enemyDefaultPrefab, EnemyFastPrefab, enemy4Prefab, enemy5Prefab, enemy6Prefab, enemy7Prefab, enemy8Prefab, enemy9Prefab, enemy10Prefab, enemy11Prefab, enemy12Prefab, enemy13Prefab, enemy14Prefab, enemy15Prefab, enemy16Prefab, enemy17Prefab, enemy18Prefab, enemy19Prefab, enemy20Prefab });
        waveEnemyTypes.Add(20, new List<GameObject> { enemyDefaultPrefab, EnemyFastPrefab, enemy4Prefab, enemy5Prefab, enemy6Prefab, enemy7Prefab, enemy8Prefab, enemy9Prefab, enemy10Prefab, enemy11Prefab, enemy12Prefab, enemy13Prefab, enemy14Prefab, enemy15Prefab, enemy16Prefab, enemy17Prefab, enemy18Prefab, enemy19Prefab, enemy20Prefab, enemy21Prefab });
        waveEnemyTypes.Add(21, new List<GameObject> { enemyDefaultPrefab, EnemyFastPrefab, enemy4Prefab, enemy5Prefab, enemy6Prefab, enemy7Prefab, enemy8Prefab, enemy9Prefab, enemy10Prefab, enemy11Prefab, enemy12Prefab, enemy13Prefab, enemy14Prefab, enemy15Prefab, enemy16Prefab, enemy17Prefab, enemy18Prefab, enemy19Prefab, enemy20Prefab, enemy21Prefab, enemy22Prefab });
        waveEnemyTypes.Add(22, new List<GameObject> { enemyDefaultPrefab, EnemyFastPrefab, enemy4Prefab, enemy5Prefab, enemy6Prefab, enemy7Prefab, enemy8Prefab, enemy9Prefab, enemy10Prefab, enemy11Prefab, enemy12Prefab, enemy13Prefab, enemy14Prefab, enemy15Prefab, enemy16Prefab, enemy17Prefab, enemy18Prefab, enemy19Prefab, enemy20Prefab, enemy21Prefab, enemy22Prefab, enemy23Prefab });
        waveEnemyTypes.Add(23, new List<GameObject> { enemyDefaultPrefab, EnemyFastPrefab, enemy4Prefab, enemy5Prefab, enemy6Prefab, enemy7Prefab, enemy8Prefab, enemy9Prefab, enemy10Prefab, enemy11Prefab, enemy12Prefab, enemy13Prefab, enemy14Prefab, enemy15Prefab, enemy16Prefab, enemy17Prefab, enemy18Prefab, enemy19Prefab, enemy20Prefab, enemy21Prefab, enemy22Prefab, enemy23Prefab, enemy24Prefab });
        waveEnemyTypes.Add(24, new List<GameObject> { enemyDefaultPrefab, EnemyFastPrefab, enemy4Prefab, enemy5Prefab, enemy6Prefab, enemy7Prefab, enemy8Prefab, enemy9Prefab, enemy10Prefab, enemy11Prefab, enemy12Prefab, enemy13Prefab, enemy14Prefab, enemy15Prefab, enemy16Prefab, enemy17Prefab, enemy18Prefab, enemy19Prefab, enemy20Prefab, enemy21Prefab, enemy22Prefab, enemy23Prefab, enemy24Prefab, enemy25Prefab });
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
                if (Time.time >= nextWaveTime || AllEnemiesKilled())
                {
                    StartNextWave();
                }
            }
        }
        float currentTime = Time.time - waveStartTime;
        if (!bossSpawnedThisWave && currentTime >= GetBossSpawnTime(currentWaveNumber))
        {
            SpawnBoss(currentWaveNumber);
            bossSpawnedThisWave = true;
        }
        spawnTimer -= Time.deltaTime;
    }

    bool AllEnemiesKilled()
    {
        return activeEnemies == 0 && totalEnemiesKilled >= enemiesPerWave;
    }

    void SpawnEnemy()
    {
        if (!allowSpawn) return;
        Vector2 spawnPosition = GetRandomSpawnPosition();
        List<GameObject> enemiesForThisWave = GetWaveEnemies(currentWaveNumber);
        GameObject chosenEnemyPrefab = enemiesForThisWave[Random.Range(0, enemiesForThisWave.Count)];
        GameObject enemy = Instantiate(chosenEnemyPrefab, spawnPosition, Quaternion.identity);
        EnemyMovement enemyMovement = enemy.GetComponent<EnemyMovement>();
        if (enemyMovement != null)
        {
            enemyMovement.SetTarget(protectionObject);
        }
        activeEnemies++;
        UpdateEnemyCountUI();
    }
   
    void SpawnBoss(int waveNumber)
    {
        GameObject bossPrefab = null;
        switch (waveNumber)
        {
            case 6: 
                bossPrefab = Boss1Prefab;
                break;
            case 12: 
                bossPrefab = Boss2Prefab;
                break;
            case 18:
                bossPrefab = Boss3Prefab;
                break;
            case 24:
                bossPrefab = Boss4Prefab;
                break;
            case 30: 
                bossPrefab = Boss5Prefab;
                break;
        }

        if (bossPrefab != null)
        {
            // Boss'u spawn et
        }
    }

    float GetBossSpawnTime(int waveNumber)
    {
        return waveNumber * 6 * 60; 
    }

    public void EnemyKilled()
    {
        totalEnemiesKilled++;
        UpdateEnemyCountUI();
    }

    public void UpdateEnemyCountUI()
    {
        if (enemyCountText != null)
        {
            enemyCountText.text = "" + totalEnemiesKilled;
        }
    }

    void StartNextWave()
    {
        bossSpawnedThisWave = false;
        currentWaveNumber++;
        int playerLevel = GameManager.main.level;

        int requiredEnemiesKilled = (currentWaveNumber * 10) + (playerLevel * 2);
        enemiesPerWave = Mathf.Min(requiredEnemiesKilled, 100 - activeEnemies);

        spawnInterval *= 0.9f;
        if (currentEnemyCountPerSpawn < 20) currentEnemyCountPerSpawn++;
        enemiesToSpawn = enemiesPerWave;
        waveStartTime = Time.time;
        SetNextWaveTime();

        UpdateEnemyCountUI();
    }

    void SetNextWaveTime()
    {
        nextWaveTime = Time.time + 60f - (Time.time % 60f);
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

        // ENEMY9
        enemy = Instantiate(enemy9Prefab, GetRandomSpawnPosition(), Quaternion.identity);
        Enemy9 enemy9Component = enemy.GetComponent<Enemy9>();
        if (enemy9Component != null)
        {
            enemy9Component.maxHealth = maxHealth;
            enemy9Component.speed = speed;
            enemy9Component.baseDamage = baseDamage;
            enemy9Component.scoreValue = scoreValue;
            enemy9Component.experiencePointsValue = experiencePointsValue;
            enemy9Component.damageMultiplierPerWave = damageMultiplier;
        }

        // ENEMY10
        enemy = Instantiate(enemy10Prefab, GetRandomSpawnPosition(), Quaternion.identity);
        Enemy10 enemy10Component = enemy.GetComponent<Enemy10>();
        if (enemy10Component != null)
        {
            enemy10Component.maxHealth = maxHealth;
            enemy10Component.speed = speed;
            enemy10Component.baseDamage = baseDamage;
            enemy10Component.scoreValue = scoreValue;
            enemy10Component.experiencePointsValue = experiencePointsValue;
            enemy10Component.damageMultiplierPerWave = damageMultiplier;
        }

        // ENEMY11
        enemy = Instantiate(enemy11Prefab, GetRandomSpawnPosition(), Quaternion.identity);
        Enemy11 enemy11Component = enemy.GetComponent<Enemy11>();
        if (enemy11Component != null)
        {
            enemy11Component.maxHealth = maxHealth;
            enemy11Component.speed = speed;
            enemy11Component.baseDamage = baseDamage;
            enemy11Component.scoreValue = scoreValue;
            enemy11Component.experiencePointsValue = experiencePointsValue;
            enemy11Component.damageMultiplierPerWave = damageMultiplier;
        }

        // ENEMY12
        enemy = Instantiate(enemy12Prefab, GetRandomSpawnPosition(), Quaternion.identity);
        Enemy12 enemy12Component = enemy.GetComponent<Enemy12>();
        if (enemy12Component != null)
        {
            enemy12Component.maxHealth = maxHealth;
            enemy12Component.speed = speed;
            enemy12Component.baseDamage = baseDamage;
            enemy12Component.scoreValue = scoreValue;
            enemy12Component.experiencePointsValue = experiencePointsValue;
            enemy12Component.damageMultiplierPerWave = damageMultiplier;
        }

        // ENEMY13
        enemy = Instantiate(enemy13Prefab, GetRandomSpawnPosition(), Quaternion.identity);
        Enemy13 enemy13Component = enemy.GetComponent<Enemy13>();
        if (enemy13Component != null)
        {
            enemy13Component.maxHealth = maxHealth;
            enemy13Component.speed = speed;
            enemy13Component.baseDamage = baseDamage;
            enemy13Component.scoreValue = scoreValue;
            enemy13Component.experiencePointsValue = experiencePointsValue;
            enemy13Component.damageMultiplierPerWave = damageMultiplier;
        }

        // ENEMY14
        enemy = Instantiate(enemy14Prefab, GetRandomSpawnPosition(), Quaternion.identity);
        Enemy14 enemy14Component = enemy.GetComponent<Enemy14>();
        if (enemy14Component != null)
        {
            enemy14Component.maxHealth = maxHealth;
            enemy14Component.speed = speed;
            enemy14Component.baseDamage = baseDamage;
            enemy14Component.scoreValue = scoreValue;
            enemy14Component.experiencePointsValue = experiencePointsValue;
            enemy14Component.damageMultiplierPerWave = damageMultiplier;
        }

        // ENEMY15
        enemy = Instantiate(enemy15Prefab, GetRandomSpawnPosition(), Quaternion.identity);
        Enemy15 enemy15Component = enemy.GetComponent<Enemy15>();
        if (enemy15Component != null)
        {
            enemy15Component.maxHealth = maxHealth;
            enemy15Component.speed = speed;
            enemy15Component.baseDamage = baseDamage;
            enemy15Component.scoreValue = scoreValue;
            enemy15Component.experiencePointsValue = experiencePointsValue;
            enemy15Component.damageMultiplierPerWave = damageMultiplier;
        }

        // ENEMY16
        enemy = Instantiate(enemy16Prefab, GetRandomSpawnPosition(), Quaternion.identity);
        Enemy16 enemy16Component = enemy.GetComponent<Enemy16>();
        if (enemy16Component != null)
        {
            enemy16Component.maxHealth = maxHealth;
            enemy16Component.speed = speed;
            enemy16Component.baseDamage = baseDamage;
            enemy16Component.scoreValue = scoreValue;
            enemy16Component.experiencePointsValue = experiencePointsValue;
            enemy16Component.damageMultiplierPerWave = damageMultiplier;
        }

        // ENEMY17
        enemy = Instantiate(enemy17Prefab, GetRandomSpawnPosition(), Quaternion.identity);
        Enemy17 enemy17Component = enemy.GetComponent<Enemy17>();
        if (enemy17Component != null)
        {
            enemy17Component.maxHealth = maxHealth;
            enemy17Component.speed = speed;
            enemy17Component.baseDamage = baseDamage;
            enemy17Component.scoreValue = scoreValue;
            enemy17Component.experiencePointsValue = experiencePointsValue;
            enemy17Component.damageMultiplierPerWave = damageMultiplier;
        }

        // ENEMY18
        enemy = Instantiate(enemy18Prefab, GetRandomSpawnPosition(), Quaternion.identity);
        Enemy18 enemy18Component = enemy.GetComponent<Enemy18>();
        if (enemy18Component != null)
        {
            enemy18Component.maxHealth = maxHealth;
            enemy18Component.speed = speed;
            enemy18Component.baseDamage = baseDamage;
            enemy18Component.scoreValue = scoreValue;
            enemy18Component.experiencePointsValue = experiencePointsValue;
            enemy18Component.damageMultiplierPerWave = damageMultiplier;
        }

        // ENEMY19
        enemy = Instantiate(enemy19Prefab, GetRandomSpawnPosition(), Quaternion.identity);
        Enemy19 enemy19Component = enemy.GetComponent<Enemy19>();
        if (enemy19Component != null)
        {
            enemy19Component.maxHealth = maxHealth;
            enemy19Component.speed = speed;
            enemy19Component.baseDamage = baseDamage;
            enemy19Component.scoreValue = scoreValue;
            enemy19Component.experiencePointsValue = experiencePointsValue;
            enemy19Component.damageMultiplierPerWave = damageMultiplier;
        }

        // ENEMY20
        enemy = Instantiate(enemy20Prefab, GetRandomSpawnPosition(), Quaternion.identity);
        Enemy20 enemy20Component = enemy.GetComponent<Enemy20>();
        if (enemy20Component != null)
        {
            enemy20Component.maxHealth = maxHealth;
            enemy20Component.speed = speed;
            enemy20Component.baseDamage = baseDamage;
            enemy20Component.scoreValue = scoreValue;
            enemy20Component.experiencePointsValue = experiencePointsValue;
            enemy20Component.damageMultiplierPerWave = damageMultiplier;
        }

        // ENEMY21
        enemy = Instantiate(enemy21Prefab, GetRandomSpawnPosition(), Quaternion.identity);
        Enemy21 enemy21Component = enemy.GetComponent<Enemy21>();
        if (enemy21Component != null)
        {
            enemy21Component.maxHealth = maxHealth;
            enemy21Component.speed = speed;
            enemy21Component.baseDamage = baseDamage;
            enemy21Component.scoreValue = scoreValue;
            enemy21Component.experiencePointsValue = experiencePointsValue;
            enemy21Component.damageMultiplierPerWave = damageMultiplier;
        }

        // ENEMY22
        enemy = Instantiate(enemy22Prefab, GetRandomSpawnPosition(), Quaternion.identity);
        Enemy22 enemy22Component = enemy.GetComponent<Enemy22>();
        if (enemy22Component != null)
        {
            enemy22Component.maxHealth = maxHealth;
            enemy22Component.speed = speed;
            enemy22Component.baseDamage = baseDamage;
            enemy22Component.scoreValue = scoreValue;
            enemy22Component.experiencePointsValue = experiencePointsValue;
            enemy22Component.damageMultiplierPerWave = damageMultiplier;
        }

        // ENEMY23
        enemy = Instantiate(enemy23Prefab, GetRandomSpawnPosition(), Quaternion.identity);
        Enemy23 enemy23Component = enemy.GetComponent<Enemy23>();
        if (enemy23Component != null)
        {
            enemy23Component.maxHealth = maxHealth;
            enemy23Component.speed = speed;
            enemy23Component.baseDamage = baseDamage;
            enemy23Component.scoreValue = scoreValue;
            enemy23Component.experiencePointsValue = experiencePointsValue;
            enemy23Component.damageMultiplierPerWave = damageMultiplier;
        }

        // ENEMY24
        enemy = Instantiate(enemy24Prefab, GetRandomSpawnPosition(), Quaternion.identity);
        Enemy24 enemy24Component = enemy.GetComponent<Enemy24>();
        if (enemy24Component != null)
        {
            enemy24Component.maxHealth = maxHealth;
            enemy24Component.speed = speed;
            enemy24Component.baseDamage = baseDamage;
            enemy24Component.scoreValue = scoreValue;
            enemy24Component.experiencePointsValue = experiencePointsValue;
            enemy24Component.damageMultiplierPerWave = damageMultiplier;
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

        if (waveNumber >= 1) waveEnemies.Add(enemyDefaultPrefab);
        if (waveNumber >= 2) waveEnemies.Add(EnemyFastPrefab);
        if (waveNumber >= 3) waveEnemies.Add(enemy4Prefab);
        if (waveNumber >= 4) waveEnemies.Add(enemy5Prefab);
        if (waveNumber >= 5) waveEnemies.Add(enemy6Prefab);
        if (waveNumber >= 6) waveEnemies.Add(enemy7Prefab);
        if (waveNumber >= 7) waveEnemies.Add(enemy8Prefab);
        if (waveNumber >= 8) waveEnemies.Add(enemy9Prefab);
        if (waveNumber >= 9) waveEnemies.Add(enemy10Prefab);
        if (waveNumber >= 10) waveEnemies.Add(enemy11Prefab);
        if (waveNumber >= 11) waveEnemies.Add(enemy12Prefab);
        if (waveNumber >= 12) waveEnemies.Add(enemy13Prefab);
        if (waveNumber >= 13) waveEnemies.Add(enemy14Prefab);
        if (waveNumber >= 14) waveEnemies.Add(enemy15Prefab);
        if (waveNumber >= 15) waveEnemies.Add(enemy16Prefab);
        if (waveNumber >= 16) waveEnemies.Add(enemy17Prefab);
        if (waveNumber >= 17) waveEnemies.Add(enemy18Prefab);
        if (waveNumber >= 18) waveEnemies.Add(enemy19Prefab);
        if (waveNumber >= 19) waveEnemies.Add(enemy20Prefab);
        if (waveNumber >= 20) waveEnemies.Add(enemy21Prefab);
        if (waveNumber >= 21) waveEnemies.Add(enemy22Prefab);
        if (waveNumber >= 22) waveEnemies.Add(enemy23Prefab);
        if (waveNumber >= 23) waveEnemies.Add(enemy24Prefab);
        if (waveNumber >= 24) waveEnemies.Add(enemy25Prefab);

        return waveEnemies;
    }

}