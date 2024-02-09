using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy4 : Enemy
{
    new void Start()
    {
        base.Start();
        maxHealth = 70;
        speed = 2f;
        baseDamage = 3;
        scoreValue = 15;
        damageMultiplierPerWave = 1.5f;
        currentHealth = maxHealth;
    }
}
