using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

[System.Serializable]
public class Upgrade
{
    public string name;
    public string displayName;
    public int cost;
    public int level;

    public Upgrade(string name, string displayName, int cost)
    {
        this.name = name;
        this.displayName = displayName;
        this.cost = cost;
        this.level = 0;
    }
}

public class UpgradesManager : MonoBehaviour
{
    [System.Serializable]
    public class UpgradeUI
    {
        public Button upgradeButton;
        public TextMeshProUGUI nameText;
        public TextMeshProUGUI countText;
    }

    public GameObject upgradesPanel;
    public List<Upgrade> upgrades;
    public GameManager gameManager;
    public List<UpgradeUI> upgradeUIs;
    public TextMeshProUGUI bossKillScoreText;
    private bool needToUpdateUI = true;
    private bool isUpgrading = false;

    [Header("Bullet Upgrade UI Elements")]
    public TextMeshProUGUI electricBulletLevelText;
    public TextMeshProUGUI fireBulletLevelText;
    public TextMeshProUGUI iceBulletLevelText;
    public TextMeshProUGUI windBulletLevelText;

    void Start()
    {
        LoadUpgrades();
        InitializeTurretUpgrades();
        UpdateUI();
    }

    void Update()
    {
        if (needToUpdateUI)
        {
            UpdateUI();
            needToUpdateUI = false;
        }
    }

    IEnumerator SavePlayerPrefsWithDelay(float delay)
    {
        PlayerPrefs.Save();
        yield return new WaitForSeconds(delay);
    }

    public void BuyUpgrade(int index)
    {
        if (index < 0 || index >= upgrades.Count) return;

        Upgrade upgrade = upgrades[index];
        int bossKills = ScoreManager.GetBossKills();

        if (bossKills < 1)
        {
            Debug.Log("Not enough Boss Kills to buy the upgrade.");
            return;
        }

        Debug.Log($"Attempting to buy upgrade: {upgrade.name}, Current Boss Kills: {bossKills}");

        ScoreManager.Instance.DecreaseBossKillScore(1);
        Debug.Log($"After DecreaseBossKillScore, Boss Kills: {ScoreManager.GetBossKills()}");
        upgrade.level++;
        PlayerPrefs.SetInt(upgrade.name + "_Level", upgrade.level);
        PlayerPrefs.Save();
        ApplyUpgrade(index);
        UpdateUI();
    }

    IEnumerator SaveAndFinishUpgrade()
    {
        PlayerPrefs.Save();
        yield return new WaitForSeconds(0.1f);
        UpdateUI();
        isUpgrading = false;
    }

    void LoadUpgrades()
    {
        for (int i = 0; i < upgrades.Count; i++)
        {
            upgrades[i].level = PlayerPrefs.GetInt(upgrades[i].name + "_Level", 0);
        }
    }

    void ApplyUpgrade(int index)
    {
        PlayerMovement player = FindObjectOfType<PlayerMovement>();
        if (player != null)
        {
            switch (index)
            {
                case 0:
                    player.IncreaseCriticalHitDamage(5f);
                    break;
                case 1:
                    player.IncreaseDoubleAttackChance();
                    break;
                case 2:
                    GameManager.main.BuyXpUpgrade();
                    Debug.Log("Experience Gain Upgrade purchased. Current Level: " + GameManager.main.xpUpgradeLevel);
                    break;
            }
        }
    }

    void UpdateUI()
    {
        for (int i = 0; i < upgrades.Count; i++)
        {
            int index = i;
            upgradeUIs[i].nameText.text = upgrades[i].displayName;
            upgradeUIs[i].countText.text = "Level: " + upgrades[i].level;
            upgradeUIs[i].upgradeButton.onClick.RemoveAllListeners();
            //upgradeUIs[i].upgradeButton.onClick.AddListener(() => BuyUpgrade(index));
        }
        bossKillScoreText.text = "Bounties: " + ScoreManager.GetBossKills().ToString();
        UpdateBulletUpgradeUI();
    }

    public void OpenUpgradesPanel()
    {
        if (upgradesPanel != null)
        {
            upgradesPanel.SetActive(true);
        }
    }

    public void CloseUpgradesPanel()
    {
        if (upgradesPanel != null)
        {
            upgradesPanel.SetActive(false);
        }
    }

