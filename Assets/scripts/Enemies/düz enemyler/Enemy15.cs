using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy15 : Enemy
{
    new void Start()
    {
        base.Start();
        maxHealth = 200;
        speed = 6f;
        baseDamage = 4;
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
