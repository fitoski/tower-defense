using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Turret : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firingPoint;
    [SerializeField] private Animator animator;

    [Header("Attributes")]
    [SerializeField] private float targetingRange = 6f;
    [SerializeField] private float bps = 1f;
    public string turretName = "Turret";
    public Sprite turretIcon;

    [Header("Cost")]
    [SerializeField] private int cost;
    public int Cost => cost;

    private Transform target;
    private float timeUntilFire;

    private void Update()
    {
        if (target == null || !CheckTargetIsInRange() || TargetIsDead())
        {
            FindTarget();
        }

        if (target != null)
        {
            timeUntilFire += Time.deltaTime;
            if (timeUntilFire >= 1f / bps)
            {
                Shoot();
                timeUntilFire = 0f;
            }
        }
    }

    private void Shoot()
    {
        //GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
        //Bullet bulletScript = bulletObj.GetComponent<Bullet>();
        //bulletScript.SetTarget(target);

        animator.SetTrigger("Shoot");
    }

    public void OnShootAnimationEvent()
    {
        if (target != null)
        {
            Vector2 direction = (target.position - firingPoint.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg  + 180f;

            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, rotation);
            Bullet bulletScript = bulletObj.GetComponent<Bullet>();
            bulletScript.SetTarget(target, direction);
        }
    }

    private void FindTarget()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, targetingRange, enemyMask);
        Transform nearestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (Collider2D hitCollider in hitColliders)
        {
            Enemy enemy = hitCollider.GetComponent<Enemy>();
            if (enemy != null && !enemy.IsDead)
            {
                Vector3 directionToTarget = hitCollider.transform.position - currentPosition;
                float dSqrToTarget = directionToTarget.sqrMagnitude;
                if (dSqrToTarget < closestDistanceSqr)
                {
                    closestDistanceSqr = dSqrToTarget;
                    nearestTarget = hitCollider.transform;
                }
            }
        }

        target = nearestTarget;
    }

    private bool CheckTargetIsInRange()
    {
        if (target == null)
            return false;

        return Vector2.Distance(target.position, transform.position) <= targetingRange;
    }

    private bool TargetIsDead()
    {
        if (target == null)
            return false;

        Enemy enemy = target.GetComponent<Enemy>();
        if (enemy != null)
        {
            return enemy.IsDead;
        }
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);
    }
}