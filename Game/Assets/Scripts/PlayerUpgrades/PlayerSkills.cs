using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkills : MonoBehaviour
{

    //private List<SkillType> unlockedSkillList;
    public static List<SkillType> unlockedSkillLevels = new List<SkillType>();
    public int playerSkillTokens;
    public int playerSkillCurrency;

    public GameObject tier1Dropdown;

    public void UpdateValues()
    {
        GameObject gameManager = GameObject.FindWithTag("GameController");
        string skillType = "";

        foreach (SkillType currentClass in unlockedSkillLevels)
        {
            skillType = currentClass.GetSkillType();
            if (skillType.Equals("Health"))
            {
                skillType = "maxHealth";
            }
            float modifyBy = currentClass.GetModifyValue();
            //float playerStatValue = gameManager.GetComponent<PlayerStats>().FindFromClassType(skillType);
            gameManager.GetComponent<PlayerStats>().SetStat(ref skillType, modifyBy);
        }
        Debug.Log("Player Stat Values Updated");
    }

    private void PopulateList()
    {
        Debug.Log("Dropdown value: " + tier1Dropdown.GetComponent<Dropdown>().captionText.text);

        SkillType tier1HealthDef = new SkillType(tier1Dropdown.GetComponent<Dropdown>().captionText.text, 1, 1, "tier1HealthDef");
        unlockedSkillLevels.Add(tier1HealthDef);       // Tier 1 (HP/D)
        /*
        unlockedSkillLevels.Add(new SkillType(tier1Dropdown.GetComponent<Dropdown>().value));       // Tier 1 (HP/D)
        unlockedSkillLevels.Add(new SkillType());
        unlockedSkillLevels.Add(new SkillType());
        unlockedSkillLevels.Add(new SkillType());
        */
    }

    // get parent of gameobject, then find the name of the dropdown, then find the tier of the dropdown, then go into List and change class instance
    public void AddPoints(GameObject childButton)
    {
        string nameOfSkill = childButton.transform.parent.name;
        SkillType result = unlockedSkillLevels.Find(x => x.GetSkillID().Equals(nameOfSkill)); // Finds the SkillType class in the List through Lambdas.       Link: https://stackoverflow.com/questions/9854917/how-can-i-find-a-specific-element-in-a-listt/9854944
        Debug.Log(result.GetSkillID());
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
        if (result.GetSkillLevel() < 2)
        {
            Debug.Log("Skill at Lowest Level");
        }
        else
        {
            int currencyCost = result.AddSkillLevel(-1); // Have to get the currency cost of the operation
            playerSkillCurrency += currencyCost;
        }

        UpdateValues();
    }

    public void Awake()
    {
        PopulateList();
        UpdateValues();
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
