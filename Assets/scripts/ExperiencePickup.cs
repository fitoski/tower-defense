using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperiencePickup : MonoBehaviour
{
    public int experienceAmount = 1; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.main.IncreaseExperiencePoints(experienceAmount);
            Destroy(gameObject);
        }
    }
}
