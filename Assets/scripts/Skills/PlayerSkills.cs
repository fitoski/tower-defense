using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkills : MonoBehaviour
{
    private List<PassiveSkill> passiveSkills = new List<PassiveSkill>();

    public void addSkillToPassiveSkills(PassiveSkill skill)
    {
        passiveSkills.Add(skill);
        skill.InitializeSkill();
    }

    private void Update()
    {
        
        foreach (var skill in passiveSkills)
        {
            skill.ActivatePassiveSkill(this);
        }
    }
}