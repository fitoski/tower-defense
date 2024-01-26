using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PulseDamageSkill", menuName = "Skills/PulseDamageSkill", order = 5)]
public class PulseDamageSkill : PassiveSkill
{
    public GameObject prefab;

    public override void ActivatePassiveSkillInternal(PlayerSkills player)
    {
        Instantiate(prefab, player.transform.position, Quaternion.identity);
    }

    public override void InitializeSkillInternal()
    {

    }
}