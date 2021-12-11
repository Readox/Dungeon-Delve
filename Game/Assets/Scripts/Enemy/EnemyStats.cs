using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStats : MonoBehaviour
{

    public float maxHealth;
    public float currentHealth;

    public float baseDamage;

    public GameObject healthBar;
    public Slider healthBarSlider;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void DealDamage(float damage)
    {
        healthBar.SetActive(true);
        currentHealth -= damage;
        CheckDeath();
        healthBarSlider.value = CalculateHealthPercentage();    
    }

    public void HealCharacter(float heal)
    {
        currentHealth += heal;
        CheckHealthMax();
        healthBarSlider.value = CalculateHealthPercentage();
    }

    private float CalculateHealthPercentage()
    {
        return (currentHealth/maxHealth);
    }

    public void CheckHealthMax()
    {
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    public float GetDamage()
    {
        return baseDamage;
    }

    public void CheckDeath()
    {
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }


}