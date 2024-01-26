using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Chest Trigger Entered");
        if (other.CompareTag("Player"))
        {
            SkillsManager.Instance.BossDefeated();
            Destroy(gameObject); 
        }
    }
}
