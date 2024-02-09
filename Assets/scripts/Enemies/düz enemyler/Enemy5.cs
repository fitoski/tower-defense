using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy5 : Enemy
{
    new void Start()
    {
        base.Start();
        maxHealth = 75;
        speed = 3f;
        baseDamage = 5;
        scoreValue = 15;
        damageMultiplierPerWave = 1.5f;
        currentHealth = maxHealth;
    }
}
