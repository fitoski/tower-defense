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
        List<System.Action> availableActions = new List<System.Action>();

        if (player.moveSpeed < player.maxMoveSpeed)
        {
            availableActions.Add(IncreaseSpeed);
        }
        if (player.orbitRadius < player.maxOrbitRadius)
        {
            availableActions.Add(IncreaseOrbitRadiusPerLevel);
        }
        if (player.healthRegenerationRate < player.maxHealthRegenRate)
        {
            availableActions.Add(IncreaseHealthRegeneration);
        }

        availableActions.Add(IncreaseDamage);
        availableActions.Add(IncreaseHealth);
        availableActions.Add(IncreaseArmor);
        availableActions.Add(IncreaseCriticalHitChance);

        List<System.Action> selectedActions = new List<System.Action>();
        while (selectedActions.Count < Mathf.Min(4, availableActions.Count))
        {
            int randomIndex = UnityEngine.Random.Range(0, availableActions.Count);
            System.Action selectedAction = availableActions[randomIndex];
            selectedActions.Add(selectedAction);
            availableActions.RemoveAt(randomIndex);
        }

        foreach (var button in statButtons)
        {
            button.gameObject.SetActive(false);
        }

        for (int i = 0; i < selectedActions.Count; i++)
        {
            System.Action actionToAssign = selectedActions[i];
            Button button = statButtons[i];
            TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText == null)
            {
                Debug.LogError("TextMeshProUGUI component on stat button is not found.");
                continue;
            }
            buttonText.text = actionToAssign.Method.Name.Replace("Increase", "") + " +";
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => actionToAssign());
            button.gameObject.SetActive(true);
        }
    }

    public void IncreaseDamage()
    {
        player.IncreaseDamagePerLevel(1);
        ClosePanel();
    }

    public void IncreaseHealth()
    {
        player.IncreaseMaxHealthPerLevel(10);
        ClosePanel();
    }

    public void IncreaseSpeed()
    {
        player.IncreaseMoveSpeedPerLevel(0.5f);
        ClosePanel();
    }

    public void IncreaseArmor()
    {
        player.ChangeArmor(player.armorValue + 5);
        ClosePanel();
    }

    public void IncreaseCriticalHitChance()
    {
        player.IncreaseCriticalHitChance(0.01f); 
        ClosePanel();
    }

    public void IncreaseHealthRegeneration()
    {
        player.IncreaseHealthRegeneration(0.1f); 
        ClosePanel();
    }

    public void IncreaseOrbitRadiusPerLevel()
    {
        player.IncreaseOrbitRadiusPerLevel(0.5f); 
        ClosePanel();
    }

    private void ClosePanel()
    {
        panel.SetActive(false);
        Time.timeScale = 1; 
    }
}