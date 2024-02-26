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
    public StatButton[] statButtons;
    private PlayerMovement player;
    public List<StatProperty> stats = new List<StatProperty>();
    public TextMeshProUGUI descriptionText;

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
        if (player.moveSpeed >= PlayerMovement.maxMoveSpeed)
        {
            statsDict.Remove("Increase Speed");
        }
        if (player.orbitRadius >= PlayerMovement.maxOrbitRadius)
        {

        }
        if (player.healthRegenerationRate >= PlayerMovement.maxHealthRegenRate)
        {

        }
        if (player.attackCooldown <= PlayerMovement.minimumAttackCooldown)
        {

        }
        if (player.criticalHitChance < 1f)
        {

        }
        if (player.defense >= 100)
        {
            statsDict.Remove("Increase Defense");
        }
        if (player.blockChance >= 100)
        {
            statsDict.Remove("Increase Block Chance");
        }
        if (player.pickupRangeLevel >= PlayerMovement.maxPickupRangeLevel)
        {
            statsDict.Remove("Increase Pickup Range");
        }

        foreach (var statButton in statButtons)
        {
            statButton.button.gameObject.SetActive(false);
        }

        List<StatProperty> selectedStats = new List<StatProperty>();

        while (selectedStats.Count < 4 && statsDict.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, statsDict.Count);
            StatProperty selectedStat = statsDict.ElementAt(randomIndex).Value;
            selectedStats.Add(selectedStat);
            statsDict.Remove(selectedStat.name);
        }

        for (int i = 0; i < Mathf.Min(selectedStats.Count, statButtons.Length); i++)
        {
            var statButton = statButtons[i];
            var stat = selectedStats[i];
            statButton.button.gameObject.SetActive(true);
            statButton.button.onClick.RemoveAllListeners();
            statButton.button.onClick.AddListener(() => stat.action.Invoke());
            statButton.nameText.text = stat.name;
            statButton.descriptionText.text = stat.description;
            Image image = statButton.button.GetComponentInChildren<Image>(true);

            if (image != null)
            {
                image.sprite = stat.icon;
            }
        }
    }

    private void UpdateDescription(string description)
    {
        if (descriptionText != null)
        {
            descriptionText.text = description;
        }
        else
        {
            Debug.LogError("Description TextMeshProUGUI component is not assigned.");
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

    public void IncreaseDefense()
    {
        player.IncreaseDefense();
        ClosePanel();
    }

    public void IncreaseBlockChance()
    {
        player.IncreaseBlockChance();
        ClosePanel();
    }

    public void IncreasePickupRange()
    {
        player.IncreasePickupRangeLevel();
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

[System.Serializable]
public class StatButton
{
    public Button button;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
}