    [System.Serializable]
    public class TurretUpgrade
    {
        public string turretName;
        public int level = 0;
    }

    public List<TurretUpgrade> turretUpgrades;

    void InitializeTurretUpgrades()
    {
        foreach (var turret in GameManager.main.AvailableTurrets)
        {
            var turretScript = turret.GetComponent<Turret>();
            if (turretScript != null)
            {
                TurretUpgrade newUpgrade = new TurretUpgrade
                {
                    turretName = turretScript.turretName
                };
                turretUpgrades.Add(newUpgrade);
            }
        }
    }

    public void BuyTurretUpgrade(int index)
    {
        if (index < 0 || index >= turretUpgrades.Count) return;

        TurretUpgrade upgrade = turretUpgrades[index];
        int bossKills = ScoreManager.GetBossKills();
        if (bossKills >= 1)
        {
            ScoreManager.Instance.DecreaseBossKillScore(1);
            upgrade.level++;
            PlayerPrefs.SetInt(upgrade.turretName + "_Level", upgrade.level);
            PlayerPrefs.Save();
            ApplyTurretUpgrade(upgrade.turretName, upgrade.level);
            UpdateUI();
        }
        else
        {
            Debug.Log("Not enough Boss Kills to buy the upgrade.");
        }
    }

    void ApplyTurretUpgrade(string turretName, int level)
    {
        foreach (var turretGameObject in GameManager.main.AvailableTurrets)
        {
            Turret turretScript = turretGameObject.GetComponent<Turret>();
            if (turretScript != null && turretScript.turretName == turretName)
            {
                GameObject bulletPrefab = Instantiate(turretScript.BulletPrefab);
                Bullet bulletScript = bulletPrefab.GetComponent<Bullet>();

                turretScript.TargetingRange += level;
                turretScript.Bps += level;

                if (bulletScript != null)
                {
                    bulletScript.BulletDamage += level;
                    bulletScript.BulletSpeed += level;
                }
                break;
            }
        }
    }

    public void BuyBulletUpgrade(string bulletName)
    {
        int bossKills = ScoreManager.GetBossKills();
        if (bossKills >= 1)
        {
            int currentLevel = PlayerPrefs.GetInt(bulletName + "_Level", 0) + 1;
            PlayerPrefs.SetInt(bulletName + "_Level", currentLevel);
            ScoreManager.Instance.DecreaseBossKillScore(1);
            ApplyBulletUpgrade(bulletName, currentLevel);
            UpdateUI();
        }
        else
        {
            Debug.Log("Not enough Boss Kills to buy the upgrade.");
        }
    }

    public void BuyElectricBulletUpgrade()
    {
        BuyBulletUpgrade("ElectricBullet");
        UpdateUI();
    }

    public void BuyFireBulletUpgrade()
    {
        BuyBulletUpgrade("FireBullet");
        UpdateUI();
    }

    public void BuyIceBulletUpgrade()
    {
        BuyBulletUpgrade("IceBullet");
        UpdateUI();
    }

    public void BuyWindBulletUpgrade()
    {
        BuyBulletUpgrade("WindBullet");
        UpdateUI();
    }

    void UpdateBulletUpgradeUI()
    {
        electricBulletLevelText.text = "Level: " + PlayerPrefs.GetInt("ElectricBullet_Level", 0).ToString();
        fireBulletLevelText.text = "Level: " + PlayerPrefs.GetInt("FireBullet_Level", 0).ToString();
        iceBulletLevelText.text = "Level: " + PlayerPrefs.GetInt("IceBullet_Level", 0).ToString();
        windBulletLevelText.text = "Level: " + PlayerPrefs.GetInt("WindBullet_Level", 0).ToString();
    }

    void ApplyBulletUpgrade(string bulletName, int level)
    {
        switch (bulletName)
        {
            case "ElectricBullet":
                PlayerPrefs.SetInt("ElectricBullet_BulletDamageLevel", level);
                break;
            case "FireBullet":
                PlayerPrefs.SetInt("FireBullet_BulletDamageLevel", level);
                break;
            case "IceBullet":
                PlayerPrefs.SetInt("IceBullet_BulletDamageLevel", level);
                break;
            case "WindBullet":
                PlayerPrefs.SetInt("WindBullet_BulletDamageLevel", level);
                break;
        }
        PlayerPrefs.Save();
    }
}