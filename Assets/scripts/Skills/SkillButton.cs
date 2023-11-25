using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    private PassiveSkill skill;
    [SerializeField] private TMP_Text skillName;
    [SerializeField] private TMP_Text skillDescription;
    [SerializeField] private Image skillIcon;
    [SerializeField] private Button button; 

    public void UpdateUI(PassiveSkill skill)
    {
        this.skill = skill;
        skillName.text = skill.name;
        skillDescription.text = skill.description;
        skillIcon.sprite = skill.icon;
    }

    public void SelectSkill()
    {
        Debug.Log("SelectSkill called.");
        PlayerSkills playerSkills = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerSkills>();
        playerSkills.addSkillToPassiveSkills(skill);

        //transform.parent.parent.gameObject.SetActive(false);
        SkillsManager.Instance.CloseSkillRewardPanel();
        Time.timeScale = 1;
    }
}
