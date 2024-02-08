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
        if (skill != null) 
        {
            this.skill = skill;
            skillName.text = skill.skillName; 
            skillDescription.text = skill.description;
            skillIcon.sprite = skill.icon;
        }
        else
        {
            skillName.text = "";
            skillDescription.text = "";
            skillIcon.sprite = null;
        }
    }

    public void SelectSkill()
    {
        if (skill != null) 
        {
            Debug.Log("SelectSkill called.");
            PlayerSkills playerSkills = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerSkills>();
            if (playerSkills != null) 
            {
                playerSkills.addSkillToPassiveSkills(skill);
            }

            SkillsManager.Instance.CloseSkillRewardPanel();
            Time.timeScale = 1;
        }
    }
}