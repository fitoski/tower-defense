using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDefault : Enemy
{
    new void Start()
    {
        base.Start();
        maxHealth = 60;
        speed = 2f;
        baseDamage = 1;
        scoreValue = 15;
        damageMultiplierPerWave = 1.5f;
        currentHealth = maxHealth;
    }
}