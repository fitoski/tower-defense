using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;

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
        Time.timeScale = (pauseMenuUI.activeSelf) ? 0f : 1f; 
    }

    public void ResumeGame()
    {
        TogglePauseMenu();
    }

    public void OpenOptions()
    {
        SceneManager.LoadScene("OptionsScene");
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
