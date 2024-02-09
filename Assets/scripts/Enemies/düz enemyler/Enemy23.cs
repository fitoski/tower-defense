using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy23 : Enemy
{
    new void Start()
    {
        base.Start();
        maxHealth = 170;
        speed = 6f;
        baseDamage = 8;
        scoreValue = 15;
        damageMultiplierPerWave = 1.5f;
        currentHealth = maxHealth;
    }
}
