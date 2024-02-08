using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFast : Enemy
{
    new void Start()
    {
        base.Start();
        maxHealth = 65;
        speed = 2.5f;
        baseDamage = 2;
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