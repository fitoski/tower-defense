using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Core : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public Image coreHealthBar;
    public GameObject bzztPrefab;
    public List<Sprite> crackSprites;
    public SpriteRenderer domeRenderer;

    private bool isAnimationPlaying = false;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateDomeAppearance();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (!isAnimationPlaying)
        {
            PlayBzztAnimation();
        }
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Debug.Log("Core Destroyed! Game Over.");
        }
        UpdateHealthBars();
        UpdateDomeAppearance();
    }

    void PlayBzztAnimation()
    {
        isAnimationPlaying = true;
        GameObject bzztInstance = Instantiate(bzztPrefab, transform.position, Quaternion.identity);
        Destroy(bzztInstance, 0.4f);
        StartCoroutine(ResetAnimationFlag(0.4f));
    }

    IEnumerator ResetAnimationFlag(float delay)
    {
        yield return new WaitForSeconds(delay);
        isAnimationPlaying = false;
    }

    void UpdateHealthBars()
    {
        float coreHealthRatio = (float)currentHealth / maxHealth;
        coreHealthBar.fillAmount = coreHealthRatio;
    }

    void UpdateDomeAppearance()
    {
        int spriteIndex = (int)(((float)currentHealth / maxHealth) * (crackSprites.Count - 1));
        domeRenderer.sprite = crackSprites[spriteIndex];
    }
}
