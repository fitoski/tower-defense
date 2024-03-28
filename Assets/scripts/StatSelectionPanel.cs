using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.Events;
using System.Linq;
using System.Collections;

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

    public void OpenStatSelection(int levelsGained)
    {
        panel.SetActive(true);
        Time.timeScale = 0;
        GameManager.main.isStatPanelOpen = true;
        RandomizeStats();

        GameManager.main.levelsGainedThisSession -= 1;
        if (GameManager.main.levelsGainedThisSession > 0)
        {
            ClosePanel(true);
        }
    }

    private void ClosePanel(bool reopenIfNeeded = false)
    {
        panel.SetActive(false);
        GameManager.main.isStatPanelOpen = false;
        if (!reopenIfNeeded)
        {
            GameManager.main.CheckAndResumeGame();
        }
        else if (GameManager.main.levelsGainedThisSession > 0)
        {
            StartCoroutine(ReopenPanelNextFrame());
        }
    }

    private IEnumerator ReopenPanelNextFrame()
    {
        yield return null;
        OpenStatSelection(GameManager.main.levelsGainedThisSession);
    }

    private void RandomizeStats()
    {
        foreach (var statButton in statButtons)
        {
            statButton.button.gameObject.SetActive(false);
        }

        List<StatProperty> selectedStats = new List<StatProperty>(stats);

        for (int i = 0; i < selectedStats.Count; i++)
        {
            StatProperty temp = selectedStats[i];
            int randomIndex = UnityEngine.Random.Range(i, selectedStats.Count);
            selectedStats[i] = selectedStats[randomIndex];
            selectedStats[randomIndex] = temp;
        }

        int statCount = Mathf.Min(statButtons.Length, selectedStats.Count);
        for (int i = 0; i < statCount; i++)
        {
            var statButton = statButtons[i];
            var stat = selectedStats[i];
            statButton.button.gameObject.SetActive(true);
            statButton.button.onClick.RemoveAllListeners();
            statButton.button.onClick.AddListener(() => stat.action.Invoke());
            statButton.nameText.text = stat.name;
            statButton.descriptionText.text = stat.description;
            statButton.icon.sprite = stat.icon;
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
    public TMP_Text nameText;
    public TMP_Text descriptionText;
    public Image icon;
}