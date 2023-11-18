using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    private PlayerMovement player;

    void Start()
    {
        player = GetComponentInParent<PlayerMovement>();
    }

    public void Attack()
    {
        player.Attack();
    }
}
