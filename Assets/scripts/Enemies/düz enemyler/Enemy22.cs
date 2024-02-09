using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy22 : Enemy
{
    new void Start()
    {
        base.Start();
        maxHealth = 160;
        speed = 6f;
        baseDamage = 12;
        scoreValue = 15;
        damageMultiplierPerWave = 1.5f;
        currentHealth = maxHealth;
    }
}
