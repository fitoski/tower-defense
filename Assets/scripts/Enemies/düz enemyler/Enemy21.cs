using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy21 : Enemy
{
    new void Start()
    {
        base.Start();
        maxHealth = 310;
        speed = 3f;
        baseDamage = 20;
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
