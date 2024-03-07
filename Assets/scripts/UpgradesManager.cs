using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class Upgrade
{
    public string name;
    public int cost;
    public int level;

    public Upgrade(string name, int cost)
    {
        this.name = name;
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

    void Start()
    {
        LoadUpgrades();
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


    public void BuyUpgrade(int index)
    {
        if (index < 0 || index >= upgrades.Count) return;

        Upgrade upgrade = upgrades[index];
        int bossKills = ScoreManager.GetBossKills();
        if (bossKills >= 1)
        {
            ScoreManager.Instance.DecreaseBossKillScore(1);
            upgrade.level++;
            PlayerPrefs.SetInt(upgrade.name + "_Level", upgrade.level);
            PlayerPrefs.Save();
            ApplyUpgrade(index);
            UpdateUI();
        }
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
            upgradeUIs[i].nameText.text = upgrades[i].name;
            upgradeUIs[i].countText.text = "Level: " + upgrades[i].level;
            upgradeUIs[i].upgradeButton.onClick.RemoveAllListeners();
            upgradeUIs[i].upgradeButton.onClick.AddListener(() => BuyUpgrade(index));
        }
        bossKillScoreText.text = "Boss Kills: " + ScoreManager.GetBossKills().ToString();
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
}
