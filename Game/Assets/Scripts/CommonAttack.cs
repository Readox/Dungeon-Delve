using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CommonAttack : MonoBehaviour
{

    public GameObject gameManager;



    public float CalculateDamage(float weaponDamage) // Eventually this will take weapons and such as args (maybe)
    {
        float damage = 10 + weaponDamage; // Base Weapon Damage = 10
        if (GetRandFloat(0,100) > GetCritChance()) // No Critical Hit
        {
            damage = (10 + weaponDamage) * (1 + GetStrength()/100);
        }
        else // Critical Hit
        {
            damage = (10 + weaponDamage) * (1 + (GetStrength()/100)) * (1 + (GetCritDamage()/100));
        }

        if (GetCritChance() > 100)
        {
            damage *= 1 + ((GetCritChance()-100)/100);
        }
        Debug.Log("Final Damage: " + damage);
        return damage;
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

        Debug.Log("Ferocity: " + fero + "\nProcs: " + procs);
        return procs;
    }

    public float GetStrength()
    {
        return gameManager.GetComponent<PlayerStats>().Strength;
    }

    public float GetCritChance()
    {
        return gameManager.GetComponent<PlayerStats>().CritChance;
    }

    public float GetCritDamage()
    {
        return gameManager.GetComponent<PlayerStats>().CritDamage;
    }

    public float GetFerocity()
    {
        return gameManager.GetComponent<PlayerStats>().Ferocity;
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
        gameManager = GameObject.FindWithTag("GameController");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
