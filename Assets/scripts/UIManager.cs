using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI levelText;
    public Image experienceBar;

    private GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.main;
    }

    void Update()
    {
        if (gameManager != null)
        {
            int currentLevel = gameManager.GetCurrentLevel();
            string currentSceneName = SceneManager.GetActiveScene().name;

            levelText.text = "Lv: " + currentLevel.ToString();

            float currentXp = gameManager.GetCurrentExperiencePoints();
            float nextLevelXp = gameManager.CalculateXpForNextLevel(currentLevel, currentSceneName);
            float experienceRatio = currentXp / nextLevelXp;
            experienceBar.fillAmount = experienceRatio;
        }
    }
}