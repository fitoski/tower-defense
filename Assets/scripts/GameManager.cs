using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using static Enums;
using TMPro;
using System;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

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
    public int levelsGainedThisSession = 0;
    private Queue<int> experienceQueue = new Queue<int>();


    // death panel
    private DeathScreenManager deathScreenManager; 
    //public TextMeshProUGUI bountiesLabel;

    public void EnqueueExperience(int amount)
    {
        experienceQueue.Enqueue(amount);
    }

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        main = this;
        if (OnGoldChanged == null)
        {
            OnGoldChanged = () => { };
        }

        deathScreenManager = GameObject.FindGameObjectWithTag("DeathScreen")?.GetComponent<DeathScreenManager>();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ApplyLanguageSettings();
    }

    private void ApplyLanguageSettings()
    {
        if (LocalizationManager.Instance != null)
        {
            UpdateDeathScreenTexts();
        }
    }

    public int Gold
    {
        get { return gold; }
        private set { gold = value; }
    }

    private void Start()
    {
        LocalizationManager.Instance.OnLanguageChanged += UpdateDeathScreenTexts;
        rb = GetComponent<Rigidbody2D>();
        level = 1;
        experiencePoints = 0;
        InvokeRepeating("SpawnTrader", 5f, UnityEngine.Random.Range(20f, 40f));
        traderUIManager.Awake();
        traderUIManager = TraderUIManager.instance;
        playTime = 0f;
        LoadUpgrades();
        UpdateDeathScreenTexts();
    }

    private void OnDestroy()
    {
        if (LocalizationManager.Instance != null)
        {
            LocalizationManager.Instance.OnLanguageChanged -= UpdateDeathScreenTexts;
        }
    }

    private void LoadUpgrades()
    {
        xpUpgradeLevel = PlayerPrefs.GetInt("XPUpgradeLevel", 0);
    }

    private void Update()
    {
        playTime += Time.deltaTime;
        UpdatePlayTimeUI();
        if (experienceQueue.Count > 0)
        {
            int experienceToProcess = experienceQueue.Dequeue();
            IncreaseExperiencePoints(experienceToProcess);
        }
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
        return Mathf.FloorToInt((10 * Mathf.Pow(1.04f, currentLevel) + a * Mathf.Pow(0.95f, currentLevel) + b) * currentLevel + c) * 100;
    }

    public void IncreaseGold(int amount)
    {
        Gold += amount;
        OnGoldChanged?.Invoke();
    }

    public void EnemyKilled()
    {
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
        levelsGainedThisSession++;
        CheckAndOpenStatSelectionPanel();

        //StatSelectionPanel statSelectionPanel = FindObjectOfType<StatSelectionPanel>();
        //if (statSelectionPanel != null)
        //{
        //    statSelectionPanel.OpenStatSelection();
        //}
        //else
        //{
        //    Debug.LogError("StatSelectionPanel not found in the scene!");
        //}
    }

    void CheckAndOpenStatSelectionPanel()
    {
        if (levelsGainedThisSession > 0)
        {
            StatSelectionPanel statSelectionPanel = FindObjectOfType<StatSelectionPanel>();
            if (statSelectionPanel != null)
            {
                statSelectionPanel.OpenStatSelection(levelsGainedThisSession);
                levelsGainedThisSession = 0;
            }
            else
            {
                Debug.LogError("StatSelectionPanel not found in the scene!");
            }
        }
    }

    public void CheckLevelUp()
    {
        int requiredExperience = CalculateXpForNextLevel(level, "map1");
        Debug.Log($"current exp: {experiencePoints}");
        Debug.Log($"required exp: {requiredExperience}");

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
        experiencePoints += amount;
        CheckLevelUp();
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

    public void UpdateDeathScreenTexts()
    {
        if (deathScreenManager != null)
        {
            deathScreenManager.restartButtonText.text = LocalizationManager.Instance.GetLocalizedValue("death_panel_restart_button");
            deathScreenManager.returnToMainMenuButtonText.text = LocalizationManager.Instance.GetLocalizedValue("death_panel_returntomainmenu_button");
            deathScreenManager.exitToDesktopButtonText.text = LocalizationManager.Instance.GetLocalizedValue("death_panel_exittodesktop_button");
            deathScreenManager.minutesSurvivedLabel.text = LocalizationManager.Instance.GetLocalizedValue("death_panel_minutes_survived");
            deathScreenManager.totalGoldLabel.text = LocalizationManager.Instance.GetLocalizedValue("death_panel_total_gold");
        }
        //bountiesLabel.text = LocalizationManager.Instance.GetLocalizedValue("death_panel_bounties");
    }

    public void ShowDeathScreen()
    {
        Time.timeScale = 0;

        deathScreenPanel.GetComponent<CanvasGroup>().alpha = 1;
        deathScreenPanel.GetComponent<CanvasGroup>().interactable = true;
        deathScreenPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;

        int minutes = (int)(playTime / 60);
        int seconds = (int)(playTime % 60);
        minutesSurvivedText.text = $"{LocalizationManager.Instance.GetLocalizedValue("death_panel_minutes_survived")} " + string.Format("{0:00}:{1:00}", minutes, seconds);
        totalGoldText.text = $"{LocalizationManager.Instance.GetLocalizedValue("death_panel_total_gold")} " + gold;
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