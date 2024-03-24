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
            optionsMenuUI.SetActive(false);
        }
        else
        {
            backgroundMusic.UnPause();
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