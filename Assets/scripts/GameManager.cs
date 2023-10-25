using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Enums;

public class GameManager : MonoBehaviour
{
    public static GameManager main;
    public int currency;
    public int level;
    public int experiencePoints;

    public int turretCost = 50;  
    public int wallCost = 30;

    private Vector3 selectedNodePosition;

    private Rigidbody2D rb;

    public GameObject traderPrefab;
    public TraderUIManager traderUIManager;

    private void Awake()
    {
        main = this;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currency = 0;
        level = 1;
        experiencePoints = 0;
        InvokeRepeating("SpawnTrader", 5f, Random.Range(20f, 40f));
        traderUIManager.Awake();
        traderUIManager = TraderUIManager.instance;
        Debug.Log(traderUIManager == null ? "TraderUIManager is null" : "TraderUIManager found");
    }

    public void IncreaseCurrency(int amount)
    {
        currency += amount;
    }

    public bool SpendCurrency(int amount)
    {
        if (amount <= currency)
        {
            currency -= amount;
            return true;
        }
        else
        {
            Debug.Log("Not enough currency.");
            return false;
        }
    }

    public bool HasEnoughCurrency(int amount)
    {
        return currency >= amount;
    }

    public void SetSelectedNodePosition(Vector3 position)
    {
        selectedNodePosition = position;
    }

    public Vector3 GetSelectedNodePosition()
    {
        return selectedNodePosition;
    }

    public void ClearSelectedNodePosition()
    {
        selectedNodePosition = Vector3.zero;
    }

    public int GetTurretCost()
    {
        return turretCost;
    }

    public int GetWallCost()
    {
        return wallCost;
    }

    public void IncreaseExperiencePoints(int amount)
    {
        experiencePoints += amount;
        CheckLevelUp();
    }

    void LevelUp()
    {
        PlayerMovement playerScript = FindObjectOfType<PlayerMovement>();

        if (playerScript != null)
        {
            playerScript.IncreaseDamagePerLevel(2);
            playerScript.IncreaseMoveSpeedPerLevel(1f);
            playerScript.DecreaseAttackCooldownPerLevel(0.1f);
            playerScript.IncreaseMaxHealthPerLevel(20);
        }
    }

    public void CheckLevelUp()
    {
        int requiredExperience = Mathf.FloorToInt(10 * Mathf.Pow(1.7f, level - 1));

        if (experiencePoints >= requiredExperience)
        {
            level++;
            experiencePoints = 0;
            IncreasePlayerStatsOnLevelUp();
        }
    }

    public int GetCurrentLevel()
    {
        return level;
    }

    public int GetCurrentExperiencePoints()
    {
        return experiencePoints;
    }

    public void IncreasePlayerStatsOnLevelUp()
    {
        PlayerMovement playerScript = FindObjectOfType<PlayerMovement>();
        if (playerScript != null)
        {
            playerScript.IncreaseDamagePerLevel(2);
            playerScript.IncreaseMoveSpeedPerLevel(1f);
            playerScript.DecreaseAttackCooldownPerLevel(0.1f);
            playerScript.IncreaseMaxHealthPerLevel(20);
        }
    }

    public void SpawnTrader()
    {
        GameObject trader = Instantiate(traderPrefab, new Vector2(Random.Range(-10, 10), Random.Range(-10, 10)), Quaternion.identity);
        Trader traderScript = trader.GetComponent<Trader>();
        if (traderScript != null)
        {
            traderScript.traderUIManager = TraderUIManager.instance;
        }
        Debug.Log("A Trader has spawned!");
        Debug.Log("Trader spawned at: " + trader.transform.position);
    }
}