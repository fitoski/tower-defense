using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy25 : Enemy
{
    new void Start()
    {
        base.Start();
        maxHealth = 180;
        speed = 6f;
        baseDamage = 12;
        scoreValue = 15;
        damageMultiplierPerWave = 1.5f;
        currentHealth = maxHealth;
    }
}
