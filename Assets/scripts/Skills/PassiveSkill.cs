using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PassiveSkill : BaseSkill
{
    public float cooldown = 5;
    protected float lastUsed;

    public void ActivatePassiveSkill(PlayerSkills player)
    {
        if (lastUsed < Time.time - cooldown)
        {
            lastUsed = Time.time;
            ActivatePassiveSkillInternal(player);
        }
    }
    public abstract void ActivatePassiveSkillInternal(PlayerSkills player);

    public void InitializeSkill()
    {
        lastUsed = -999;
        InitializeSkillInternal();
    }

    public abstract void InitializeSkillInternal();
}
