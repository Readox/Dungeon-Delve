using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillType
{
    public GameObject dropdown;
    public string skillType;
    public int skillLevel; // Skills will have base level of zero
    public int skillTreeTier;
    public string skillID;

    public float skillAmountIncreased;
    private int currencyCostIncrease = 10;

    private GameObject gameManager;

    public SkillType(GameObject dropdown, int skillLevel, int skillTreeTier, string skillID)
    {
        this.dropdown = dropdown;
        this.skillType = dropdown.GetComponent<Dropdown>().captionText.text;
        this.skillLevel = skillLevel;
        this.skillTreeTier = skillTreeTier;
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
    public int GetSkillTier()
    {
        return skillTreeTier;
    }


    public int GetCurrencyCost()
    {
        int currencyCost = GetSkillLevel() * currencyCostIncrease;
        return currencyCost;
    }

    // Also returns the cost in currency
    public int AddSkillLevel(int x)
    {
        this.skillLevel += x;
        return GetCurrencyCost();
    }
    public int SubtractSkillLevel(int x)
    {
        this.skillLevel -= x;
        return GetCurrencyCost();
    }


    // Just a note, but the += used below increases the amount by that the first time, and then by that + that next time (pretty sure its exponential, but I'm tired)
    public float GetModifyValue()
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
        // add to the amount skill has been increased by
        skillAmountIncreased += finalVal;

        Debug.Log(finalVal + " Final Val");
        Debug.Log(skillAmountIncreased + " Skill Amount Increased");
        return finalVal;
    }






}
