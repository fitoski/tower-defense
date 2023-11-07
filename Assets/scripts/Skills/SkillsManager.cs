using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillsManager : MonoBehaviour
{
    public static SkillsManager Instance { get; private set; }
    

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public List<BaseSkill> skills = new();

    private PlayerSkills playerSkills;
    void Start()
    {
        playerSkills = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerSkills>();

        foreach (BaseSkill skill in skills)
        {
            if (skill is PassiveSkill)
            {
                playerSkills.addSkillToPassiveSkills((PassiveSkill) skill);
            }
        }
    }
}
