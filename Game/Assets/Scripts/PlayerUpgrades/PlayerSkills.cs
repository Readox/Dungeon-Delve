using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class PlayerSkills : MonoBehaviour
{
    private string filePath1; // = Application.persistentDataPath + "/playerUpgradeInfo1.json"; // Build 1
    private string filePath2; // = Application.persistentDataPath + "/playerUpgradeInfo2.json"; // Build 2
    private string filePath3; // = Application.persistentDataPath + "/playerUpgradeInfo3.json"; // Build 3
    private string currentBuildDropdownPath;

    //private List<SkillType> unlockedSkillList;
    public List<SkillType> unlockedSkillLevels = new List<SkillType>();

    public int playerUpgradeCurrency;
    public int playerUpgradeTokens;

    public GameObject gameManager;
    public GameObject upgradeList;
    public List<GameObject> playerUpgrades = new List<GameObject>();
    
    private PlayerStats playerStats_script;

    public void OnChangeBuildDropdown(GameObject dropdown)
    {
        Save(currentBuildDropdownPath);

        ResetAll();

        string temp = dropdown.GetComponent<Dropdown>().captionText.text;
        Debug.Log("Dropdown Caption Text: " + temp);
        if (temp.Equals("Build 1"))
        {
            currentBuildDropdownPath = filePath1;
        }
        else if (temp.Equals("Build 2"))
        {
            currentBuildDropdownPath = filePath2;
        }
        else if (temp.Equals("Build 3"))
        {
            currentBuildDropdownPath = filePath3;
        }
        else
        {
            currentBuildDropdownPath = null;
            Debug.Log("Bad String Value");
        }

        Load(currentBuildDropdownPath);
    }


    public void Save(string filePath) // Dont use Binary Formatter apparently
    {
        Save data = new Save();
        data.name = "SavedUpgrades"; // Save name here
        data.playerUpgradeCurrency = this.playerUpgradeCurrency;
        data.playerUpgradeTokens = this.playerUpgradeTokens;
        // Save all data here
        foreach (SkillType st in unlockedSkillLevels)
        {
            data.upgrades.Add(st);
            data.totalCurrencyCost += st.GetTotalCurrencyCost();
        }

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(filePath, json);

        Debug.Log("Data Saved into JSON: " + json);
    }

    public void Load(string filePath)
    {
        if (File.Exists(filePath))
        {
            Save data = JsonUtility.FromJson<Save>(File.ReadAllText(filePath));
            // Load info here
            Debug.Log("Save Name: " + data.name);
            Debug.Log("Total Currency Cost: " + data.totalCurrencyCost);


            foreach (GameObject gameObj in playerUpgrades)
            {
                bool inList = false;
                foreach (SkillType st in data.upgrades)
                {
                    if (gameObj.name == st.skillID)
                    {
                        inList = true;
                        break;
                    }
                }
                if (inList)
                {
                    gameObj.GetComponent<LinkedGameObject>().linkedGameObject.SetActive(false);
                    gameObj.SetActive(true);
                }
                else
                {
                    gameObj.GetComponent<LinkedGameObject>().linkedGameObject.SetActive(true);
                    gameObj.SetActive(false);
                }
            }

            /*
            for (int i = 0; i < playerUpgrades.Count; i++) 
            {
                bool inList = false;
                for (int j = 0; j < data.upgrades.Count; j ++)
                {
                    if (playerUpgrades[i].name == data.upgrades[j].skillID)
                    {
                        inList = true;
                        break;
                    }
                }
                if (inList)
                {
                    playerUpgrades[i].GetComponent<LinkedGameObject>().linkedGameObject.SetActive(false);
                    playerUpgrades[i].SetActive(true);
                }
                else
                {
                    playerUpgrades[i].GetComponent<LinkedGameObject>().linkedGameObject.SetActive(true);
                    playerUpgrades[i].SetActive(false);
                }
                
            }
            */
            
            /*
            foreach (SkillType st in data.upgrades)
            {
                Debug.Log("Skill Name: " + st.skillID + "\nSkill Type: " + st.skillType + "\nSkill Level: " + st.skillLevel);
            }
            */
            // Put data back into game here
            playerUpgradeCurrency = data.playerUpgradeCurrency;
            playerUpgradeTokens = data.playerUpgradeTokens;

            unlockedSkillLevels.Clear();
            foreach (SkillType st in data.upgrades)
            {
                unlockedSkillLevels.Add(st);
            }
            PutInValues();

            UpdateUIElements();
        }
        else
        {
            Debug.Log("File does not exist");
        }
        
        
    }

    public void ResetAll()
    {
        foreach (SkillType st in unlockedSkillLevels)
        {
            playerUpgradeCurrency += st.GetTotalCurrencyCost();
            playerUpgradeTokens += 1;

            string skillType = st.GetSkillType();
            float modifyBy = st.GetSkillAmountIncreased() * -1;
            playerStats_script.SetStat(ref skillType, modifyBy);
            st.dropdown.gameObject.transform.parent.gameObject.GetComponent<LinkedGameObject>().linkedGameObject.SetActive(true);
            st.dropdown.gameObject.transform.parent.gameObject.SetActive(false);
        }
        unlockedSkillLevels.Clear();
    }

    public void PutInValues()
    {
        foreach (SkillType st in unlockedSkillLevels)
        {
            string skillType = st.GetSkillType();
            float modifyBy = st.GetSkillAmountIncreased();
            playerStats_script.SetStat(ref skillType, modifyBy);
        }
    }

    // Transitioning this so that it only occurs on + and - and switched dropdown, but will still exist for when Awake() happens
    public void UpdateValues()
    {
        string skillType = "";

        foreach (SkillType currentClass in unlockedSkillLevels)
        {
            skillType = currentClass.UpdateDropdownText();


            float modifyBy = currentClass.GetModifyValue(false);

            //Debug.Log(modifyBy);

            //float playerStatValue = gameManager.GetComponent<PlayerStats>().FindFromClassType(skillType);
            playerStats_script.SetStat(ref skillType, modifyBy);
        }

        playerStats_script.UpdateHealthEnduranceBars(); // Update the health and ability bars now before the player exits the menu so that they are ready
    }


    // Use substring to get name of GameObject that we are adding with name format: "Unlock Tier2Combat" or "Unlock Tier3Special" so substring(7)
    // Try to make it so that I use code to find the dropdown that I want instead of linking in editor later? Maybe not though, because using Find() is slow af

    // Unfortunately, Unity doesnt seem to accept assignment of functions that have two arguments, so I'm going to have to hack a little (maybe I can call two functions from one button)
    // Format for adding new skill types is: (Dropdown GameObject, skill level, Name of GameObject in editor)
    public void UnlockSkill(GameObject parentButton)
    {
        if (playerUpgradeTokens > 0)
        {
            playerUpgradeTokens -= 1; // Subtract from skill tokens
            GameObject dropdown = parentButton.transform.GetChild(0).gameObject; // Get the dropdown to use for assignment for the skillType being added

            dropdown.GetComponent<Dropdown>().captionText.text = dropdown.GetComponent<Dropdown>().options[0].text; // I need this here, because often the default value isn't what it should be, so this changes it so that it will be the first option in the dropdown

            SkillType newSkill = new SkillType(dropdown, 0, dropdown.name); // It does not matter whether the name is this or that, it is not used, more for reference in code
            unlockedSkillLevels.Add(newSkill);
            parentButton.SetActive(true);

            UpdateUIElements(dropdown, newSkill);
        }
        else
        {
            //Debug.Log("Not enough tokens!");
        }
        
        // UpdateUIElements() is not kept here
    }
    // Extension of UnlockSkill() bc/ I needed two functions to accomplish one thing
    public void SetInactiveUnlockButton(GameObject caller)
    {
        caller.SetActive(false);
    }
    public void SetInactiveButtonComponent(GameObject caller) // Unused, and doesn't accomplish what I want
    {
        caller.GetComponent<Button>().interactable = false;
    }

    // I need this to find out what the previous skill was, and then subtract that bonus from the PlayerStats
    // A notable problem is that when adding new Upgrades, they default to having the incorrect Dropdown name, causing them to become HealthDef dropdowns
    public void SwitchedDropdown(GameObject dropdown)
    {
        // Find the skill that the dropdown is associated with in unlockeSkillLevels
        SkillType currentClass = unlockedSkillLevels.Find(x => x.GetSkillID().Equals(dropdown.name));

        string skillType = currentClass.GetSkillType(); // Gets the skillType which has not been updated yet, but will be when UpdateValues() is called below

        float modifyBy = currentClass.GetSkillAmountIncreased() * -1;
        playerUpgradeCurrency += currentClass.GetTotalCurrencyCost(); // Gives back currency, because skill level is reset as well
        //Debug.Log(currentClass.GetTotalCurrencyCost());
        playerStats_script.SetStat(ref skillType, modifyBy);


        unlockedSkillLevels.Remove(currentClass);
        SkillType replacementSkill = new SkillType(dropdown, 0, dropdown.name);
        unlockedSkillLevels.Add(replacementSkill);

        UpdateUIElements(dropdown, replacementSkill);
    }
    


    // get parent of gameobject, then find the name of the dropdown, then find the tier of the dropdown, then go into List and change class instance: public void AddPoints(GameObject childbutton)
    public void AddPoints(GameObject childButton)
    {
        string nameOfSkill = childButton.transform.parent.name;

        
        SkillType currentClass = null;
        for (int i = 0; i < unlockedSkillLevels.Count; i++)
        {
            if (unlockedSkillLevels[i].GetSkillID().Equals(nameOfSkill))
            {
                currentClass = unlockedSkillLevels[i];
                break;
            }
        }

        // I am fairly certain that the next line is what is causing my problems
        //SkillType currentClass = unlockedSkillLevels.Find(x => x.GetSkillID().Equals(nameOfSkill)); // Finds the SkillType class in the List through Lambdas.       Link: https://stackoverflow.com/questions/9854917/how-can-i-find-a-specific-element-in-a-listt/9854944
        
        
        if (currentClass.GetCurrencyCost() > playerUpgradeCurrency || currentClass.IsMaxLevel() || currentClass == null)
        {
            Debug.Log("Not enough currency, stat already increased to max level, or the skill is null!");
        }
        else
        {
            int currencyCost = currentClass.AddSkillLevel(1); // Have to get the currency cost of the operation
            Debug.Log("Currency Cost: " + currencyCost);
            
            playerUpgradeCurrency -= currencyCost;

            string skillType = currentClass.UpdateDropdownText();
            
            float modifyBy = currentClass.GetModifyValue(false);
            playerStats_script.SetStat(ref skillType, modifyBy);
        }
        
        

        UpdateUIElements(childButton.transform.parent.gameObject, currentClass);
    }

    public void SubtractPoints(GameObject childButton)
    {
        string nameOfSkill = childButton.transform.parent.name;
        SkillType currentClass = unlockedSkillLevels.Find(x => x.GetSkillID() == nameOfSkill);
        
        if (currentClass.GetSkillLevel() < 1)
        {
            //Debug.Log("Skill at Lowest Level");
        }
        else
        {
            float modifyBy = currentClass.GetModifyValue(true) * -1; // Have to do this here, before I subtract the skill level below
            int currencyCost = currentClass.SubtractSkillLevel(1); // function returns the currency cost of the operation
            playerUpgradeCurrency += currencyCost;
            string skillType = currentClass.UpdateDropdownText();

            playerStats_script.SetStat(ref skillType, modifyBy);
        }

        UpdateUIElements(childButton.transform.parent.gameObject, currentClass);
    }

    public void Awake()
    {
        gameManager = GameObject.FindWithTag("GameController");
        ClearList();

        //filePath = Application.persistentDataPath + "/playerUpgradeInfo.json";
        //Save(currentBuildDropdownPath);

        // TODO: I need to add player upgrades in here that were saved

        playerStats_script = gameManager.GetComponent<PlayerStats>();

        UpdateValues();
        UpdateUIElements();
    }

    public void OpenUpgradesMenuStart()
    {
        UpdateUIElements();
    }

    private void PrintAllInList()
    {
        foreach(SkillType currentClass in unlockedSkillLevels)
        {
            Debug.Log(currentClass.GetSkillID() + "\n");
        }
    }


    public Text playerUpgradeCurrencyTokensText;

    public void UpdateUIElements(GameObject parent, SkillType currentClass) // This one is for the Add and Subtract Points
    {
        playerUpgradeCurrencyTokensText.text = "Upgrade Currency: " + playerUpgradeCurrency + "\nUpgrade Unlocks: " + playerUpgradeTokens;
        playerStats_script.UpdateHealthEnduranceBars();

        string colorVal = playerStats_script.GetColorForStat(currentClass.GetSkillType());
        parent.transform.GetChild(0).GetComponent<Text>().text = $"<color={colorVal}>+{currentClass.GetSkillAmountIncreased()} {parent.GetComponent<Dropdown>().captionText.text}</color>";
        //gameManager.GetComponent<PlayerStats>().SetUpgradeText(currentClass.GetSkillType(), parent.transform.GetChild(0).gameObject, currentClass.GetSkillAmountIncreased());
    }

    public void UpdateUIElements()
    {
        playerUpgradeCurrencyTokensText.text = "Upgrade Currency: " + playerUpgradeCurrency + "\nUpgrade Unlocks: " + playerUpgradeTokens;
        playerStats_script.UpdateHealthEnduranceBars();


    }


    private void ClearList()
    {
        unlockedSkillLevels.Clear();
    }





    // Start is called before the first frame update
    void Start()
    {
        filePath1 = Application.persistentDataPath + "/playerUpgradeInfo1.json"; // Build 1
        filePath2 = Application.persistentDataPath + "/playerUpgradeInfo2.json"; // Build 2
        filePath3 = Application.persistentDataPath + "/playerUpgradeInfo3.json"; // Build 3
        currentBuildDropdownPath = filePath1;

        // Initializes all the files
        Save(filePath1);
        Save(filePath2);
        Save(filePath3);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

[Serializable]
public class Save 
{
    // All of this from https://stackoverflow.com/questions/40078490/saving-loading-data-in-unity/40097623#40097623
    public List<int> ID = new List<int>();
    public List<int> Amounts = new List<int>();
    public string name;
    public int totalCurrencyCost;
    public List<SkillType> upgrades = new List<SkillType>();
    public int playerUpgradeCurrency;
    public int playerUpgradeTokens;
}


