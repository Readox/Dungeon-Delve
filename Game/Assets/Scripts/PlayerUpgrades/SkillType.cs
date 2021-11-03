using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillType
{

    public string skillType;
    public int skillLevel;
    public int skillTreeTier;
    public string skillID;

    private int currencyCostIncrease = 10;

    private GameObject gameManager;

    public SkillType(string skillType, int skillLevel, int skillTreeTier, string skillID)
    {
        this.skillType = skillType;
        this.skillLevel = skillLevel;
        this.skillTreeTier = skillTreeTier;
        this.skillID = skillID;
        gameManager = GameObject.FindWithTag("GameController");
    }

    public SkillType()
    {

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

    // Also returns the cost in currency
    public int AddSkillLevel(int x)
    {
        this.skillLevel += x;
        return GetCurrencyCost();
    }

    public int GetCurrencyCost()
    {
        int currencyCost = GetSkillLevel() * currencyCostIncrease;
        return currencyCost;
    }

    
    public float GetModifyValue()
    {
        float finalVal = 0f;
        if (skillType.Equals("Health"))
        {
            finalVal += skillLevel * 10;
        }
        else if (skillType.Equals("Defense"))
        {
            finalVal += skillLevel;
        }
        return finalVal;
    }






}
