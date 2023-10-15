using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFast : MonoBehaviour
{
    [Header("Attributes of Enemy Fast")]
    [SerializeField] public int maxHealth = 10;
    [SerializeField] public float speed = 5f;
    [SerializeField] public int baseDamage = 1;
    [SerializeField] public int scoreValue = 15;
    [SerializeField] public int experiencePointsValue = 10;
    [SerializeField] public float damageMultiplierPerWave = 1.5f;

    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

}