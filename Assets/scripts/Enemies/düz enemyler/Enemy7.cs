using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy7 : Enemy
{
    new void Start()
    {
        base.Start();
        maxHealth = 85;
        speed = 6f;
        baseDamage = 2;
        scoreValue = 15;
        damageMultiplierPerWave = 1.5f;
        currentHealth = maxHealth;
    }
}
