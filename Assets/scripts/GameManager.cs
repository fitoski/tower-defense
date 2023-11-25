using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using static Enums;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager main;
    public int currency;
    public int level;
    public int experiencePoints;
    private float playTime = 0f;
    public TextMeshProUGUI playTimeText;
    public int turretCost = 50;  
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
        playTime = 0f; 
    }

    private void Update()
    {
        playTime += Time.deltaTime;
        UpdatePlayTimeUI();
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

    private void UpdatePlayTimeUI()
    {
        if (playTimeText != null)
        {
            int minutes = (int)(playTime / 60);
            int seconds = (int)(playTime % 60);
            playTimeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    public bool HasEnoughCurrency(int amount)
    {
        return currency >= amount;
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
        int requiredExperience = Mathf.FloorToInt(10 * Mathf.Pow(1.7f, level - 1));

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

    public void SpawnTrader()
    {
        GameObject trader = Instantiate(traderPrefab, new Vector2(Random.Range(-10, 10), Random.Range(-10, 10)), Quaternion.identity);
        Trader traderScript = trader.GetComponent<Trader>();
        if (traderScript != null)
        {
            traderScript.traderUIManager = TraderUIManager.instance;
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

    public void ShowDeathScreen()
    {
        Time.timeScale = 0;

        deathScreenPanel.GetComponent<CanvasGroup>().alpha = 1;
        deathScreenPanel.GetComponent<CanvasGroup>().interactable = true;
        deathScreenPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;

        int minutes = (int)(playTime / 60);
        int seconds = (int)(playTime % 60);
        minutesSurvivedText.text = "Minutes Survived: " + string.Format("{0:00}:{1:00}", minutes, seconds);
        totalGoldText.text = "Total Gold: " + currency;
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1;

        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

}