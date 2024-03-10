using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    public TextMeshProUGUI bossKillScoreText;

    void Awake()
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
    }

    private void Start()
    {
        UpdateBossKillScoreUI();
    }

    public void IncreaseBossKills()
    {
        int currentKills = PlayerPrefs.GetInt("BossKills", 0);
        currentKills++;
        PlayerPrefs.SetInt("BossKills", currentKills);
        PlayerPrefs.Save();
        UpdateBossKillScoreUI();
    }

    public void DecreaseBossKillScore(int amount)
    {
        int currentKills = Mathf.Max(0, PlayerPrefs.GetInt("BossKills", 0) - amount);
        PlayerPrefs.SetInt("BossKills", currentKills);
        PlayerPrefs.Save();
        UpdateBossKillScoreUI();
    }

    private void UpdateBossKillScoreUI()
    {
        if (bossKillScoreText != null)
        {
            bossKillScoreText.text = "Boss Kills: " + GetBossKills().ToString();
        }
    }

    public static int GetBossKills()
    {
        return PlayerPrefs.GetInt("BossKills", 0);
    }

    public void DisplayBossKills()
    {
        int bossKills = PlayerPrefs.GetInt("BossKills", 0);
        bossKillScoreText.text = "Boss Kills: " + bossKills.ToString();
    }
}