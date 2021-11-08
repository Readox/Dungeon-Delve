using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillType
{
    public GameObject dropdown;
    public string skillType;
    public int skillLevel;
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
        this.skillType = newText;
        skillAmountIncreased = 0; // Makes the amount skill has been increased by 0 because the dropdown has switched to a new type
        return newText;
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


    
    public float GetModifyValue()
    {
        float finalVal = 0f;
        if (skillType.Equals("Health"))
        {
            finalVal += 10;
        }
        else if (skillType.Equals("Defense"))
        {
            finalVal += 1;
        }
        // add to the amount skill has been increased by
        skillAmountIncreased += finalVal;

        Debug.Log(finalVal + " Final Val");
        Debug.Log(skillAmountIncreased + " Skill Amount INcreased");
        return finalVal;
    }






}
