using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Trader;

public class SkillsManager : MonoBehaviour
{
    public static SkillsManager Instance { get; private set; }
    public GameObject skillRewardPanel;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public List<PassiveSkill> skills = new();

    [SerializeField] private SkillButton[] skillRewardButtons;

    private void Start()
    {

    }

    public void PrepareSkillRewardPanel()
    {
        Debug.Log("PrepareSkillRewardPanel called.");

        if (skillRewardButtons.Length == 0) return;

        foreach (var button in skillRewardButtons)
        {
            button.GetComponent<Button>().interactable = true;
        }
        skillRewardPanel.SetActive(true);
        GameManager.main.isSkillPanelOpen = true;
        Time.timeScale = 0;
        skillRewardButtons[0].transform.parent.parent.gameObject.SetActive(true);
        List<PassiveSkill> availableSkills = new List<PassiveSkill>(skills);
        for (int i = 0; i < skillRewardButtons.Length && availableSkills.Count > 0; i++)
        {
            int randomIndex = Random.Range(0, availableSkills.Count);
            skillRewardButtons[i].UpdateUI(availableSkills[randomIndex]);
            availableSkills.RemoveAt(randomIndex);
        }
    }

    public void BossDefeated()
    {
        skillRewardPanel.SetActive(true);
        PrepareSkillRewardPanel();
    }

    public void CloseSkillRewardPanel()
    {
        Debug.Log("CloseSkillRewardPanel called.");
        skillRewardPanel.SetActive(false);
        GameManager.main.isSkillPanelOpen = false;
        GameManager.main.CheckAndResumeGame();
    }

    public void OnBossDeath()
    {
        SkillsManager.Instance.BossDefeated();
    }
}