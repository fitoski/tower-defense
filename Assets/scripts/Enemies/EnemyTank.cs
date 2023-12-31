using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTank : Enemy
{
    new void Start()
    {
        base.Start();
        maxHealth = 10;
        speed = 5f;
        baseDamage = 1;
        scoreValue = 15;
        damageMultiplierPerWave = 1.5f;
        currentHealth = maxHealth;
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void Die()
    {
        base.Die();
    }
}