using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class CommonAttack : MonoBehaviour
{

    public GameObject gameManager;
    private PlayerStats playerStats_script;
    public GameObject damageIndicator; // damageIndicator prefab
    public GameObject ferocityAnimation;

    public float CalculateDamage(float weaponDamage, Transform targetPos) // Takes in Transform for damage indicator location
    {
        float damage = 10 + weaponDamage; // Base Weapon Damage = 10
        bool criticalHit = false;
        if (GetRandFloat(0,100) > GetCritChance()) // No Critical Hit
        {
            damage = (10 + weaponDamage) * (1 + GetStrength()/100);
        }
        else // Critical Hit
        {
            damage = (10 + weaponDamage) * (1 + (GetStrength()/100)) * (1 + (GetCritDamage()/100));
            criticalHit = true;
        }

        if (GetCritChance() > 100)
        {
            damage *= 1 + ((GetCritChance()-100)/100);
        }
        //Debug.Log("Final Damage: " + damage);

        float finalDamage =float.Parse(damage.ToString("N0")); // Truncates Decimals

        GameObject di = Instantiate(damageIndicator, targetPos.position, Quaternion.identity);
        ConfigureDamageIndicator(di, targetPos, finalDamage, criticalHit);

        return finalDamage;
    }


    private void ConfigureDamageIndicator(GameObject di, Transform newParent, float damage, bool criticalHit) // The new parent is the attacked entity
    {
        di.transform.SetParent(newParent);
        di.transform.position = newParent.position; // this might be redundant
        di.GetComponent<TextMeshPro>().text = damage.ToString(); 
        if (criticalHit)
        {
            //di.GetComponent<TextMeshPro>().color = 000000;
        }
    }

    public void SpawnFerocityAnimation(Transform newParent)
    {
        GameObject fero = Instantiate(ferocityAnimation, newParent.position, Quaternion.identity);
        fero.transform.SetParent(newParent);
        fero.transform.position = newParent.position; // this might be redundant
    }



    public int GetFerocityProcs()
    {
        float fero = GetFerocity();
        int procs = (int)((fero - (fero % 100))/100);
        if (GetRandFloat(0,100) < (fero % 100))
        {
            procs += 1;
        }
        /*
        if ((fero / 100) < 1)
        if (GetRandFloat(0,100) > (fero % 100))
        {
            procs += 1;
        }
        */

        //Debug.Log("Ferocity: " + fero + "\nProcs: " + procs);
        return procs;
    }

    public float GetStrength()
    {
        return playerStats_script.Strength;
    }

    public float GetCritChance()
    {
        return playerStats_script.CritChance;
    }

    public float GetCritDamage()
    {
        return playerStats_script.CritDamage;
    }

    public float GetFerocity()
    {
        return playerStats_script.Ferocity;
    }

    public float GetRandFloat(float min, float max)
    {
        System.Random random = new System.Random();
        double val = (random.NextDouble() * (max - min) + min);
        return (float) val;
    }



    // Start is called before the first frame update
    void Start()
    {
        //gameManager = GameObject.FindWithTag("GameController");
        //playerStats_script = gameManager.GetComponent<PlayerStats>();
    }

    void Awake()
    {
        gameManager = GameObject.FindWithTag("GameController");
        playerStats_script = gameManager.GetComponent<PlayerStats>();
        //playerStats_script = gameManager.GetComponent<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Older version of above
    public float CalculateDamage(float weaponDamage) // Eventually this will take weapons and such as args (maybe)
    {
        float damage = 10 + weaponDamage; // Base Weapon Damage = 10
        //bool criticalHit = false;
        if (GetRandFloat(0,100) > GetCritChance()) // No Critical Hit
        {
            damage = (10 + weaponDamage) * (1 + GetStrength()/100);
        }
        else // Critical Hit
        {
            damage = (10 + weaponDamage) * (1 + (GetStrength()/100)) * (1 + (GetCritDamage()/100));
            //criticalHit = true;
        }

        if (GetCritChance() > 100)
        {
            damage *= 1 + ((GetCritChance()-100)/100);
        }
        //Debug.Log("Final Damage: " + damage);
        return damage;
    }
}
