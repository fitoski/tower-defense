using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ThrowAndGetBackController : MonoBehaviour
{
    private float speed;
    private float distance;
    private int damage;
    private Transform player;
    private Vector2 firstPos;
    private Vector2 dir;


    private bool isGoingBack = false;
    public void InitObject(float distance, float speed, int damage)
    {
        this.speed = speed;
        this.distance = distance;
        this.damage = damage;
    }
    void Start()
    {
        Vector2? target = FindTargetPosition();

        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        player = GameObject.FindGameObjectWithTag("Player").transform;
        transform.position = player.position;
        firstPos = transform.position;
        distance = Vector2.Distance((Vector2)target, transform.position);
        dir = ((Vector2)target - (Vector2)transform.position).normalized;

        float lookAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, lookAngle - 90);
    }

    void Update()
    {
        if (Vector2.Distance(transform.position, firstPos) > distance)
        {
            if (isGoingBack)
            {
                Destroy(gameObject);
            }
            else
            {
                isGoingBack = true;
                dir = ((Vector2)player.position - (Vector2)transform.position).normalized;
                firstPos = transform.position;
                float lookAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, lookAngle - 90);
            }
        }

        transform.position += speed * Time.deltaTime * (Vector3)dir;
    }

    private Vector2? FindTargetPosition()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject cloesetEnemy = null;
        float minDistance = Mathf.Infinity;
        Vector2 currentPosition = transform.position;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(enemy.transform.position, currentPosition);
            if (distance < minDistance)
            {
                cloesetEnemy = enemy;
                minDistance = distance;
            }
        }

        if (cloesetEnemy != null)
        {
            return cloesetEnemy.transform.position;
        }

        return null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Enemy>().TakeDamage(damage);
        }
    }
}