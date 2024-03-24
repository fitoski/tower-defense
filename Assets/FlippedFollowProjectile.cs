using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlippedFollowProjectile : MonoBehaviour
{
    public float speed = 5f;
    public int damage = 10;
    public float followDuration = 5f;
    private Transform target;
    private float timer;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform projectileSpawnPoint;

    void Start()
    {
        if (target != null && projectileSpawnPoint != null)
        {
            if (target.position.x < projectileSpawnPoint.position.x)
            {
                transform.localScale = new Vector3(transform.localScale.x , transform.localScale.y * -1, transform.localScale.z);
            }
        }
    }

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
                Vector3 direction = (target.position - transform.position).normalized;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                // Projektilin yönünü hesaplayarak ve sprite'ın dikey eksende (y ekseni) flip edilip edilmeyeceğine karar ver.
                if (target.position.x < transform.position.x) // Eğer hedef projektilin solundaysa
                {
                    // Y ekseninde flip yap
                    transform.localScale = new Vector3(transform.localScale.x, -Mathf.Abs(transform.localScale.y), transform.localScale.z);
                }
                else // Hedef sağ taraftaysa veya yatay eksende flip yapılmasına gerek yoksa
                {
                    transform.localScale = new Vector3(transform.localScale.x, Mathf.Abs(transform.localScale.y), transform.localScale.z);
                }

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
