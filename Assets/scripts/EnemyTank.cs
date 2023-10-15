using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTank : MonoBehaviour
{
    [Header("Attributes of Enemy Tank")]
    [SerializeField] public int maxHealth = 50;
    [SerializeField] public float speed = 1f;
    [SerializeField] public int baseDamage = 3;
    [SerializeField] public int scoreValue = 30;
    [SerializeField] public int experiencePointsValue = 20;
    [SerializeField] public float damageMultiplierPerWave = 1.5f;

    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

}