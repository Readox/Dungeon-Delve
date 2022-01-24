using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{

    // Because Domain and Scene reloading is off, I need to be wary about this being static, but it appears to be fine for now
    public static PlayerStats playerStats;
    PlayerSkills playerSkills_script;
    public GameObject player;
    public GameObject statsScrollRect;

    public Slider healthBarSlider;
    public Text healthText;
    public Slider abilityBarSlider;
    public Text abilityText;

    public string playerClass;

    public List<Conditions> conditionsList = new List<Conditions>();
    public List<Boons> boonsList = new List<Boons>();

    public Coroutine healthRegenCoroutine;
    public Coroutine abilityRegenCoroutine;
    public Coroutine damageTickCoroutine; // For condition damage
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        currentAbilityPool = abilityPoolMax;


        healthRegenCoroutine = StartCoroutine(HealthRegeneration());
        abilityRegenCoroutine = StartCoroutine(AbilityRegeneration());
    }
    public IEnumerator HealthRegeneration()
    {
        yield return new WaitForSeconds(1);
        currentHealth += maxHealth * HealthRegen;
        CheckHealthMax();
        SetHealthInfo();
        healthRegenCoroutine = StartCoroutine(HealthRegeneration());
    }
    public IEnumerator AbilityRegeneration()
    {
        yield return new WaitForSeconds(1);
        currentAbilityPool += abilityPoolMax * AbilityRegen;
        CheckAbilityMax();
        SetAbilityInfo();
        abilityRegenCoroutine = StartCoroutine(AbilityRegeneration());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Awake()
    {
        playerStats = this;
        
        playerSkills_script = GameObject.FindWithTag("PauseMenu").GetComponent<PlayerSkills>();
        // For safeties:
        // Actually, dont have this on anything except for _game in the preload scene
        // DontDestroyOnLoad(this);

        conditionsList.Clear();
        boonsList.Clear();
        InvokeRepeating("EffectTick", 1f, 1f); // For Conditions and Boons


        PlayerClassSetup();
        UpdateScrollRectStats(statsScrollRect);

        Color color;
        ColorUtility.TryParseHtmlString(abilityPoolColor, out color);

        abilityBarSlider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = color; // Sets ability pool color to: x
        abilityBarSlider.gameObject.transform.Find("Text").GetComponent<Text>().color = color; // Sets ability pool text to x:

        currentHealth = maxHealth;
        currentAbilityPool = abilityPoolMax;

        healthBarSlider.value = 1;
        healthText.text = Mathf.Ceil(currentHealth).ToString() + " / " + Mathf.Ceil(maxHealth).ToString();

        abilityBarSlider.value = 1;
        abilityText.text = Mathf.Ceil(currentAbilityPool).ToString() + " / " + Mathf.Ceil(abilityPoolMax).ToString();
    }

    void EffectTick()
    {
        for (int i = 0; i < conditionsList.Count; i++) // need to do a for loop instead of foreach because of exceptions being thrown, same below
        {
            Conditions c = conditionsList[i];
            c.DoEffect();
            if (c.duration == 0f)
            {
                c.OnDurationExpire();
                conditionsList.Remove(c);
            }
        }
        for (int o = 0; o < boonsList.Count; o++)
        {
            Boons b = boonsList[o];
            b.DoEffect();
            if (b.duration == 0f)
            {
                b.OnDurationExpire();
                boonsList.Remove(b);
            }
        }
    }

    public void DealDamage(float damage)
    {
        float dmgredper = Defense / (Defense + 100);
        if (dmgredper == 0) { dmgredper = 1; }
        float finalDamage = damage * dmgredper;
        currentHealth -= finalDamage;
        CheckDeath();
        SetHealthInfo();
    }
    public void DealConditionDamage(float damage)
    {
        currentHealth -= damage;
        CheckDeath();
        SetHealthInfo();
    }

    IEnumerator ConditionDamageTick()
    {
        yield return new WaitForSeconds(1);
        //float finalDamage =  Condition Effect Stacks;
        //playerStats_script.DealConditionDamage(finalDamage);
    }

    public void StartHealthRegen()
    {
        healthRegenCoroutine = StartCoroutine(HealthRegeneration());
    }
    public void StopHealthRegen()
    {
        StopCoroutine(healthRegenCoroutine);
    }
    public void StartConditionDamageCoroutine()
    {
        damageTickCoroutine = StartCoroutine(ConditionDamageTick());
    }
    public void StopConditionDamageCoroutine()
    {
        StopCoroutine(damageTickCoroutine);
    }

    
    public void AbilityExpend(float abilityCost)
    {
        currentAbilityPool -= abilityCost;
        CheckAbilityMax();
        SetAbilityInfo();

    }
    public void CheckAbilityMax()
    {
        if (currentAbilityPool > abilityPoolMax)
        {
            currentAbilityPool = abilityPoolMax;
        }
        if (currentAbilityPool < 0)
        {
            currentAbilityPool = 0;
        }
    }
    public void HealCharacter(float healAmount)
    {
        currentHealth += healAmount;
        CheckHealthMax();
        SetHealthInfo();
    }
    public string GetClass()
    {
        return playerClass;
    }
    private float CalculateAbilityPercentage()
    {
        return currentAbilityPool / abilityPoolMax;
    }
    private float CalculateHealthPercentage()
    {
        return currentHealth / maxHealth;
    }
    private void SetAbilityInfo()
    {
        abilityBarSlider.value = CalculateAbilityPercentage();
        abilityText.text = Mathf.Ceil(currentAbilityPool).ToString() + " / " + Mathf.Ceil(abilityPoolMax).ToString();
    }
    private void SetHealthInfo()
    {
        healthBarSlider.value = CalculateHealthPercentage();
        healthText.text = Mathf.Ceil(currentHealth).ToString() + " / " + Mathf.Ceil(maxHealth).ToString();
    }
    public void UpdateHealthAbilityBars()
    {
        SetAbilityInfo();
        SetHealthInfo();
    }
    public void CheckHealthMax()
    {
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
    }
    public void CheckDeath()
    {
        if (currentHealth <= 0)
        {
            Destroy(player);
            MainMenu();
        }
    }
    public void AddUpgradeCurrency(int amount)
    {
        playerSkills_script.playerUpgradeCurrency += amount;
    }

    public void AddUpgradeTokens(int amount)
    {
        playerSkills_script.playerUpgradeTokens += amount;
    }

    public void MainMenu()
    {
        Time.timeScale = 0f;
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Additive);
        StartCoroutine(SceneSwitch());
    }
    IEnumerator SceneSwitch()
    {
        //SceneManager.SetActiveScene(SceneManager.GetSceneByName("MainMenu"));
        yield return null;
        SceneManager.UnloadSceneAsync("Level 0");
    }

    public float getCurrentAbilityPool()
    {
        return currentAbilityPool;
    }


    public void PlayerClassSetup()
    {
        if (playerClass.Equals("fighter"))
        {
            abilityPoolColor = "#FF2800";
        }
        else if (playerClass.Equals("mage"))
        {
            abilityPoolColor = "#43FFF8";
        }
        else if (playerClass.Equals("archer"))
        {
            abilityPoolColor = "#02F805";
        }
        else if (playerClass.Equals("tank"))
        {
            abilityPoolColor = "#335E2C";
        }
        else
        {
            Debug.Log("Select a class please!");
        }
    }

    // Everytime that SetStat is called, I will also call a method that updates the scroll rect containing player stats
    public void SetStat(ref string skillType, float modifyBy)
    {
        //Debug.Log("SkillType: " + skillType + " ModifyBy: " + modifyBy);

        // ngl, didnt think that this would work first try, especially because it looks kinda wack, but alr
        float currentVal = (float)this.GetType().GetField(skillType).GetValue(this);
        this.GetType().GetField(skillType).SetValue(this, currentVal + modifyBy);
        UpdateScrollRectStats(statsScrollRect);
    }

    public void UpdateScrollRectStats(GameObject scrollRect)
    {
        Text childText = scrollRect.GetComponentInChildren<Text>();
        // The curly bracket parts are working, as well as assignment but not the <> areas
        string outputString =
            $" <color={healthColor}>Health: {maxHealth}</color> \n" +
            $" <color={defenseColor}>Defense: {Defense}</color> \n" +
            $" <color={strengthColor}>Strength: {Strength}</color> \n" +
            $" <color={ccColor}>Critical Chance: {CritChance}</color> \n" +
            $" <color={cdColor}>Critical Damage: {CritDamage}</color> \n" +
            $" <color={ferocityColor}>Ferocity: {Ferocity}</color> \n" + 
            $" <color={healthColor}>Health Regeneration: {HealthRegen}</color> \n" +
            $" <color={abilityPoolColor}>Ability Regeneration: {AbilityRegen}</color> \n" +
            $" <color={abilityPoolColor}>Ability Pool: {abilityPoolMax}</color> \n" +
            $" <color={mfColor}>Magic Find: {MagicFind}</color> \n";
        childText.text = outputString;
    }

    // I don't like having all of these if statements, but I gotta do it
    public string GetColorForStat(string skillType)
    {
        if(skillType.Equals("maxHealth") || skillType.Equals("HealthRegen"))
        {
            return healthColor;
        }
        else if (skillType.Equals("Defense"))
        {
            return defenseColor;
        }
        else if (skillType.Equals("Strength"))
        {
            return strengthColor;
        }
        else if (skillType.Equals("CritChance"))
        {
            return ccColor;
        }
        else if (skillType.Equals("CritDamage"))
        {
            return cdColor;
        }
        else if (skillType.Equals("Ferocity"))
        {
            return ferocityColor;
        }
        else if (skillType.Equals("abilityPoolMax") || skillType.Equals("AbilityRegen"))
        {
            return abilityPoolColor;
        }
        else if (skillType.Equals("MagicFind"))
        {
            return mfColor;
        }

        return "#000000";
    }


    private string healthColor = "#9D0000";    // Stat(s) Implemented
    private string defenseColor = "#00C803";   // Stat(s) Implemented
    private string strengthColor = "#C80800";  // 
    private string ccColor = "#0009FF";        // 
    private string cdColor = "#0046FF";        // 
    private string ferocityColor = "#FF5F00";  // 
    private string mfColor = "#FF00D0";        // 



    /*
<color = #9D0000>Health</color>:
<color = #9D0000>Health</color>:
<color = #9D0000>Health</color>:
<color = #9D0000>Health</color>:
<color = #9D0000>Health</color>:
<color = #9D0000>Health</color>:
<color = #9D0000>Health</color>:
<color = #9D0000>Health</color>:
    */




    // All this stuff is down here, because there is a lot
    public string abilityPoolColor;

    public float abilityPoolMax;
    public float currentAbilityPool;


    // Current Health that the player has
    public float currentHealth;

    // Maximum health that the player will have
    // Base Value: 100
    public float maxHealth;

    // Amount that damage is reduced by (eg. player has 2 def, takes a 3 damage hit, the damage is reduced by the defense so the player only takes 1 damage)
    // Hypixel Skyblock Formula: Damage Reduction (%)  = Def / (Def + 100)
    // My Formula: Damage Reduction (%)  = Def / (Def + 100)
    // Base Value: 0
    public float Defense;

    // Amount that base damage is multiplied by (regardless of critical hit)
    // Base Value: 1
    public float Strength;

    // Percent Chance to critically hit
    // Base Value: 25%
    public float CritChance;

    // Damage multiplier (percentage) on critical hits
    // Base Value: 50%
    public float CritDamage;

    // Percentages above 100% (and there will be) will overflow into triple hits, then quadruple hits, and oh boy this is going to be hard to balance
    // Base Value: 0% 
    public float Ferocity;

    // Percentage of MaxHealth regenerated every second
    // Base Value: 1% = 0.01
    public float HealthRegen;

    // Percentage of ability energy regenerated every second
    // Base value 2% = 0.02
    public float AbilityRegen;

    // Chance for rare drops from boss chests and monsters
    // Formula = baseDropChance * (1 + (MagicFind / 100))
    // Base Value: 0%
    public float MagicFind;

    // Base Damage is given by the weapon being used, and is what is being multiplied by strength and crit damage,
    //      then increased by other percentages and fero



}
