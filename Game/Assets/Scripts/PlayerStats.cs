using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{

    public static PlayerStats playerStats;

    public GameObject player;

    // Current Health that the player has
    public float currentHealth;
    // Maximum health that the player will have
    // Base Value: 10
    public float maxHealth;
    // Amount that damage is reduced by (eg. player has 2 def, takes a 3 damage hit, the damage is reduced by the defense so the player only takes 1 damage)
    // Base Value: 0
    public float defense;
    // Amount that base damage is multiplied by (regardless of critical hit)
    // Base Value: 1
    public float strength;
    // Percent Chance to critically hit
    // Base Value: 25%
    public float critChance;
    // Damage multiplier (percentage) on critical hits
    // Base Value: 50%
    public float critDamage;
    // Chance for double hits
    // Percentages above 100% (and there will be) will overflow into triple hits, then quadruple hits, and oh boy this is going to be hard to balance
    // Base Value: 0%
    public float fero;
    // Percentage of MaxHealth regenerated every second
    // Base Value: 1%
    public float regenerationRate;
    // Chance for rare drops from boss chests and monsters
    // Base Value: 0%
    public float magicFind;

    // Base Damage is given by the weapon being used, and is what is being multiplied by strength and crit damage, then increased by other percentages and fero



    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    void Awake()
    {
        /*
        if (playerStats != null)
        {
            Destroy(playerStats);
        }
        else
        {
            playerStats = this;
        }
        */
        playerStats = this;
        // For safeties:
        DontDestroyOnLoad(this);
    }

    public void DealDamage(float damage)
    {
        currentHealth -= damage;
        CheckDeath();
    }

    public void HealCharacter(float healAmount)
    {
        currentHealth += healAmount;
        CheckHealthMax();
    }

    public void CheckHealthMax()
    {
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }


    public void CheckDeath()
    {
        if (currentHealth <= 0)
        {
            Destroy(player);
        }
    }


    // Update is called once per frame
    void Update()
    {

    }


}
