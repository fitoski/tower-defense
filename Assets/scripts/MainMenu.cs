using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject optionsPanel;

    public void PlayButtonClicked()
    {
        SceneManager.LoadScene("Level1"); 
    }

    public void OptionsButtonClicked()
    {
        optionsPanel.SetActive(true);
    }

    public void QuitOptionsButtonClicked()
    {
        optionsPanel.SetActive(false);
    }

    public void QuitButtonClicked()
    {
        Application.Quit(); 
    }
}