using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager Instance;

    private Dictionary<string, string> localizedText;
    private string currentLanguage = "English";
    public TMP_Dropdown languageDropdown;
    public TMP_Dropdown qualityDropdown;
    public LocalizationUIController localizationUIController;
    public Level1LocalizationUIController level1LocalizationUIController;

    private void Start()
    {
        InitializeLanguageDropdown();
        ChangeLanguage(currentLanguage);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Level1")
        {
            if (level1LocalizationUIController != null)
            {
                level1LocalizationUIController.UpdateTexts();
            }
        }
        else
        {
            if (localizationUIController != null)
            {
                localizationUIController.UpdateTexts();
            }
        }
    }

    private void InitializeLanguageDropdown()
    {
        languageDropdown.ClearOptions();
        List<string> options = new List<string> { "English", "Türkçe" };
        languageDropdown.AddOptions(options);

        languageDropdown.onValueChanged.AddListener(delegate {
            DropdownValueChanged(languageDropdown);
        });
    }

    public void DropdownValueChanged(TMPro.TMP_Dropdown change)
    {
        if (change.value == 0)
        {
            ChangeLanguage("English");
        }
        else if (change.value == 1)
        {
            ChangeLanguage("Turkish");
        }

        localizationUIController.UpdateTexts();
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        LoadLocalizedText(currentLanguage);
    }

    public void UpdateDropdownOptions(TMP_Dropdown dropdown, List<string> optionKeys)
    {
        dropdown.ClearOptions();
        List<string> options = new List<string>();

        foreach (var key in optionKeys)
        {
            options.Add(GetLocalizedValue(key));
        }

        dropdown.AddOptions(options);
    }

    public void LoadLocalizedText(string language)
    {
        localizedText = new Dictionary<string, string>();
        TextAsset textAsset = Resources.Load<TextAsset>(language);
        if (textAsset != null)
        {
            LocalizationData data = JsonUtility.FromJson<LocalizationData>(textAsset.text);
            localizedText["play_button"] = data.play_button;
            localizedText["options_button"] = data.options_button;
            localizedText["exit_button"] = data.exit_button;
            localizedText["quality_dropdown_low"] = data.quality_dropdown_low;
            localizedText["quality_dropdown_medium"] = data.quality_dropdown_medium;
            localizedText["quality_dropdown_high"] = data.quality_dropdown_high;
            localizedText["fullscreen_toggle"] = data.fullscreen_toggle;
            localizedText["pause_menu_resume_button"] = data.pause_menu_resume_button;
            localizedText["pause_menu_options_button"] = data.pause_menu_options_button;
            localizedText["pause_menu_quittomainmenu_button"] = data.pause_menu_quittomainmenu_button;
            localizedText["pause_menu_quittodesktop_button"] = data.pause_menu_quittodesktop_button;
            localizedText["pause_menu_quality_dropdown_low"] = data.pause_menu_quality_dropdown_low;
            localizedText["pause_menu_quality_dropdown_medium"] = data.pause_menu_quality_dropdown_medium;
            localizedText["pause_menu_quality_dropdown_high"] = data.pause_menu_quality_dropdown_high;
            localizedText["pause_menu_fullscreen_toggle"] = data.pause_menu_fullscreen_toggle;

        }
        else
        {
            Debug.LogError("Cannot find file!");
        }
        if (localizationUIController != null)
        {
            localizationUIController.UpdateTexts();
        }
    }

    public string GetLocalizedValue(string key)
    {
        string result = "Missing Text";
        if (localizedText.ContainsKey(key))
        {
            result = localizedText[key];
        }
        return result;
    }

    public void ChangeLanguage(string language)
    {
        currentLanguage = language;
        LoadLocalizedText(language);

        List<string> qualityOptionsKeys = new List<string> { "quality_dropdown_low", "quality_dropdown_medium", "quality_dropdown_high" };
        UpdateDropdownOptions(qualityDropdown, qualityOptionsKeys);

        if (localizationUIController != null)
        {
            localizationUIController.UpdateTexts();
        }

        if (level1LocalizationUIController != null)
        {
            level1LocalizationUIController.UpdateTexts();
        }
    }

    [System.Serializable]
    public class LocalizationData
    {
        public string play_button;
        public string options_button;
        public string exit_button;
        public string quality_dropdown_low;
        public string quality_dropdown_medium;
        public string quality_dropdown_high;
        public string fullscreen_toggle;
        public string pause_menu_resume_button;
        public string pause_menu_options_button;
        public string pause_menu_quittomainmenu_button;
        public string pause_menu_quittodesktop_button;
        public string pause_menu_quality_dropdown_low;
        public string pause_menu_quality_dropdown_medium;
        public string pause_menu_quality_dropdown_high;
        public string pause_menu_fullscreen_toggle;
    }
}