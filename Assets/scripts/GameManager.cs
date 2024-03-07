using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using static Enums;
using TMPro;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager main;
    public event Action OnGoldChanged;
    public int level;
    public int experiencePoints;
    private float playTime = 0f;
    public TextMeshProUGUI playTimeText;
    public int wallCost = 30;
    public int PlayerGold { get; private set; }
    public GameObject[] AvailableTurrets;
    private Node selectedNode;
    private Rigidbody2D rb;
    public GameObject traderPrefab;
    public TraderUIManager traderUIManager;
    public GameObject deathScreenPanel;
    public TextMeshProUGUI minutesSurvivedText;
    public TextMeshProUGUI totalGoldText;
    public int totalEnemiesKilled;
    private int gold = 0;
    public float xpGainMultiplier = 1.0f;
    public int xpUpgradeLevel = 0;

    private void Awake()
    {
        main = this;
        if (OnGoldChanged == null)
        {
            OnGoldChanged = () => { };
        }
    }

    public int Gold
    {
        get { return gold; }
        private set { gold = value; }
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        level = 1;
        experiencePoints = 0;
        InvokeRepeating("SpawnTrader", 5f, UnityEngine.Random.Range(20f, 40f));
        traderUIManager.Awake();
        traderUIManager = TraderUIManager.instance;
        playTime = 0f;

        LoadUpgrades();
    }

    private void LoadUpgrades()
    {
        xpUpgradeLevel = PlayerPrefs.GetInt("XPUpgradeLevel", 0);
        ExperiencePickup.baseExperienceAmount = 1 + xpUpgradeLevel;
    }

    private void Update()
    {
        playTime += Time.deltaTime;
        UpdatePlayTimeUI();
    }

    public int CalculateXpForNextLevel(int currentLevel, string stage)
    {
        int a = 0, b = 0, c = 0;
        switch (stage)
        {
            case "map1":
                a = -5;
                b = 4;
                c = -3;
                break;
            case "map2":
                a = 15;
                b = 5;
                c = 0;
                break;
            case "map3":
                a = 35;
                b = 6;
                c = 3;
                break;
            case "map4":
                a = 45;
                b = 7;
                c = 4;
                break;
        }
        return Mathf.FloorToInt((10 * Mathf.Pow(1.04f, currentLevel) + a * Mathf.Pow(0.95f, currentLevel) + b) * currentLevel + c);
    }

    public void IncreaseGold(int amount)
    {
        Gold += amount;
        OnGoldChanged?.Invoke();
    }

    public void EnemyKilled()
    {
        //gold += 1;
        totalEnemiesKilled++;
    }

    public bool SpendGold(int amount)
    {
        if (amount <= gold)
        {
            gold -= amount;
            OnGoldChanged?.Invoke();
            return true;
        }
        else
        {
            Debug.Log("Not enough gold.");
            return false;
        }
    }

    private void UpdatePlayTimeUI()
    {
        if (playTimeText != null)
        {
            int minutes = (int)(playTime / 60);
            int seconds = (int)(playTime % 60);
            playTimeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    public bool HasEnoughGold(int amount)
    {
        return gold >= amount;
    }

    public void SetSelectedNode(Node node)
    {
        selectedNode = node;
    }

    public Node GetSelectedNode()
    {
        return selectedNode;
    }

    public void ClearSelectedNodePosition()
    {
        selectedNode = null;
    }

    public int GetWallCost()
    {
        return wallCost;
    }

    public void IncreaseExperienceGain()
    {
        xpGainMultiplier += 0.1f;
    }

    void LevelUp()
    {
        level++;
        StatSelectionPanel statSelectionPanel = FindObjectOfType<StatSelectionPanel>();
        if (statSelectionPanel != null)
        {
            statSelectionPanel.OpenStatSelection();
        }
        else
        {
            Debug.LogError("StatSelectionPanel not found in the scene!");
        }
    }

    public void CheckLevelUp()
    {
        int requiredExperience = CalculateXpForNextLevel(level, "map1");

        if (experiencePoints >= requiredExperience)
        {
            experiencePoints -= requiredExperience;
            LevelUp();
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

    public void IncreaseExperiencePoints(int amount)
    {
        int extraXp = xpUpgradeLevel * 25;
        experiencePoints += amount + extraXp;
        CheckLevelUp();
    }

    public void BuyXpUpgrade()
    {
        xpUpgradeLevel++;
        PlayerPrefs.SetInt("XPUpgradeLevel", xpUpgradeLevel);
        PlayerPrefs.Save();
        ExperiencePickup.IncreaseBaseExperienceAmount();
    }

    public void SpawnTrader()
    {
        GameObject trader = Instantiate(traderPrefab, new Vector2(UnityEngine.Random.Range(-10, 10), UnityEngine.Random.Range(-10, 10)), Quaternion.identity);
        Trader traderScript = trader.GetComponent<Trader>();
    }

    public void GoToMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    public void ReplayGame()
    {
        Time.timeScale = 1;

        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }

    public void ShowDeathScreen()
    {
        Time.timeScale = 0;

        deathScreenPanel.GetComponent<CanvasGroup>().alpha = 1;
        deathScreenPanel.GetComponent<CanvasGroup>().interactable = true;
        deathScreenPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;

        int minutes = (int)(playTime / 60);
        int seconds = (int)(playTime % 60);
        minutesSurvivedText.text = "Minutes Survived: " + string.Format("{0:00}:{1:00}", minutes, seconds);
        totalGoldText.text = "Total Gold: " + gold;
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1;

        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    public void ResetGame()
    {
        Time.timeScale = 1;
        level = 1;
        experiencePoints = 0;
        gold = 0;
        UpdatePlayTimeUI();
        PlayerPrefs.Save();
    }

}
