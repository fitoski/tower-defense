using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy11 : Enemy
{
    new void Start()
    {
        base.Start();
        maxHealth = 105;
        speed = 7f;
        baseDamage = 4;
        scoreValue = 15;
        damageMultiplierPerWave = 1.5f;
        currentHealth = maxHealth;
    }
}
