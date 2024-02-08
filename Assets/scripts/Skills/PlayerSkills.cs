using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkills : MonoBehaviour
{
    private List<PassiveSkill> passiveSkills = new List<PassiveSkill>();

    public void addSkillToPassiveSkills(PassiveSkill skill)
    {
        if (skill != null) 
        {
            passiveSkills.Add(skill);
            skill.InitializeSkill();
        }
    }

    private void Update()
    {
        foreach (var skill in passiveSkills)
        {
            if (skill != null) 
            {
                skill.ActivatePassiveSkill(this);
            }
        }
    }
}