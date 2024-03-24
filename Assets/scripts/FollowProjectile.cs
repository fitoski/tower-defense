using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowProjectile : MonoBehaviour
{
    public float speed = 5f;
    public int damage = 10;
    public float followDuration = 5f;
    private Transform target;
    private float timer;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform projectileSpawnPoint;

    public void Initialize(Transform target, float duration)
    {
        this.target = target;
        this.followDuration = duration;
        timer = duration;
    }

    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            if (target != null)
            {
                var direction = (target.position - transform.position).normalized;

                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

                transform.position += direction * speed * Time.deltaTime;
            }
        }
        else
        {
            FadeOutAndDestroy();
        }
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.gameObject.CompareTag("Player"))
        {
            var player = hitInfo.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }

    private void FadeOutAndDestroy()
    {
        Destroy(gameObject);
    }
}
