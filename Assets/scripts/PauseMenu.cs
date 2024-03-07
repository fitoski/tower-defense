using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public TMP_Text upgradesText;
    public GameObject optionsMenuUI;
    public AudioSource backgroundMusic;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
    }

    public void TogglePauseMenu()
    {
        pauseMenuUI.SetActive(!pauseMenuUI.activeSelf);
        Time.timeScale = pauseMenuUI.activeSelf ? 0f : 1f;

        if (pauseMenuUI.activeSelf)
        {
            backgroundMusic.Pause();
            UpdateUpgradesDisplay();
            optionsMenuUI.SetActive(false);
        }
        else
        {
            backgroundMusic.UnPause();
        }
    }

    private void UpdateUpgradesDisplay()
    {
        PlayerMovement player = FindObjectOfType<PlayerMovement>();
        if (player != null)
        {
            string upgradesDisplayText = "";
            if (player.hasIncreasedDamage)
            {
                upgradesDisplayText += "Damage: " + player.playerDamage.ToString() + "\n";
            }
            if (player.hasIncreasedHealth)
            {
                upgradesDisplayText += "Health: " + player.maxPlayerHealth.ToString() + "\n";
            }
            if (player.hasIncreasedSpeed)
            {
                upgradesDisplayText += "Speed: " + player.moveSpeed.ToString("F1") + "\n";
            }
            if (player.hasIncreasedCritChance)
            {
                upgradesDisplayText += "Crit Chance: " + (player.criticalHitChance * 100).ToString("F1") + "%\n";
            }
            if (player.hasIncreasedHealthRegen)
            {
                upgradesDisplayText += "Health Regen: " + player.healthRegenerationRate.ToString("F1") + " / sec\n";
            }
            if (player.hasIncreasedOrbitRadius)
            {
                upgradesDisplayText += "Orbit Radius: " + player.orbitRadius.ToString("F1") + "\n";
            }
            if (player.hasDecreasedAttackCooldown)
            {
                upgradesDisplayText += "Attack Speed: " + (1f / player.attackCooldown).ToString("F1") + " attacks/sec\n";
            }
            if (player.hasIncreasedBlockChance)
            {
                upgradesDisplayText += "Block Chance: " + player.blockChance.ToString() + "\n";
            }
            upgradesText.text = upgradesDisplayText;
        }
        else
        {
            upgradesText.text = "Player not found!";
        }
    }

    public void ResumeGame()
    {
        TogglePauseMenu();
    }

    public void OpenOptions()
    {
        optionsMenuUI.SetActive(!optionsMenuUI.activeSelf);
        pauseMenuUI.SetActive(!optionsMenuUI.activeSelf);
    }

    public void QuitToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
