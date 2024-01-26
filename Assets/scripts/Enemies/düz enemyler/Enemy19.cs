using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy19 : Enemy
{
    new void Start()
    {
        base.Start();
        maxHealth = 250;
        speed = 7f;
        baseDamage = 8;
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
