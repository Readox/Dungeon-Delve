using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkills : MonoBehaviour
{

    //private List<SkillType> unlockedSkillList;
    public List<SkillType> unlockedSkillLevels = new List<SkillType>();
    public int playerSkillTokens;
    public int playerSkillCurrency;

    public GameObject tier1Dropdown;


    public GameObject gameManager;

    public void UpdateValues()
    {
        string skillType = "";

        foreach (SkillType currentClass in unlockedSkillLevels)
        {
            skillType = currentClass.UpdateDropdownText();

            if (skillType.Equals("Health"))
            {
                skillType = "maxHealth";
            }
            float modifyBy = currentClass.GetModifyValue();

            //Debug.Log(modifyBy);

            //float playerStatValue = gameManager.GetComponent<PlayerStats>().FindFromClassType(skillType);
            gameManager.GetComponent<PlayerStats>().SetStat(ref skillType, modifyBy);
        }

        gameManager.GetComponent<PlayerStats>().UpdateHealthAbilityBars(); // Update the health and ability bars now before the player exits the menu so that they are ready
    }


    // Use substring to get name of GameObject that we are adding with name format: "Unlock Tier2Combat" or "Unlock Tier3Special" so substring(7)
    // Try to make it so that I use code to find the dropdown that I want instead of linking in editor later? Maybe not though, because using Find() is slow af

    // Unfortunately, Unity doesnt seem to accept assignment of functions that have two arguments, so I'm going to have to hack a little (maybe I can call two functions from one button)
    public void UnlockSkill(GameObject parentButton)
    {
        if (playerSkillTokens > 0)
        {
            playerSkillTokens -= 1; // Subtract from skill tokens
            GameObject dropdown = parentButton.transform.GetChild(0).gameObject; // Get the dropdown to use for assignment for the skillType being added
            string skillID = dropdown.name; // Get skill ID from above dropdown, which is the name of the GameObject
            SkillType newSkill = new SkillType(dropdown, 0, 1, skillID); 
            unlockedSkillLevels.Add(newSkill);
            parentButton.SetActive(true);
        }
        else
        {
            Debug.Log("Not enough tokens!");
        }
        
    }
    // Extension of UnlockSkill() bc/ I needed two functions to accomplish one thing
    public void SetInactiveUnlockButton(GameObject caller)
    {
        caller.SetActive(false);
    }


    private void PopulateList()
    {
        // Format for adding new skill types is: (Dropdown GameObject, skill level, skill tree tier, Name of GameObject in editor)
        
        SkillType Tier1HealthDef = new SkillType(tier1Dropdown, 0, 1, "Tier1HealthDef"); // It does not matter whether the name is this or that, it is not used, more for reference in code
        unlockedSkillLevels.Add(Tier1HealthDef);       // Tier 1 (HP/D)
        




    }

    // I need this to find out what the previous skill was, and then subtract that bonus from the PlayerStats
    public void SwitchedDropdown(GameObject dropdown)
    {
        // Find the skill that the dropdown is associated with in unlockeSkillLevels
        SkillType result = unlockedSkillLevels.Find(x => x.GetSkillID().Equals(dropdown.name));

        string skillType = result.GetSkillType(); // Gets the skillType which has not been updated yet, but will be when UpdateValues() is called below

        if (skillType.Equals("Health"))
        {
            skillType = "maxHealth";
        }

        float modifyBy = result.GetSkillAmountIncreased() * -1;
        playerSkillCurrency += result.GetCurrencyCost(); // Gives back currency, because skill level is reset as well
        gameManager.GetComponent<PlayerStats>().SetStat(ref skillType, modifyBy);

        UpdateValues();

    }
       


    // get parent of gameobject, then find the name of the dropdown, then find the tier of the dropdown, then go into List and change class instance
    public void AddPoints(GameObject childButton)
    {
        string nameOfSkill = childButton.transform.parent.name;
        SkillType result = unlockedSkillLevels.Find(x => x.GetSkillID().Equals(nameOfSkill)); // Finds the SkillType class in the List through Lambdas.       Link: https://stackoverflow.com/questions/9854917/how-can-i-find-a-specific-element-in-a-listt/9854944
        if (result.GetCurrencyCost() > playerSkillCurrency)
        {
            Debug.Log("Not enough currency!");
        }
        else
        {
            int currencyCost = result.AddSkillLevel(1); // Have to get the currency cost of the operation
            playerSkillCurrency -= currencyCost;
        }
        UpdateValues();
    }

    public void SubtractPoints(GameObject childButton)
    {
        string nameOfSkill = childButton.transform.parent.name;
        SkillType result = unlockedSkillLevels.Find(x => x.GetSkillID() == nameOfSkill);
        if (result.GetSkillLevel() < 1)
        {
            Debug.Log("Skill at Lowest Level");
        }
        else
        {
            int currencyCost = result.SubtractSkillLevel(1); // function returns the currency cost of the operation
            playerSkillCurrency += currencyCost;
        }

        UpdateValues();
    }

    public void Awake()
    {
        gameManager = GameObject.FindWithTag("GameController");
        // I think I have to clear the list of everything when the game restarts, because it is saving values
        ClearList();

        PopulateList();
        UpdateValues();
    }

    private void ClearList()
    {
        unlockedSkillLevels.Clear();
    }







    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
