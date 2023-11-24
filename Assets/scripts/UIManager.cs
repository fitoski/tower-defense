using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
            levelText.text = "Lv: " + gameManager.GetCurrentLevel().ToString();

            float experienceRatio = (float)gameManager.GetCurrentExperiencePoints() /
                                   (10 * Mathf.Pow(1.7f, gameManager.GetCurrentLevel() - 1));
            experienceBar.fillAmount = experienceRatio;
        }
    }
}