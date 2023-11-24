using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Core : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public Image coreHealthBar;


    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Debug.Log("Core Destroyed! Game Over.");
        }

        UpdateHealthBars();

    }

    void UpdateHealthBars()
    {
        float coreHealthRatio = (float)currentHealth / maxHealth;
        coreHealthBar.fillAmount = coreHealthRatio;
    }
}