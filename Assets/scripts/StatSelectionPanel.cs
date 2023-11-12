using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatSelectionPanel : MonoBehaviour
{
    public GameObject panel;
    public Button[] statButtons;
    private PlayerMovement player;

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
        List<System.Action> availableActions = new List<System.Action>
    {
        IncreaseDamage,
        IncreaseHealth,
        IncreaseSpeed,
        IncreaseOrbitRadiusPerLevel,
        IncreaseHealthRegeneration,
        DecreaseAttackCooldown
    };
        if (player.moveSpeed >= player.maxMoveSpeed)
        {
            availableActions.Remove(IncreaseSpeed);
        }
        if (player.orbitRadius >= player.maxOrbitRadius)
        {
            availableActions.Remove(IncreaseOrbitRadiusPerLevel);
        }
        if (player.healthRegenerationRate >= player.maxHealthRegenRate)
        {
            availableActions.Remove(IncreaseHealthRegeneration);
        }
        if (player.attackCooldown <= player.minimumAttackCooldown)
        {
            availableActions.Remove(DecreaseAttackCooldown);
        }
        if (player.criticalHitChance < 1f)
        {
            availableActions.Add(IncreaseCriticalHitChance);
        }
        foreach (var button in statButtons)
        {
            button.gameObject.SetActive(false);
        }
        List<System.Action> selectedActions = new List<System.Action>();
        while (selectedActions.Count < 4 && availableActions.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, availableActions.Count);
            System.Action selectedAction = availableActions[randomIndex];
            selectedActions.Add(selectedAction);
            availableActions.RemoveAt(randomIndex);
        }
        for (int i = 0; i < Mathf.Min(selectedActions.Count, statButtons.Length); i++)
        {
            Button button = statButtons[i];
            System.Action actionToAssign = selectedActions[i];
            TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText == null)
            {
                Debug.LogError("TextMeshProUGUI component on stat button is not found.");
                continue;
            }
            buttonText.text = actionToAssign.Method.Name.Replace("Increase", "").Replace("Decrease", "") + " +";
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => actionToAssign());
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