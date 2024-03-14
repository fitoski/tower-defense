using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperiencePickup : MonoBehaviour
{
    public static int experienceAmount = 100;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"{experienceAmount}: baseExperienceAmount Pickup");
            GameManager.main.EnqueueExperience(experienceAmount);
            Destroy(gameObject);
        }
    }

    public static void IncreaseBaseExperienceAmount(float ratio)
    {
        experienceAmount = Mathf.FloorToInt(ratio * experienceAmount);
        Debug.Log($"{experienceAmount}: baseExperienceAmount");
    }
}