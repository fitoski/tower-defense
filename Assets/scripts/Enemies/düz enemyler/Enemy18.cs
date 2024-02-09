using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy18 : Enemy
{
    new void Start()
    {
        base.Start();
        maxHealth = 140;
        speed = 5f;
        baseDamage = 4;
        scoreValue = 15;
        damageMultiplierPerWave = 1.5f;
        currentHealth = maxHealth;
    }
}
