using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CircularObjectSkill", menuName = "Skills/CircularObjectSkill", order = 1)]
public class CircularObjectSkill : PassiveSkill
{
    public int baseDamage;
    public int damagePerLevel;
    public GameObject prefab;
    public float orbitDistance = 5.0f;
    public float orbitDegreesPerSec = 180f;
    public float maxAngle = 360;
    private CircularObjectController controller;
    public override void ActivatePassiveSkillInternal(PlayerSkills player)
    {
        GameObject instancedObj = Instantiate(prefab, player.transform.position, Quaternion.identity);
        controller = instancedObj.GetComponent<CircularObjectController>();
        controller.InitObject(orbitDistance, orbitDegreesPerSec, maxAngle, baseDamage);
    }

    public override void InitializeSkillInternal()
    {

    }
}