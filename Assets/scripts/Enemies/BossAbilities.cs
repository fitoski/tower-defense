using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAbilities : MonoBehaviour
{
    public float specialAttackCooldown = 10f;
    public float anotherAbilityCooldown = 1f;

    public void SpecialAttack()
    {
        Debug.Log("Boss1: Özel saldırı gerçekleştirildi!");
    }

    public void AnotherAbility()
    {
        Debug.Log("Boss1: Diğer yetenek aktif!");
    }
}
