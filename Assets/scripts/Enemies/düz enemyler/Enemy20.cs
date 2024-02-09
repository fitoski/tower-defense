using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy20 : Enemy
{
    new void Start()
    {
        base.Start();
        maxHealth = 150;
        speed = 5f;
        baseDamage = 12;
        scoreValue = 15;
        damageMultiplierPerWave = 1.5f;
        currentHealth = maxHealth;
    }
}
