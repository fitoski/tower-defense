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
        List<System.Action> availableActions = new List<System.Action> {
        IncreaseDamage,
        IncreaseHealth,
        IncreaseSpeed,
        IncreaseArmor
        // Burada daha fazla eylem ekleyebilirsiniz.
    };

        // Rastgele seçilen eylemleri saklayacak yeni bir liste oluşturun.
        List<System.Action> selectedActions = new List<System.Action>();

        // Toplamda görünmesini istediğiniz buton sayısı için bir döngü oluşturun, örneğin 4.
        for (int i = 0; i < 4; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, availableActions.Count);
            System.Action selectedAction = availableActions[randomIndex];
            selectedActions.Add(selectedAction);
            // Aynı eylemi tekrar seçmemek için listeden kaldırın.
            availableActions.RemoveAt(randomIndex);
        }

        // Paneldeki her butonu saklıyoruz ve hepsini gizliyoruz.
        foreach (var button in statButtons)
        {
            button.gameObject.SetActive(false);
        }

        // Seçilen eylemleri butonlara atayın ve sadece bu butonları etkinleştirin.
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
            buttonText.text = actionToAssign.Method.Name.Replace("Increase", " + ");
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => actionToAssign());
            // Butonu etkinleştirin.
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

    private void ClosePanel()
    {
        panel.SetActive(false);
        Time.timeScale = 1; 
    }
}