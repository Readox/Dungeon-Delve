using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class PlayerSkills : MonoBehaviour
{
    // Not needed? //private string globalDataValuesPath; // = Application.persistentDataPath + "/globalDataValues.json"; // stores important values like player currency
    private string filePath1; // = Application.persistentDataPath + "/playerUpgradeInfo1.json"; // Build 1
    private string filePath2; // = Application.persistentDataPath + "/playerUpgradeInfo2.json"; // Build 2
    private string filePath3; // = Application.persistentDataPath + "/playerUpgradeInfo3.json"; // Build 3
    private string gameStatePath; // = Application.persistentDataPath + "/gameStatePath.json"; // Stores game state info 
    public string currentBuildDropdownPath;

    //private List<SkillType> unlockedSkillList;
    public List<SkillType> unlockedSkillLevels = new List<SkillType>();

    public int playerUpgradeCurrency; // The current amount of player upgrade currency

    public GameObject gameManager;
    public GameObject upgradeList;
    public List<GameObject> playerUpgrades = new List<GameObject>();
    public GameObject buildDropdown;
    
    private PlayerStats playerStats_script;

    public void OnChangeBuildDropdown(GameObject dropdown) // When build selection dropdown is changed, do this
    {
        Save(currentBuildDropdownPath);

        ResetAll();

        string temp = dropdown.GetComponent<Dropdown>().captionText.text;
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

    public void ResetBuild()
    {
        ResetAll();
        Save(currentBuildDropdownPath);
    }

    public void SaveGameState()
    {
        GlobalDataSave data = new GlobalDataSave();

        if (File.Exists(currentBuildDropdownPath))
        {
            Save saveData = JsonUtility.FromJson<Save>(File.ReadAllText(currentBuildDropdownPath));
            data.totalPlayerUpgradeCurrency = /*saveData.totalCurrencyCost + */playerUpgradeCurrency;
        }
        else
        {
            Debug.Log("The build file does not exist.");
        }

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(gameStatePath, json);

        UpdateAllUIElements();
        //Debug.Log("Data Saved into JSON: " + json);
    }

    public void LoadGameState()
    {
        if (File.Exists(gameStatePath))
        {
            GlobalDataSave data = JsonUtility.FromJson<GlobalDataSave>(File.ReadAllText(gameStatePath));

            playerUpgradeCurrency -= data.totalPlayerUpgradeCurrency;

            UpdateAllUIElements();
        }
        else
        {
            Debug.Log("File does not exist");
        }
    }

    public void Save(string filePath) // Saves what is currently in the menu to a file
    {
        Save data = new Save();
        data.name = filePath;
        foreach (SkillType st in unlockedSkillLevels)
        {
            data.upgrades.Add(st);
            data.totalCurrencyCost += st.GetTotalCurrencyCost();
        }

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(filePath, json);

        UpdateAllUIElements();
        //Debug.Log("Data Saved into JSON: " + json);
    }

    public void Save() // Saves current build path
    {
        Save data = new Save();
        data.name = currentBuildDropdownPath;
        foreach (SkillType st in unlockedSkillLevels)
        {
            data.upgrades.Add(st);
            data.totalCurrencyCost += st.GetTotalCurrencyCost();
        }

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(currentBuildDropdownPath, json);

        UpdateAllUIElements();
        //Debug.Log("Data Saved into JSON: " + json);
    }

    public void InitializeFile(string filePath) // Only call this upon game start
    {
        Save data = new Save();
        data.name = currentBuildDropdownPath;
        foreach (SkillType st in unlockedSkillLevels)
        {
            data.upgrades.Add(st);
            data.totalCurrencyCost += st.GetTotalCurrencyCost();
        }

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(filePath, json);

        UpdateAllUIElements();
        //Debug.Log("Data Saved into JSON: " + json);
    }

    public void Load(string filePath) // Loads in a save file
    {
        if (File.Exists(filePath))
        {
            Save data = JsonUtility.FromJson<Save>(File.ReadAllText(filePath));

            playerUpgradeCurrency -= data.totalCurrencyCost; // Sets currency to proper amount
            if (playerUpgradeCurrency < 0)
            {
                playerUpgradeCurrency = 0;
            }

            for (int x = 0; x < unlockedSkillLevels.Count; x++)
            {
                
                SkillType st = unlockedSkillLevels[x];
                for (int i = 0; i < data.upgrades.Count; i++)
                {
                    if (st.skillType.Equals(data.upgrades[i].skillType))
                    {
                        st = data.upgrades[i];
                        break;
                    }
                }
                for (int j = 0; j < playerUpgrades.Count; j++)
                {
                    if (st.skillID.Equals(playerUpgrades[j].name))
                    {
                        st.assoc = playerUpgrades[j];
                        break;
                    }
                }

                unlockedSkillLevels[x] = st;
            }

            /*
            unlockedSkillLevels.Clear();

            for (int i = 0; i < data.upgrades.Count; i++)
            {
                SkillType st = data.upgrades[i];
                int temp = st.dropdown.GetComponent<Dropdown>().value;
                for (int j = 0; j < playerUpgrades.Count; j++)
                {
                    if (playerUpgrades[j].name.Equals(data.upgrades[i].skillID))
                    {
                        st.dropdown = playerUpgrades[j].transform.GetChild(0).gameObject;
                        break;
                    }
                }
                //st.dropdown = playerUpgrades[i].transform.GetChild(0).gameObject; // Necessary because if scene is destroyed and recreated, values won't match and this fixes it
                st.dropdown.GetComponent<Dropdown>().value = temp;
                unlockedSkillLevels.Add(st);
            }


            */
            PutInValues();

            UpdateAllUIElements();
        }
        else
        {
            Debug.Log("File does not exist");
        }

    }

    public void ResetAll() // Resets everything changed by the current/previous save
    {
        foreach (SkillType st in unlockedSkillLevels)
        {
            playerUpgradeCurrency += st.GetTotalCurrencyCost();

            string skillType = st.skillType;
            float modifyBy = st.GetSkillAmountIncreased() * -1;
            playerStats_script.SetStat(ref skillType, modifyBy);
        }
        unlockedSkillLevels.Clear();
        PopulateList();
        UpdateAllUIElements();
    }

    public void PutInValues() // Puts values from the recently loaded save files into PlayerStats.cs
    {
        foreach (SkillType st in unlockedSkillLevels)
        {
            string skillType = st.skillType;
            float modifyBy = st.GetSkillAmountIncreased();
            //Debug.Log("Skill Type: " + skillType + " Modify By: " + modifyBy);
            playerStats_script.SetStat(ref skillType, modifyBy);
        }
    }


    // Format for adding new skill types is: (Dropdown GameObject, skill level, Name of GameObject in editor)
    public void UnlockSkill(GameObject parent) // Unlocks a skill, but this script mostly just creates the SkillType class instances
    {
        SkillType newSkill = new SkillType(parent, 0, parent.name); // It does not matter whether the name is this or that, it is not used, more for reference in code
        unlockedSkillLevels.Add(newSkill);
        UpdateUIElements(parent, newSkill);
    }

    /*
    public void SwitchedDropdown(GameObject dropdown) // When upgrade dropdown is switched, do all this
    {
        // Find the skill that the dropdown is associated with in unlockeSkillLevels
        SkillType currentClass = unlockedSkillLevels.Find(x => x.GetSkillID().Equals(dropdown.name));

        float modifyBy = currentClass.GetSkillAmountIncreased() * -1;
        playerUpgradeCurrency += currentClass.GetTotalCurrencyCost(); // Gives back currency, because skill level is reset as well

        playerStats_script.SetStat(ref skillType, modifyBy);

        unlockedSkillLevels.Remove(currentClass);
        SkillType replacementSkill = new SkillType(dropdown, 0, dropdown.name);
        unlockedSkillLevels.Add(replacementSkill);

        UpdateUIElements(dropdown, replacementSkill);
    }
    */
    

    // get gameObject, then find the name of the dropdown, then find the tier of the dropdown, then go into List and change class instance: public void AddPoints(GameObject childbutton)
    public void AddPoints(GameObject parent) // Adds points to an upgrade
    {
        string nameOfSkill = parent.name;
        SkillType currentClass = unlockedSkillLevels.Find(x => x.GetSkillID().Equals(nameOfSkill)); // Credit: https://stackoverflow.com/questions/9854917/how-can-i-find-a-specific-element-in-a-listt/9854944
        
        if (currentClass.GetCurrencyCost() > playerUpgradeCurrency || currentClass.IsMaxLevel() || currentClass == null)
        {
            Debug.Log("Not enough currency, stat already increased to max level, or the skill is null!");
        }
        else
        {
            if (Input.GetKey(KeyCode.LeftShift) && currentClass.GetCurrencyCost() * 10 < playerUpgradeCurrency && !currentClass.IsMaxLevel(10))
            {
                int currencyCost = currentClass.AddSkillLevel(10); // Have to get the currency cost of the operation
                playerUpgradeCurrency -= currencyCost;
                float modifyBy = currentClass.GetModifyValue(false, true);

                string upgradeType = currentClass.GetFormattedSkillType(parent);
                //Debug.Log("Upgrade Type: " + upgradeType);
                playerStats_script.SetStat(ref upgradeType, modifyBy);
            }
            else
            {
                int currencyCost = currentClass.AddSkillLevel(1); // Have to get the currency cost of the operation
                playerUpgradeCurrency -= currencyCost;
                float modifyBy = currentClass.GetModifyValue(false, false);

                string upgradeType = currentClass.GetFormattedSkillType(parent);
                //Debug.Log("Upgrade Type: " + upgradeType);
                playerStats_script.SetStat(ref upgradeType, modifyBy);
            }
            
        }

        UpdateUIElements(parent, currentClass);
    }

    public void SubtractPoints(GameObject parent) //Subtracts points from an upgrade
    {
        string nameOfSkill = parent.name;
        SkillType currentClass = unlockedSkillLevels.Find(x => x.GetSkillID() == nameOfSkill); // Credit: https://stackoverflow.com/questions/9854917/how-can-i-find-a-specific-element-in-a-listt/9854944
        
        if (currentClass.GetSkillLevel() < 1)
        {
            Debug.Log("Skill at Lowest Level");
        }
        else
        {
            if (Input.GetKey(KeyCode.LeftShift) && currentClass.GetSkillLevel() >= 10)
            {
                // This won't work with how GetModifyValue is coded currently
                float modifyBy = currentClass.GetModifyValue(true, true) * -1; // Have to do this here, before I subtract the skill level below
                int currencyCost = currentClass.SubtractSkillLevel(10); // function returns the currency cost of the operation
                playerUpgradeCurrency += currencyCost;

                string upgradeType = currentClass.GetFormattedSkillType(parent);
                playerStats_script.SetStat(ref upgradeType, modifyBy);
            }
            else
            {
                float modifyBy = currentClass.GetModifyValue(true, false) * -1; // Have to do this here, before I subtract the skill level below
                int currencyCost = currentClass.SubtractSkillLevel(1); // function returns the currency cost of the operation
                playerUpgradeCurrency += currencyCost;

                string upgradeType = currentClass.GetFormattedSkillType(parent);
                playerStats_script.SetStat(ref upgradeType, modifyBy);
            }
        }

        UpdateUIElements(parent, currentClass);
    }

    // I used to have it so that the player would have to unlock skills, but removed that in favor of having them be able to choose whatever
    void PopulateList() // Initializes and unlocks all the upgrades
    {
        foreach(GameObject g in playerUpgrades)
        {
            UnlockSkill(g);
        }
    }

    public void OpenUpgradesMenuStart() // When upgrade menu is opened, this updates the currency text
    {
        UpdateAllUIElements();
    }

    public Text playerUpgradeCurrencyTokensText; // Stores the text for playerUpgradeCurrency

    public void UpdateUIElements(GameObject parent, SkillType currentClass) // Calls UpdateUIElements() and updates a specific upgrade GameObject
    {
        UpdateUIElements();

        string colorVal = playerStats_script.GetColorForStat(currentClass.GetSkillType());
        parent.transform.GetChild(0).GetComponent<Text>().text = $"<color={colorVal}>{currentClass.assoc.name}</color>";
        parent.transform.GetChild(1).GetComponent<Text>().text = $"<color={colorVal}>+{currentClass.GetSkillAmountIncreased()}</color>";
        //gameManager.GetComponent<PlayerStats>().SetUpgradeText(currentClass.GetSkillType(), parent.transform.GetChild(0).gameObject, currentClass.GetSkillAmountIncreased());
    }

    public void UpdateUIElements() // Updates UI bars and currency
    {
        playerUpgradeCurrencyTokensText.text = "Upgrade Currency: " + playerUpgradeCurrency;
        playerStats_script.UpdateHealthEnduranceBars();
    }

    public void UpdateAllUIElements() // Updates every Upgrade block using the unlocked upgrades (which is all of them), the currency, and the UI bars
    {
        playerUpgradeCurrencyTokensText.text = "Upgrade Currency: " + playerUpgradeCurrency;
        playerStats_script.UpdateHealthEnduranceBars();

        foreach (SkillType st in unlockedSkillLevels)
        {
            GameObject parent = st.assoc;
            string colorVal = playerStats_script.GetColorForStat(st.GetSkillType());
            parent.transform.GetChild(0).GetComponent<Text>().text = $"<color={colorVal}>{parent.name}</color>";
            parent.transform.GetChild(1).GetComponent<Text>().text = $"<color={colorVal}>+{st.GetSkillAmountIncreased()}</color>";
        }

    }

    public void ResetAllSaveFiles()
    {
        // These two lines reset everything in the upgrades menu
        unlockedSkillLevels.Clear();
        PopulateList();

        // Puts the blank values that have been set above on all the save files
        InitializeFile(filePath1);
        InitializeFile(filePath2);
        InitializeFile(filePath3);
    }

    public void Awake()
    {
        // Initialize all the file paths
        // globalDataValuesPath = Application.persistentDataPath + "/globalDataValues.json"; // Global Data Values
        filePath1 = Application.persistentDataPath + "/playerUpgradeInfo1.json"; // Build 1
        filePath2 = Application.persistentDataPath + "/playerUpgradeInfo2.json"; // Build 2
        filePath3 = Application.persistentDataPath + "/playerUpgradeInfo3.json"; // Build 3
        gameStatePath = Application.persistentDataPath + "/gameStatePath.json"; // Game State Path
        currentBuildDropdownPath = filePath1;

    }

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindWithTag("GameController");
        playerStats_script = gameManager.GetComponent<PlayerStats>();

        //filePath = Application.persistentDataPath + "/playerUpgradeInfo.json";
        //Save(currentBuildDropdownPath);
        unlockedSkillLevels.Clear();
        PopulateList();

        if (!File.Exists(gameStatePath))
        {
            SaveGameState();
        }

        if (!File.Exists(filePath1))
        {
            InitializeFile(filePath1);
        }
        if (!File.Exists(filePath2))
        {
            InitializeFile(filePath2);
        }
        if (!File.Exists(filePath3))
        {
            InitializeFile(filePath3);
        }

        // TODO: Load starting file here
        
        //LoadGameState();
        Load(filePath1); // Using the currentBuildDropdownPath leads to bad outcomes
        UpdateUIElements();
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
}

[Serializable]
public class GlobalDataSave
{
    public int totalPlayerUpgradeCurrency;
}


