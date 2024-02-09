using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy24 : Enemy
{
    new void Start()
    {
        base.Start();
        maxHealth = 175;
        speed = 5f;
        baseDamage = 20;
        scoreValue = 15;
        damageMultiplierPerWave = 1.5f;
        currentHealth = maxHealth;
    }
}
