using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy6 : Enemy
{
    new void Start()
    {
        base.Start();
        maxHealth = 80;
        speed = 6f;
        baseDamage = 3;
        scoreValue = 15;
        damageMultiplierPerWave = 1.5f;
        currentHealth = maxHealth;
    }
}
