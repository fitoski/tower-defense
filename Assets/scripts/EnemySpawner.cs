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

    private int currentEnemyCountPerSpawn = 1;

    private int currentWaveNumber = 1;
    public bool allowSpawn = true;

    public GameObject EnemyFastPrefab;
    public GameObject EnemyTankPrefab;
    public GameObject Boss1Prefab;
    public GameObject Boss2Prefab;
    private bool bossSpawnedThisWave = false;

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
        waveEnemyTypes.Add(5, new List<GameObject> { Boss1Prefab, enemyPrefab, EnemyFastPrefab, EnemyTankPrefab });
        waveEnemyTypes.Add(10, new List<GameObject> { Boss2Prefab, enemyPrefab, EnemyFastPrefab, EnemyTankPrefab });
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
    }



    //public void StartWave(int waveNumber)
    //{
    //    if (waveEnemyTypes.ContainsKey(waveNumber))
    //    {
    //        List<GameObject> enemiesInThisWave = waveEnemyTypes[waveNumber];
    //        for (int i = 0; i < enemiesPerWave + 2; i++)
    //        {
    //            Vector2 spawnPosition = GetRandomSpawnPosition();
    //            GameObject chosenEnemyPrefab = enemiesInThisWave[Random.Range(0, enemiesInThisWave.Count)];
    //            GameObject spawnedEnemy = Instantiate(chosenEnemyPrefab, spawnPosition, Quaternion.identity);
    //        }
    //    }
    //    else
    //    {
    //        Debug.LogWarning("Wave " + waveNumber + " için belirlenmiş düşman türü yok.");
    //    }
    //}

    void StartNextWave()
    {
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
        EnemyMovement enemyMovement = enemy.GetComponent<EnemyMovement>();
        if (enemyMovement != null)
            enemyMovement.SetTarget(protectionObject);
    }
    private Vector2 GetRandomSpawnPosition()
    {
        Vector2 spawnPosition = (Vector2)transform.position + Random.insideUnitCircle * 30;

        if (Vector2.Distance(transform.position, spawnPosition) < 10f)
        {
            Debug.Log("dasdasdsa");
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