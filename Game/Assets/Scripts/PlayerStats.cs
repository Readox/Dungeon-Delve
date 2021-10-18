using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{

    public static PlayerStats playerStats;

    public GameObject player;

    public Slider healthBarSlider;
    public Text healthText;

    public Slider abilityBarSlider;
    public Text abilityText;

    public string playerClass;


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        currentAbilityPool = abilityPoolMax;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Awake()
    {
        playerStats = this;
        // For safeties:
        DontDestroyOnLoad(this);


        PlayerClassSetup();

        currentHealth = maxHealth;

        healthBarSlider.value = 1;
        healthText.text = Mathf.Ceil(currentHealth).ToString() + " / " + Mathf.Ceil(maxHealth).ToString();
    }

    public void DealDamage(float damage)
    {
        currentHealth -= damage;
        CheckDeath();
        SetHealthInfo();
    }

    public void HealCharacter(float healAmount)
    {
        currentHealth += healAmount;
        CheckHealthMax();
        SetHealthInfo();
    }

    public string GetClass()
    {
        return playerClass;
    }


    private float CalculateHealthPercentage()
    {
        return currentHealth / maxHealth;
    }

    private void SetHealthInfo()
    {
        healthBarSlider.value = CalculateHealthPercentage();
        healthText.text = Mathf.Ceil(currentHealth).ToString() + " / " + Mathf.Ceil(maxHealth).ToString();
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


    public void PlayerClassSetup()
    {
        if (playerClass.Equals("fighter"))
        {

        }
        else if (playerClass.Equals("mage"))
        {

        }
        else if (playerClass.Equals("archer"))
        {

        }
        else if (playerClass.Equals("tank"))
        {

        }
        else
        {
            Debug.Log("Select a class please!");
        }
    }






    // All this stuff is down here, because there is a lot

    public float abilityPoolMax;
    public float currentAbilityPool;


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

    // Base Damage is given by the weapon being used, and is what is being multiplied by strength and crit damage,
    //      then increased by other percentages and fero



}
