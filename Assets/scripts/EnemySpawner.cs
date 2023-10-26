using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
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

    private int currentWaveNumber = 1;
    public bool allowSpawn = true;

    public GameObject EnemyFastPrefab;
    public GameObject EnemyTankPrefab;

    private Dictionary<int, List<GameObject>> waveEnemyTypes = new Dictionary<int, List<GameObject>>();

    void Start()
    {
        spawnInterval = initialSpawnInterval;
        enemiesPerWave = initialEnemiesPerWave;
        spawnTimer = spawnInterval;
        enemiesToSpawn = enemiesPerWave;
        waveEnemyTypes.Add(1, new List<GameObject> { enemyPrefab });
        waveEnemyTypes.Add(2, new List<GameObject> { enemyPrefab, EnemyFastPrefab });
        waveEnemyTypes.Add(3, new List<GameObject> { enemyPrefab, EnemyFastPrefab, EnemyTankPrefab });
    }

    void Update()
    {
        if (allowSpawn) 
        {
            if (spawnTimer <= 0f && enemiesToSpawn > 0)
            {
                SpawnEnemy();
                spawnTimer = spawnInterval;
                enemiesToSpawn--;
            }
            else if (spawnTimer <= 0f && enemiesToSpawn == 0)
            {
                Invoke("StartNextWave", waveDelay);
                spawnTimer = spawnInterval;
            }
        }

        spawnTimer -= Time.deltaTime;
    }

    void SpawnEnemy()
    {
        if (!allowSpawn) return;

        Vector2 spawnPosition = GetRandomSpawnPosition();

        GameObject chosenEnemyPrefab = waveEnemyTypes[currentWaveNumber][Random.Range(0, waveEnemyTypes[currentWaveNumber].Count)];

        GameObject enemy = Instantiate(chosenEnemyPrefab, spawnPosition, Quaternion.identity);
        EnemyMovement enemyMovement = enemy.GetComponent<EnemyMovement>();
        if (enemyMovement != null)
            enemyMovement.SetTarget(protectionObject);
    }

    public void StartWave(int waveNumber)
    {
        if (waveEnemyTypes.ContainsKey(waveNumber))
        {
            List<GameObject> enemiesInThisWave = waveEnemyTypes[waveNumber];
            for (int i = 0; i < enemiesPerWave; i++)
            {
                Vector2 spawnPosition = GetRandomSpawnPosition();
                GameObject chosenEnemyPrefab = enemiesInThisWave[Random.Range(0, enemiesInThisWave.Count)];
                GameObject spawnedEnemy = Instantiate(chosenEnemyPrefab, spawnPosition, Quaternion.identity);
            }
        }
        else
        {
            Debug.LogWarning("Wave " + waveNumber + " için belirlenmiş düşman türü yok.");
        }
    }

    void StartNextWave()
    {
        currentWaveNumber++;

        enemiesPerWave += enemiesIncreasePerWave;
        enemiesToSpawn = enemiesPerWave;
        spawnInterval *= 0.9f;
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
        EnemyMovement enemyMovement = enemy.GetComponent<EnemyMovement>();
        if (enemyMovement != null)
            enemyMovement.SetTarget(protectionObject);
    }
    private Vector2 GetRandomSpawnPosition()
    {
        return (Vector2)transform.position + Random.insideUnitCircle * 15f;
    }
}