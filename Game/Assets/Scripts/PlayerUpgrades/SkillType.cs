using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillType
{
    public GameObject dropdown;
    public string skillType;
    public int skillLevel; // Skills will have base level of zero
    public string skillID;

    public float skillAmountIncreased;
    private int currencyCostIncrease = 10;

    private GameObject gameManager;

    public SkillType(GameObject dropdown, int skillLevel, string skillID)
    {
        this.dropdown = dropdown;
        this.skillType = dropdown.GetComponent<Dropdown>().captionText.text;
        this.skillLevel = skillLevel;
        this.skillID = skillID;
        this.skillAmountIncreased = 0; // Starts at 0
        //gameManager = GameObject.FindWithTag("GameController");
    }

    public SkillType()
    {

    }

    // Changes the skillType based on the dropdown text
    public string UpdateDropdownText()
    {
        string newText = dropdown.GetComponent<Dropdown>().captionText.text;
        // Make if statement here to check whether dropdown has changed
        if (!this.skillType.Equals(newText))
        {
            this.skillType = newText;
            this.skillAmountIncreased = 0;
            this.skillLevel = 0; // Resets the level of the skill, which might be important if I change costs based on skill type
        }

        return this.skillType;
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
        int currencyCost = (GetSkillLevel() + 1) * currencyCostIncrease;
        return currencyCost;
    }

    // Also returns the cost in currency
    public int AddSkillLevel(int x)
    {
        this.skillLevel += x;
        return GetSkillLevel() * currencyCostIncrease;
    }
    public int SubtractSkillLevel(int x)
    {
        this.skillLevel -= x;
        return (GetSkillLevel() + 1) * currencyCostIncrease; // needs to be +1 account for subtraction (currency cost only for that operation)
    }


    // Just a note, but the += used below increases the amount by that the first time, and then by that + that next time (pretty sure its exponential, but I'm tired)
    public float GetModifyValue(bool subtract)
    {
        float finalVal = 0f;
        if (skillType.Equals("Health")) // Health modifier = 10
        {
            finalVal = 10 * skillLevel;
        }
        else if (skillType.Equals("Defense")) // Defense modifier = 1
        {
            finalVal = 1 * skillLevel;
        }
        else // Default is 1
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
