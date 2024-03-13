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
    public TextMeshProUGUI upgradesCriticalHitDamageText;
    public TextMeshProUGUI upgradesMultistrikeChanceText;
    public TextMeshProUGUI upgradesExperienceGainText;
    public TextMeshProUGUI upgradesBasicTurretText;
    public TextMeshProUGUI upgradesElectricTurretText;
    public TextMeshProUGUI upgradesFireTurretText;
    public TextMeshProUGUI upgradesIceTurretText;
    public TextMeshProUGUI upgradesWindTurretText;
    public TextMeshProUGUI upgradesBossKillScoreText;
    public TextMeshProUGUI upgradesButtonText;


    public TMP_Dropdown languageDropdown;
    public TMP_Dropdown qualityDropdown;

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
        upgradesCriticalHitDamageText.text = LocalizationManager.Instance.GetLocalizedValue("upgrades_critical_hit_damage");
        upgradesMultistrikeChanceText.text = LocalizationManager.Instance.GetLocalizedValue("upgrades_multistrike_chance");
        upgradesExperienceGainText.text = LocalizationManager.Instance.GetLocalizedValue("upgrades_experience_gain");
        upgradesBasicTurretText.text = LocalizationManager.Instance.GetLocalizedValue("upgrades_basic_turret");
        upgradesElectricTurretText.text = LocalizationManager.Instance.GetLocalizedValue("upgrades_electric_turret");
        upgradesFireTurretText.text = LocalizationManager.Instance.GetLocalizedValue("upgrades_fire_turret");
        upgradesIceTurretText.text = LocalizationManager.Instance.GetLocalizedValue("upgrades_ice_turret");
        upgradesWindTurretText.text = LocalizationManager.Instance.GetLocalizedValue("upgrades_wind_turret");
        upgradesButtonText.text = LocalizationManager.Instance.GetLocalizedValue("upgrades_button_text");
    }
}