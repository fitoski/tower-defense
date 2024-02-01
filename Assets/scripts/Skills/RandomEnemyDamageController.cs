using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEnemyDamageController : MonoBehaviour
{
    public int damage = 30;
    public float interval = 15f;
    private float lastAttackTime = -999f;
    public GameObject lightningEffectPrefab;
    public float effectDuration = 2f;

    void Update()
    {
        if (Time.time >= lastAttackTime + interval)
        {
            lastAttackTime = Time.time;
            AttackRandomEnemies();
        }
    }

    void AttackRandomEnemies()
    {
        List<GameObject> enemiesInView = GetEnemiesInView();
        int enemiesCount = enemiesInView.Count;

        if (enemiesCount > 0)
        {
            int attacks = Mathf.Min(2, enemiesCount);
            for (int i = 0; i < attacks; i++)
            {
                int randomIndex = UnityEngine.Random.Range(0, enemiesInView.Count);
                GameObject enemy = enemiesInView[randomIndex];
                if (enemy != null)
                {
                    enemy.GetComponent<Enemy>().TakeDamage(damage);
                    CreateLightningEffect(enemy.transform.position);
                    enemiesInView.RemoveAt(randomIndex);
                }
            }
        }
    }

    List<GameObject> GetEnemiesInView()
    {
        List<GameObject> enemiesInView = new List<GameObject>();
        Camera cam = Camera.main;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            Vector3 viewPos = cam.WorldToViewportPoint(enemy.transform.position);
            if (viewPos.x > 0 && viewPos.x < 1 && viewPos.y > 0 && viewPos.y < 1)
            {
                enemiesInView.Add(enemy);
            }
        }
        return enemiesInView;
    }

    void CreateLightningEffect(Vector3 position)
    {
        GameObject effectInstance = Instantiate(lightningEffectPrefab, position, Quaternion.identity);
        Destroy(effectInstance, effectDuration);
    }

    public void DestroyGameObject()
    {
        Destroy(gameObject);
    }
}