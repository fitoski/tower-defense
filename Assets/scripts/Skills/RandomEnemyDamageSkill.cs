using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RandomEnemyDamageSkill", menuName = "Skills/RandomEnemyDamageSkill", order = 4)]
public class RandomEnemyDamageSkill : PassiveSkill
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
