using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager Instance;
    public event Action OnLanguageChanged;

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
            var controller = FindObjectOfType<Level1LocalizationUIController>();
            if (controller != null)
            {
                level1LocalizationUIController = controller;
                controller.UpdateTexts();
            }
        }
        if (scene.name == "MainMenu")
        {
            var controller = FindObjectOfType<LocalizationUIController>();
            if (controller != null)
            {
                localizationUIController = controller;
                controller.UpdateTexts();
            }
        }
    }

    private void InitializeLanguageDropdown()
    {
        languageDropdown.ClearOptions();
        List<string> options = new List<string> {
        "English",
        "Türkçe",
        "简体中文",
        "Español",
        "Français",
        "Italiano",
        "Deutsch",
        "日本語",
        "한국어",
        "Polski",
        "Português",
        "Русский"};
        languageDropdown.AddOptions(options);

        languageDropdown.onValueChanged.AddListener(delegate {
            DropdownValueChanged(languageDropdown);
        });
    }

    public void DropdownValueChanged(TMPro.TMP_Dropdown change)
    {
        switch (change.value)
        {
            case 0:
                ChangeLanguage("English");
                break;
            case 1:
                ChangeLanguage("Turkish");
                break;
            case 2:
                ChangeLanguage("SimplifiedChinese");
                break;
            case 3:
                ChangeLanguage("Spanish");
                break;
            case 4:
                ChangeLanguage("French");
                break;
            case 5:
                ChangeLanguage("Italian");
                break;
            case 6:
                ChangeLanguage("German");
                break;
            case 7:
                ChangeLanguage("Japanese");
                break;
            case 8:
                ChangeLanguage("Korean");
                break;
            case 9:
                ChangeLanguage("Polish");
                break;
            case 10:
                ChangeLanguage("Portuguese");
                break;
            case 11:
                ChangeLanguage("Russian");
                break;

            default:
                Debug.LogError("Unsupported language selected.");
                break;
        }

        if (localizationUIController != null)
        {
            localizationUIController.UpdateTexts();
        }

        if (level1LocalizationUIController != null)
        {
            level1LocalizationUIController.UpdateTexts();
        }
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

        currentLanguage = PlayerPrefs.GetString("SelectedLanguage", "English");
        LoadLocalizedText(currentLanguage);
        ApplyLanguageToUI();
    }

    public void ApplyLanguageToUI()
    {
        if (localizationUIController != null)
        {
            localizationUIController.UpdateTexts();
        }
        if (level1LocalizationUIController != null)
        {
            level1LocalizationUIController.UpdateTexts();
        }
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

    public void UpdateUpgradesPanelTexts()
    {
        TextMeshProUGUI criticalHitDamageText = GameObject.Find("CriticalHitDamageText").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI mulstrikeText = GameObject.Find("MultistrikeText").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI experiencegainText = GameObject.Find("ExperienceGainText").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI basicturretupgradeText = GameObject.Find("BasicTurretUpgradeText").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI electricturretupgradeText = GameObject.Find("ElectricTurretUpgradeText").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI fireturretupgradeText = GameObject.Find("FireTurretUpgradeText").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI iceturretupgradeText = GameObject.Find("IceTurretUpgradeText").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI windturretupgradeText = GameObject.Find("WindTurretUpgradeText").GetComponent<TextMeshProUGUI>();
        //TextMeshProUGUI bountiesText = GameObject.Find("BountiesText").GetComponent<TextMeshProUGUI>();

        criticalHitDamageText.text = LocalizationManager.Instance.GetLocalizedValue("upgrades_critical_hit_damage");
        mulstrikeText.text = LocalizationManager.Instance.GetLocalizedValue("upgrades_multistrike_chance");
        experiencegainText.text = LocalizationManager.Instance.GetLocalizedValue("upgrades_experience_gain");
        basicturretupgradeText.text = LocalizationManager.Instance.GetLocalizedValue("upgrades_basic_turret");
        electricturretupgradeText.text = LocalizationManager.Instance.GetLocalizedValue("upgrades_electric_turret");
        fireturretupgradeText.text = LocalizationManager.Instance.GetLocalizedValue("upgrades_fire_turret");
        iceturretupgradeText.text = LocalizationManager.Instance.GetLocalizedValue("upgrades_ice_turret");
        windturretupgradeText.text = LocalizationManager.Instance.GetLocalizedValue("upgrades_wind_turret");
        //bountiesText.text = LocalizationManager.Instance.GetLocalizedValue("upgrades_boss_kill_score");
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
            localizedText["upgrades_critical_hit_damage"] = data.upgrades_critical_hit_damage;
            localizedText["upgrades_multistrike_chance"] = data.upgrades_multistrike_chance;
            localizedText["upgrades_experience_gain"] = data.upgrades_experience_gain;
            localizedText["upgrades_basic_turret"] = data.upgrades_basic_turret;
            localizedText["upgrades_electric_turret"] = data.upgrades_electric_turret;
            localizedText["upgrades_fire_turret"] = data.upgrades_fire_turret;
            localizedText["upgrades_ice_turret"] = data.upgrades_ice_turret;
            localizedText["upgrades_wind_turret"] = data.upgrades_wind_turret;
            localizedText["upgrades_boss_kill_score"] = data.upgrades_boss_kill_score;
            localizedText["upgrades_button_text"] = data.upgrades_button_text;
            localizedText["death_panel_restart_button"] = data.death_panel_restart_button_text;
            localizedText["death_panel_returntomainmenu_button"] = data.death_panel_returntomainmenu_button_text;
            localizedText["death_panel_exittodesktop_button"] = data.death_panel_exittodesktop_button_text;
            localizedText["death_panel_minutes_survived"] = data.death_panel_minutes_survived_text;
            localizedText["death_panel_total_gold"] = data.death_panel_total_gold_text;
            //localizedText["death_panel_bounties"] = data.death_panel_bounties_text;

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
        PlayerPrefs.SetString("SelectedLanguage", language);
        PlayerPrefs.Save();

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
        ApplyLanguageToUI();
        OnLanguageChanged?.Invoke();
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
        public string upgrades_critical_hit_damage;
        public string upgrades_multistrike_chance;
        public string upgrades_experience_gain;
        public string upgrades_basic_turret;
        public string upgrades_electric_turret;
        public string upgrades_fire_turret;
        public string upgrades_ice_turret;
        public string upgrades_wind_turret;
        public string upgrades_boss_kill_score;
        public string upgrades_button_text;
        public string death_panel_restart_button_text;
        public string death_panel_returntomainmenu_button_text;
        public string death_panel_exittodesktop_button_text;
        public string death_panel_minutes_survived_text;
        public string death_panel_total_gold_text;
        //public string death_panel_bounties_text;
        //public string
        //public string
        //public string
    }
}