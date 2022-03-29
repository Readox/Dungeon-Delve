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
    public GameObject deathAnimation;
    private GameObject homeSpawner;
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
            //StartCoroutine(InvulnerabilityFrameCoroutine(0.1f)); // Invul frames
        }
    }

    IEnumerator InvulnerabilityFrameCoroutine(float time)
    {
        invulnerable = true;
        yield return new WaitForSeconds(time);
        invulnerable = false;

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

                GameObject deathAnim = Instantiate(deathAnimation, transform.position, Quaternion.identity);
                deathAnim.gameObject.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
                
                // Destruction
                if (transform.parent != null) // Is there a parent?
                {
                    homeSpawner.GetComponent<SpawnPoint>().RemoveFromList(gameObject.transform.parent.gameObject);
                    Destroy(gameObject.transform.parent.gameObject);
                }
                else
                {
                    homeSpawner.GetComponent<SpawnPoint>().RemoveFromList(gameObject.transform.parent.gameObject);
                    Destroy(gameObject);
                }
                
            }
            
        }
    }

    public void SetHomeSpawner(GameObject s)
    {
        homeSpawner = s;
    }

    public float GetRandFloat(float min, float max)
    {
        System.Random random = new System.Random();
        double val = (random.NextDouble() * (max - min) + min);
        return (float) val;
    }


}
