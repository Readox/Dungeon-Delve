using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkills : MonoBehaviour
{

    //private List<SkillType> unlockedSkillList;
    public static List<SkillType> unlockedSkillLevels = new List<SkillType>();
    public static int PlayerSkillTokens;
    public static int PlayerSkillCurrency; 
     

    public GameObject tier1Dropdown;

    public void UpdateValues()
    {

        Debug.Log("Player Stat Values Updated");
    }

    private void PopulateList()
    {
        Debug.Log("Dropdown value: " + tier1Dropdown.GetComponent<Dropdown>().value);
        /*
        unlockedSkillLevels.Add(new SkillType(tier1Dropdown.GetComponent<Dropdown>().value));       // Tier 1 (HP/D)
        unlockedSkillLevels.Add(new SkillType());
        unlockedSkillLevels.Add(new SkillType());
        unlockedSkillLevels.Add(new SkillType());
        */
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
