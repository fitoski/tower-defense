using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularObjectController : MonoBehaviour
{
    private float orbitDistance;
    private float orbitDegreesPerSec;
    private float maxAngle;
    private int damage;

    private Transform player;
    private float currentAngle = 0;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        transform.position = player.transform.position + new Vector3(orbitDistance, 0, 0);
    }

    public void InitObject(float orbitDistance, float orbitDegreesPerSec, float maxAngle, int damage)
    {
        this.orbitDistance = orbitDistance;
        this.orbitDegreesPerSec = orbitDegreesPerSec;
        this.maxAngle = maxAngle;
        this.damage = damage;
    }

    void Update()
    {
        if (player != null)
        {
            float angle = orbitDegreesPerSec * Time.deltaTime;

            Quaternion rot = Quaternion.Euler(0, 0, angle);

            Vector3 offset = player.position + (transform.position - player.position).normalized * orbitDistance;

            transform.position = player.position + rot * (offset - player.position);

            currentAngle += angle;
        }

        if (currentAngle > maxAngle)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Enemy>().TakeDamage(damage);
        }
    }
}