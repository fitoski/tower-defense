using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperiencePickup : MonoBehaviour
{
    public static int baseExperienceAmount = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.main.EnqueueExperience(baseExperienceAmount);
            Destroy(gameObject);
        }
    }

    public static void IncreaseBaseExperienceAmount()
    {
        baseExperienceAmount += 1; 
    }
}