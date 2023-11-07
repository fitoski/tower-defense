using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ThrowAndGetBackSkill", menuName = "Skills/ThrowAndGetBackSkill", order = 3)]
public class ThrowAndGetBackSkill : PassiveSkill
{
    public float baseDamage;
    public float damagePerLevel;
    public GameObject prefab;

    public float speed;
    public float distance;
    private ThrowAndGetBackController controller;
    public override void ActivatePassiveSkillInternal(PlayerSkills player)
    {
        GameObject instancedObj = Instantiate(prefab, player.transform.position, Quaternion.identity);
        controller = instancedObj.GetComponent<ThrowAndGetBackController>();
        controller.InitObject(distance, speed);
    }

    public override void InitializeSkillInternal()
    {

    }
}
