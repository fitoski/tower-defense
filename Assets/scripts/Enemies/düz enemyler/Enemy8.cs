using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy8 : Enemy
{
    new void Start()
    {
        base.Start();
        maxHealth = 110;
        speed = 5f;
        baseDamage = 5;
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
