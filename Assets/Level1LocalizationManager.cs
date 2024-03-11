using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Level1LocalizationManager : MonoBehaviour
{
    public TextMeshProUGUI resumeButtonText;
    public TextMeshProUGUI optionsButtonText;
    public TextMeshProUGUI quitToMainMenuButtonText;
    public TextMeshProUGUI quitToDesktopButtonText;
    public TextMeshProUGUI fullscreenToggleText;

    private Dictionary<string, string> localizedText;
    private string currentLanguage = "English";

    void Start()
    {
        LoadLocalizedText(currentLanguage);
        UpdateTexts();
    }

    public void ChangeLanguage(string language)
    {
        currentLanguage = language;
        LoadLocalizedText(language);
        UpdateTexts();
    }

    void LoadLocalizedText(string language)
    {
        localizedText = new Dictionary<string, string>();
        TextAsset textAsset = Resources.Load<TextAsset>(language);
        if (textAsset != null)
        {
            LocalizationData data = JsonUtility.FromJson<LocalizationData>(textAsset.text);

            localizedText["resume_button"] = data.resume_button;
            localizedText["options_button"] = data.options_button;
            localizedText["quit_to_main_menu_button"] = data.quit_to_main_menu_button;
            localizedText["quit_to_desktop_button"] = data.quit_to_desktop_button;
            localizedText["pause_menu_fullscreen_toggle"] = data.pause_menu_fullscreen_toggle;
        }
        else
        {
            Debug.LogError("Cannot find localization file for language: " + language);
        }
    }

    void UpdateTexts()
    {
        resumeButtonText.text = GetLocalizedValue("resume_button");
        optionsButtonText.text = GetLocalizedValue("options_button");
        quitToMainMenuButtonText.text = GetLocalizedValue("quit_to_main_menu_button");
        quitToDesktopButtonText.text = GetLocalizedValue("quit_to_desktop_button");
        fullscreenToggleText.text = GetLocalizedValue("pause_menu_fullscreen_toggle");
    }

    string GetLocalizedValue(string key)
    {
        if (localizedText.TryGetValue(key, out string value))
        {
            return value;
        }
        else
        {
            return "Missing Text";
        }
    }

    [System.Serializable]
    public class LocalizationData
    {
        public string resume_button, options_button, quit_to_main_menu_button, quit_to_desktop_button, pause_menu_fullscreen_toggle;
    }
}
