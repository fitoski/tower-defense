using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.Events;
using System.Linq;

public class StatSelectionPanel : MonoBehaviour
{
    public GameObject panel;
    public Button[] statButtons;
    private PlayerMovement player;
    public List<StatProperty> stats = new List<StatProperty>();

    private void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
        if (player == null)
        {
            Debug.LogError("PlayerMovement component not found in the scene.");
            return; 
        }

        if (statButtons == null || statButtons.Length == 0)
        {
            Debug.LogError("Stat buttons are not assigned in the inspector.");
            return; 
        }

        foreach (var button in statButtons)
        {
            if (button == null)
            {
                Debug.LogError("One of the stat buttons is not assigned in the inspector.");
                return; 
            }
        }
        panel.SetActive(false);
    }

    public void OpenStatSelection()
    {
        panel.SetActive(true);
        Time.timeScale = 0; 
        RandomizeStats();
    }

    private void RandomizeStats()
    {
        Dictionary<string, StatProperty> statsDict = stats.ToDictionary(sp => sp.name);

        if (player.moveSpeed >= player.maxMoveSpeed)
        {
            statsDict.Remove("Increase Speed");
        }
        if (player.orbitRadius >= player.maxOrbitRadius)
        {
            
        }
        if (player.healthRegenerationRate >= player.maxHealthRegenRate)
        {
            
        }
        if (player.attackCooldown <= player.minimumAttackCooldown)
        {
            
        }
        if (player.criticalHitChance < 1f)
        {
            
        }

        foreach (var button in statButtons)
        {
            button.gameObject.SetActive(false);
        }
        List<StatProperty> selectedStats = new List<StatProperty> ();
        while (selectedStats.Count < 4 && statsDict.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, statsDict.Count);
            StatProperty selectedStat = statsDict.ElementAt(randomIndex).Value;
            selectedStats.Add(selectedStat);
            statsDict.Remove(selectedStat.name);
        }
        for (int i = 0; i < Mathf.Min(selectedStats.Count, statButtons.Length); i++)
        {
            Button button = statButtons[i];
            UnityEvent actionToAssign = selectedStats[i].action;
            TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
            Image image = button.GetComponentsInChildren<Image>()[1];
            image.sprite = selectedStats[i].icon;
            if (buttonText == null)
            {
                Debug.LogError("TextMeshProUGUI component on stat button is not found.");
                continue;
            }
            buttonText.text = selectedStats[i].name;
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => actionToAssign.Invoke());
            button.gameObject.SetActive(true);
        }
    }

    public void IncreaseDamage()
    {
        player.IncreaseDamagePerLevel();
        ClosePanel();
    }

    public void IncreaseHealth()
    {
        player.IncreaseMaxHealthPerLevel();
        ClosePanel();
    }

    public void IncreaseSpeed()
    {
        player.IncreaseMoveSpeedPerLevel();
        ClosePanel();
    }

    public void IncreaseCriticalHitChance()
    {
        player.IncreaseCriticalHitChance(); 
        ClosePanel();
    }

    public void IncreaseHealthRegeneration()
    {
        player.IncreaseHealthRegeneration(); 
        ClosePanel();
    }

    public void IncreaseOrbitRadiusPerLevel()
    {
        player.IncreaseOrbitRadiusPerLevel(); 
        ClosePanel();
    }

    public void DecreaseAttackCooldown()
    {
        player.DecreaseAttackCooldownPerLevel(); 
        ClosePanel();
    }

    private void ClosePanel()
    {
        panel.SetActive(false);
        Time.timeScale = 1; 
    }
}

[Serializable]
public class StatProperty
{
    public Sprite icon;
    public string name;
    public string description;
    public UnityEvent action;
}