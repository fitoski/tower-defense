using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy13 : Enemy
{
    new void Start()
    {
        base.Start();
        maxHealth = 210;
        speed = 3f;
        baseDamage = 10;
        scoreValue = 15;
        damageMultiplierPerWave = 1.5f;
        currentHealth = maxHealth;
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void Die()
    {
        base.Die();
    }
}
