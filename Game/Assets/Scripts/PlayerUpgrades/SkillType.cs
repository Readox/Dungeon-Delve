using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[Serializable]
public class SkillType
{
    public GameObject assoc;
    public string skillType;
    public int skillLevel; // Skills will have base level of zero
    public string skillID;

    public float skillAmountIncreased;
    private int currencyCostIncrease;

    private GameObject gameManager;

    public SkillType(GameObject assoc, int skillLevel, string skillID)
    {
        this.assoc = assoc;
        this.skillType = GetFormattedSkillType(assoc); // Need this function because some of the upgrade types have spaces in them
        this.skillLevel = skillLevel;
        this.skillID = skillID;
        this.skillAmountIncreased = 0; // Starts at 0
        this.currencyCostIncrease = 10;
        //gameManager = GameObject.FindWithTag("GameController");
    }

    
    public string GetFormattedSkillType(GameObject assoc)
    {
        string finalText = assoc.name; // assoc.transform.GetChild(0).GetComponent<Text>().text;
        if (finalText.Equals("Health"))
        {
            finalText = "maxHealth";
        }
        else if (finalText.Equals("Critical Chance"))
        {
            finalText = "CritChance";
        }
        else if (finalText.Equals("Critical Damage"))
        {
            finalText = "CritDamage";
        }
        else if (finalText.Equals("Endurance Max"))
        {
            finalText = "endurancePoolMax";
        }
        else if (finalText.Equals("Endurance Regen"))
        {
            finalText = "EnduranceRegen";
        }
        else if (finalText.Equals("Health Regen"))
        {
            finalText = "HealthRegen";
        }
        else if (finalText.Equals("Magic Find"))
        {
            finalText = "MagicFind";
        }

        return finalText;
    }

    /*
    // Changes the skillType based on the gameObject text
    public string UpdateDropdownText()
    {
        string newText = GetFormattedSkillType(assoc);
        // Make if statement here to check whether gameObject has changed
        if (!this.skillType.Equals(newText))
        {
            this.skillType = newText;
            this.skillAmountIncreased = 0;
            this.skillLevel = 0; // Resets the level of the skill, which might be important if I change costs based on skill type
        }

        return this.skillType;
    }
    */

    public bool IsMaxLevel()
    {
        if (skillType.Equals("HealthRegen") && skillLevel == 25) 
        {
            return true;
        }
        else if (skillType.Equals("EnduranceRegen") && skillLevel == 25) 
        {
            return true;
        }
        /*
        else if (skillType.Equals("CritChance") && skillLevel == 20) 
        {
            return true;
        }
        */

        return false;
    }

    public float GetSkillAmountIncreased()
    {
        return skillAmountIncreased; // Gets the amount that the skill has been increased by
    }

    public string GetSkillID()
    {
        return skillID;
    }

    public string GetSkillType()
    {
        return skillType;
    }
    public int GetSkillLevel()
    {
        //Debug.Log("Skill Level: " + skillLevel);
        return skillLevel;
    }


    public int GetTotalCurrencyCost()
    {
        int finalVal = 0;
        for (int i = 0; i < skillLevel + 1; i++)
        {
            finalVal += (i) * currencyCostIncrease;
        }
        return finalVal;
    }

    // Only use this method for finding the currency cost for upgrading (eg, AddPoints())
    public int GetCurrencyCost()
    {
        currencyCostIncrease = 10;
        int currencyCost = (GetSkillLevel() + 1) * currencyCostIncrease;
        //Debug.Log("Currency Cost: " + currencyCost + "    Skill Level: " + GetSkillLevel() + "    Currency Cost Increase: " + currencyCostIncrease);
        return currencyCost;
    }

    // Also returns the cost in currency
    public int AddSkillLevel(int x)
    {
        currencyCostIncrease = 10;
        this.skillLevel += x;
        return GetSkillLevel() * currencyCostIncrease;
    }
    public int SubtractSkillLevel(int x)
    {
        currencyCostIncrease = 10;
        this.skillLevel -= x;
        return (GetSkillLevel() + 1) * currencyCostIncrease; // needs to be +1 account for subtraction (currency cost only for that operation)
    }


    // Just a note, but the += used below increases the amount by that the first time, and then by that + that next time (pretty sure its exponential, but I'm tired)
    public float GetModifyValue(bool subtract)
    {
        float finalVal = 0f;
        if (skillType.Equals("maxHealth")) // Health modifier = 10
        {
            finalVal = 10 * skillLevel;
        }
        else if (skillType.Equals("EnduranceRegen") || skillType.Equals("HealthRegen"))
        {
            finalVal = 1; // No multiplacation by skill Level, only increases by set amount every level
        }
        else // Default is 1 (Defaults: Defense, Strength, Critical Chance, Critical Damage, Ferocity, Magic Find)
        {
            finalVal = 1 * skillLevel;
        }


        // add/subtract to the amount skill has been increased by (depends on subtract field)
        if (subtract)
        {
            skillAmountIncreased -= finalVal;
        }
        else
        {
            skillAmountIncreased += finalVal;
        }
        
        return finalVal;
    }






}
