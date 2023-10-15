using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public void PlayButtonClicked()
    {
        SceneManager.LoadScene("Level1"); 
    }

    public void OptionsButtonClicked()
    {
        
    }

    public void QuitButtonClicked()
    {
        Application.Quit(); 
    }
}