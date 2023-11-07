using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ThrowProjectileSkill", menuName = "Skills/ThrowProjectileSkill", order = 2)]
public class ThrowProjectileSkill : PassiveSkill
{
    public float baseDamage;
    public float damagePerLevel;
    public float distance;
    public float speed;
    public GameObject prefab;

    private ThrowableProjectileControl controller;

    public override void ActivatePassiveSkillInternal(PlayerSkills player)
    {
        GameObject instancedObj = Instantiate(prefab, player.transform.position, Quaternion.identity);
        controller = instancedObj.GetComponent<ThrowableProjectileControl>();
        controller.InitObject(distance, speed);
    }

    public override void InitializeSkillInternal()
    {

    }
}
