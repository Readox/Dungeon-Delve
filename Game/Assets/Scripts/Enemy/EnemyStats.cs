using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStats : MonoBehaviour
{

    public float maxHealth;
    public float currentHealth;
    public bool invulnerable;

    public float baseDamage;

    public GameObject healthBar;
    public Slider healthBarSlider;

    public GameObject upgradeCurrencyDrop;
    public float upgradeCurrencyDropChance;

    float playerMagicFind;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void DealDamage(float damage)
    {
        if (!invulnerable)
        {
            healthBar.SetActive(true);
            currentHealth -= damage;
            CheckDeath();
            healthBarSlider.value = CalculateHealthPercentage();  
        }
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
            if (gameObject.name.Equals("Training Dummy"))
            {
                currentHealth = maxHealth;
            }
            else
            {
                playerMagicFind = GameObject.FindWithTag("GameController").GetComponent<PlayerStats>().MagicFind;
                

                if (upgradeCurrencyDropChance * (1 + (playerMagicFind / 100)) >= GetRandFloat(0,100))
                {
                    GameObject upgradeCurrency = Instantiate(upgradeCurrencyDrop, this.transform.position, Quaternion.identity);
                }
                
                // Destruction
                if (transform.parent != null) // Is there a parent?
                {
                    Destroy(gameObject.transform.parent.gameObject);
                }
                else
                {
                    Destroy(gameObject);
                }
                
            }
            
        }
    }

    public float GetRandFloat(float min, float max)
    {
        System.Random random = new System.Random();
        double val = (random.NextDouble() * (max - min) + min);
        return (float) val;
    }


}
