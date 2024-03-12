using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Level1LocalizationUIController : MonoBehaviour
{
    public TextMeshProUGUI resumeButtonText;
    public TextMeshProUGUI optionsButtonText;
    public TextMeshProUGUI quitToMainMenuButtonText;
    public TextMeshProUGUI quitToDesktopButtonText;
    public TMP_Dropdown graphicsQualityDropdown;
    public Text fullscreenToggleText; 

    void Start()
    {
        if (LocalizationManager.Instance != null)
        {
            LocalizationManager.Instance.level1LocalizationUIController = this;
        }

        LocalizationManager.Instance.OnLanguageChanged += UpdateTexts;
        LocalizationManager.Instance.OnLanguageChanged += GameManager.main.UpdateDeathScreenTexts;
        UpdateTexts();
    }

    void OnEnable()
    {
        UpdateTexts();
        if (GameManager.main != null)
        {
            GameManager.main.UpdateDeathScreenTexts();
        }
    }

    void OnDestroy()
    {
        LocalizationManager.Instance.OnLanguageChanged -= UpdateTexts;
        LocalizationManager.Instance.OnLanguageChanged -= GameManager.main.UpdateDeathScreenTexts;
    }

    public void UpdateTexts()
    {
        resumeButtonText.text = LocalizationManager.Instance.GetLocalizedValue("pause_menu_resume_button");
        optionsButtonText.text = LocalizationManager.Instance.GetLocalizedValue("pause_menu_options_button");
        quitToMainMenuButtonText.text = LocalizationManager.Instance.GetLocalizedValue("pause_menu_quittomainmenu_button");
        quitToDesktopButtonText.text = LocalizationManager.Instance.GetLocalizedValue("pause_menu_quittodesktop_button");
        fullscreenToggleText.text = LocalizationManager.Instance.GetLocalizedValue("pause_menu_fullscreen_toggle");

        List<string> qualityOptionsKeys = new List<string> { "quality_dropdown_low", "quality_dropdown_medium", "quality_dropdown_high" };
        LocalizationManager.Instance.UpdateDropdownOptions(graphicsQualityDropdown, qualityOptionsKeys);
    }
}