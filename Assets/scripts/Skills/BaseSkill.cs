using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public abstract class BaseSkill : ScriptableObject
{
    public string skillName;
    public string description;
    public Sprite icon;
    protected int level = 1;
    public int maxLevel;

    public virtual void LevelUp()
    {
        if (level < maxLevel)
        {
            level++;
        }
    }
}
