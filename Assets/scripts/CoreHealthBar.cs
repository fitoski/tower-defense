using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoreHealthBar : MonoBehaviour
{
    public Core core; 
    public Image healthBar;  

    void Update()
    {
        if (core != null)
        {
            float healthRatio = (float)core.currentHealth / core.maxHealth;
            healthBar.fillAmount = healthRatio;
        }
    }
}
