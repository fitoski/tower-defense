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
    public bool isStatPanelOpen = false;
    public bool isSkillPanelOpen = false;


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
        //LocalizationManager.Instance.OnLanguageChanged += UpdateDeathScreenTexts;
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
        string currentSceneName = SceneManager.GetActiveScene().name;
        int baseXp = 500;
        float xpMultiplier = 1.5f;

        if (currentSceneName == "Level1")
        {
            return Mathf.FloorToInt(baseXp * Mathf.Pow(xpMultiplier, currentLevel - 1));
        }
        else if (currentSceneName == "Level2")
        {
            return Mathf.FloorToInt(baseXp * 1.5f * Mathf.Pow(xpMultiplier, currentLevel - 1));
        }
        else
        {
            return Mathf.FloorToInt(baseXp * Mathf.Pow(xpMultiplier, currentLevel - 1));
        }
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

    public void CheckAndResumeGame()
    {
        Debug.Log($"CheckAndResumeGame called. isStatPanelOpen: {isStatPanelOpen}, isSkillPanelOpen: {isSkillPanelOpen}");
        if (!isStatPanelOpen && !isSkillPanelOpen)
        {
            Debug.Log("Both panels are closed. Resuming game.");
            Time.timeScale = 1;
        }
        else
        {
            Debug.Log("One or both panels are open. Checking further...");

            if (isStatPanelOpen && !FindObjectOfType<StatSelectionPanel>().panel.activeSelf)
            {
                Debug.LogError("Stat panel state was incorrectly marked as open. Correcting state...");
                isStatPanelOpen = false;
            }

            if (isSkillPanelOpen && !SkillsManager.Instance.skillRewardPanel.activeSelf)
            {
                Debug.LogError("Skill panel state was incorrectly marked as open. Correcting state...");
                isSkillPanelOpen = false;
            }

            if (!isStatPanelOpen && !isSkillPanelOpen)
            {
                Debug.Log("Corrected panel states. Resuming game.");
                Time.timeScale = 1;
            }
            else
            {
                Debug.LogWarning("Game remains paused due to an open panel.");
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

    public void CheckPanelsAndResumeGame()
    {
        if (!isStatPanelOpen && !isSkillPanelOpen)
        {
            Time.timeScale = 1;
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
        Debug.Log("GameManager: Trying to spawn a trader...");

        GameObject trader = Instantiate(traderPrefab, new Vector2(UnityEngine.Random.Range(-10, 10), UnityEngine.Random.Range(-10, 10)), Quaternion.identity);
        if (trader == null)
        {
            Debug.LogError("GameManager: Failed to instantiate trader prefab.");
        }
        else
        {
            Debug.Log("GameManager: Trader spawned successfully.");
        }
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
        //if (deathScreenManager != null)
        //{
        //    //deathScreenManager.restartButtonText.text = LocalizationManager.Instance.GetLocalizedValue("death_panel_restart_button");
        //    //deathScreenManager.returnToMainMenuButtonText.text = LocalizationManager.Instance.GetLocalizedValue("death_panel_returntomainmenu_button");
        //    //deathScreenManager.exitToDesktopButtonText.text = LocalizationManager.Instance.GetLocalizedValue("death_panel_exittodesktop_button");
        //    //deathScreenManager.minutesSurvivedLabel.text = LocalizationManager.Instance.GetLocalizedValue("death_panel_minutes_survived");
        //    //deathScreenManager.totalGoldLabel.text = LocalizationManager.Instance.GetLocalizedValue("death_panel_total_gold");
        //}
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