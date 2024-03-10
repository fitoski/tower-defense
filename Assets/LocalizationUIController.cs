using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class LocalizationUIController : MonoBehaviour
{
    public TextMeshProUGUI playButtonText;
    public TextMeshProUGUI optionsButtonText;
    public TextMeshProUGUI exitButtonText;
    public Text fullscreen_toggleText;
    //public TextMeshProUGUI pauseMenuResumeButtonText;
    //public TextMeshProUGUI pauseMenuOptionsButtonText;
    //public TextMeshProUGUI pauseMenuQuitToMainMenuButtonText;
    //public TextMeshProUGUI pauseMenuQuitToDesktopButtonText;
    //public Text pauseMenufullscreen_toggleText;

    // public TextMeshProUGUI
    // public TextMeshProUGUI
    // public TextMeshProUGUI
    // public TextMeshProUGUI
    //public TextMeshProUGUI
    //public TextMeshProUGUI
    //public TextMeshProUGUI
    //public TextMeshProUGUI

    private void Start()
    {
        UpdateTexts();
    }

    public void UpdateTexts()
    {
        playButtonText.text = LocalizationManager.Instance.GetLocalizedValue("play_button");
        optionsButtonText.text = LocalizationManager.Instance.GetLocalizedValue("options_button");
        exitButtonText.text = LocalizationManager.Instance.GetLocalizedValue("exit_button");
        fullscreen_toggleText.text = LocalizationManager.Instance.GetLocalizedValue("fullscreen_toggle");
        //pauseMenuResumeButtonText.text = LocalizationManager.Instance.GetLocalizedValue("pause_menu_resume_button");
        //pauseMenuOptionsButtonText.text = LocalizationManager.Instance.GetLocalizedValue("pause_menu_options_button");
        //pauseMenuQuitToMainMenuButtonText.text = LocalizationManager.Instance.GetLocalizedValue("pause_menu_quittomainmenu_button");
        //pauseMenuQuitToDesktopButtonText.text = LocalizationManager.Instance.GetLocalizedValue("pause_menu_quittodesktop_button");
        //pauseMenufullscreen_toggleText.text = LocalizationManager.Instance.GetLocalizedValue("pause_menu_fullscreen_toggle");
    }
}