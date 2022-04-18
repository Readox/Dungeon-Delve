using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerStats : MonoBehaviour
{

    // Because Domain and Scene reloading is off, I need to be wary about this being static, but it appears to be fine for now
    public Transform playerTransform;
    public static PlayerStats playerStats;
    PlayerSkills playerSkills_script;
    public GameObject player;
    public GameObject statsScrollRect;
    public GameObject screenText;

    public Slider healthBarSlider;
    public Text healthText;
    public Slider enduranceBarSlider;
    public Text enduranceText;
    public Text healthPotionText;

    public string playerClass;

    [HideInInspector] public Coroutine healthRegenCoroutine;
    private bool healthRegenIsRunning = false;
    [HideInInspector] public Coroutine enduranceRegenCoroutine;

    public float healthPotionAmount; // I am making it so that players get an amount of health potions to use per room or dungeon (base heal potions will not be crafted but higher tiers will)
    private float currentHealthPotionAmount;
    public float healthPotionBaseHeal;
    [HideInInspector] public bool poisoned; // if true, divide all health potion heals by 2

    private bool isEvading = false;
    [HideInInspector] public bool invulnerable = false;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        currentEndurancePool = endurancePoolMax;
        currentHealthPotionAmount = healthPotionAmount;


        // healthRegenCoroutine = StartCoroutine(HealthRegeneration()); 
        // healthRegenIsRunning = true;
        enduranceRegenCoroutine = StartCoroutine(EnduranceRegeneration());
    }
    public IEnumerator HealthRegeneration()
    {
        yield return new WaitForSeconds(1);
        currentHealth += maxHealth * (HealthRegen / 100);
        CheckHealthMax();
        SetHealthInfo();
        healthRegenCoroutine = StartCoroutine(HealthRegeneration());
    }
    public IEnumerator EnduranceRegeneration()
    {
        yield return new WaitForSeconds(1);
        currentEndurancePool += endurancePoolMax * (EnduranceRegen / 100);
        CheckEnduranceMax();
        SetEnduranceInfo();
        enduranceRegenCoroutine = StartCoroutine(EnduranceRegeneration());
    }

    // Update is called once per frame
    void Update() // I am using update to detect whether the player is using a Heal Potion
    {
        if (Input.GetKey(KeyCode.H))
        {
            UseHealthPotion();
        }
    }

    void Awake()
    {
        playerStats = this;
        
        playerSkills_script = GameObject.FindWithTag("PauseMenu").GetComponent<PlayerSkills>();
        // For safeties:
        // Actually, dont have this on anything except for _game in the preload scene
        // DontDestroyOnLoad(this);

        PlayerClassSetup();
        UpdateScrollRectStats(statsScrollRect);

        Color color;
        ColorUtility.TryParseHtmlString(endurancePoolColor, out color);

        enduranceBarSlider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = color; // Sets endurance pool color to: x
        enduranceBarSlider.gameObject.transform.Find("Text").GetComponent<Text>().color = color; // Sets endurance pool text to x:

        currentHealth = maxHealth;
        currentEndurancePool = endurancePoolMax;

        healthBarSlider.value = 1;
        healthText.text = Mathf.Ceil(currentHealth).ToString() + " / " + Mathf.Ceil(maxHealth).ToString();

        enduranceBarSlider.value = 1;
        enduranceText.text = Mathf.Ceil(currentEndurancePool).ToString() + " / " + Mathf.Ceil(endurancePoolMax).ToString();
    }

    private bool onCooldown = false;
    private void UseHealthPotion()
    {
        if (currentHealthPotionAmount > 0 && !onCooldown)
        {
            if (poisoned)
            {
                currentHealth += (healthPotionBaseHeal + (maxHealth * (HealthRegen / 100))) / 2;
            }
            else
            {
                currentHealth += healthPotionBaseHeal + (maxHealth * (HealthRegen / 100));
            }
            currentHealthPotionAmount -= 1;
            CheckHealthMax();
            SetHealthInfo();
            SetHealthPotionIndicatorInfo();
            StartCoroutine(HealthPotionCooldown(1));
        }
    }
    public void ResetHealthPotionAmount()
    {
        currentHealthPotionAmount = healthPotionAmount;
        SetHealthPotionIndicatorInfo();
    }
    IEnumerator HealthPotionCooldown(float time)
    {
        onCooldown = true;
        yield return new WaitForSeconds(time);
        onCooldown = false;

    }

    public void DealDamage(float damage)
    {
        if (isEvading)
        {
            SpawnEvadeIndicator();
        }
        else if (invulnerable)
        {
            SpawnInvulnerableIndicator();
            gameObject.GetComponent<ConditionManager>().RemoveAegis();
        }
        else
        {
            float dmgredper = Defense / (Defense + 100);
            if (dmgredper == 0) { dmgredper = 1; }
            float finalDamage = damage * dmgredper;
            currentHealth -= finalDamage;
            CheckDeath();
            SetHealthInfo();
        }
        
    }
    public void DealConditionDamage(float damage)
    {
        currentHealth -= damage;
        CheckDeath();
        SetHealthInfo();
    }

    public void StartHealthRegen()
    {
        if (!healthRegenIsRunning)
        {
            healthRegenCoroutine = StartCoroutine(HealthRegeneration());
            healthRegenIsRunning = true;
        }
        
    }
    public void StopHealthRegen()
    {
        if (healthRegenCoroutine != null)
        {
            StopCoroutine(healthRegenCoroutine);
            healthRegenIsRunning = false;
        }
    }

    public void SetUIActiveState(string state)
    {
        if (state.Equals("true"))
        {
            healthBarSlider.gameObject.transform.parent.parent.gameObject.SetActive(true);
        }
        else
        {
            healthBarSlider.gameObject.transform.parent.parent.gameObject.SetActive(false);
        }
        
    }
    
    public void EnduranceExpend(float enduranceCost)
    {
        currentEndurancePool -= enduranceCost;
        CheckEnduranceMax();
        SetEnduranceInfo();

    }
    public void CheckEnduranceMax()
    {
        if (currentEndurancePool > endurancePoolMax)
        {
            currentEndurancePool = endurancePoolMax;
        }
        if (currentEndurancePool < 0)
        {
            currentEndurancePool = 0;
        }
    }
    public void HealCharacter(float healAmount)
    {
        currentHealth += healAmount;
        CheckHealthMax();
        SetHealthInfo();
    }
    private void SpawnInvulnerableIndicator()
    {
        GameObject inv = Instantiate(screenText, playerTransform.position, Quaternion.identity);
        inv.transform.SetParent(playerTransform);
        inv.transform.position = playerTransform.position;
        inv.GetComponent<TextMeshPro>().text = "Invulnerable";
        inv.GetComponent<TextMeshPro>().color = new Color32(0, 195, 147, 255);
    }
    private void SpawnEvadeIndicator()
    {
        GameObject ev = Instantiate(screenText, playerTransform.position, Quaternion.identity);
        ev.transform.SetParent(playerTransform);
        ev.transform.position = playerTransform.position; // this might be redundant
        ev.GetComponent<TextMeshPro>().text = "Dodge"; 
        ev.GetComponent<TextMeshPro>().color = new Color32(0, 207, 0, 255);
    }
    public string GetClass()
    {
        return playerClass;
    }
    public bool GetEvadingStatus()
    {
        return isEvading;
    }
    private float CalculateEndurancePercentage()
    {
        return currentEndurancePool / endurancePoolMax;
    }
    private float CalculateHealthPercentage()
    {
        return currentHealth / maxHealth;
    }
    private void SetEnduranceInfo()
    {
        enduranceBarSlider.value = CalculateEndurancePercentage();
        enduranceText.text = Mathf.Ceil(currentEndurancePool).ToString() + " / " + Mathf.Ceil(endurancePoolMax).ToString();
    }
    private void SetHealthInfo()
    {
        healthBarSlider.value = CalculateHealthPercentage();
        healthText.text = Mathf.Ceil(currentHealth).ToString() + " / " + Mathf.Ceil(maxHealth).ToString();
    }
    private void SetHealthPotionIndicatorInfo()
    {
        healthPotionText.text = currentHealthPotionAmount.ToString();
    }
    public void UpdateHealthEnduranceBars()
    {
        SetEnduranceInfo();
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
    public void FlipEvading()
    {
        isEvading = !isEvading;
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

    public float getCurrentEndurancePool()
    {
        return currentEndurancePool;
    }


    public void PlayerClassSetup()
    {
        if (playerClass.Equals("fighter"))
        {
            endurancePoolColor = "#FF2800";
        }
        else if (playerClass.Equals("mage"))
        {
            endurancePoolColor = "#43FFF8";
        }
        else if (playerClass.Equals("archer"))
        {
            endurancePoolColor = "#02F805";
        }
        else if (playerClass.Equals("tank"))
        {
            endurancePoolColor = "#335E2C";
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
            $" <color={healthColor}>Health Regeneration: {HealthRegen}%</color> \n" +
            $" <color={endurancePoolColor}>Endurance Regeneration: {EnduranceRegen}%</color> \n" +
            $" <color={endurancePoolColor}>Endurance Pool: {endurancePoolMax}</color> \n" +
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
        else if (skillType.Equals("endurancePoolMax") || skillType.Equals("EnduranceRegen"))
        {
            return endurancePoolColor;
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
    public string endurancePoolColor;

    public float endurancePoolMax;
    public float currentEndurancePool;


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
    // Base Value: 1% = 1
    public float HealthRegen;

    // Percentage of endurance energy regenerated every second
    // Base value 2% = 2
    public float EnduranceRegen;

    // Chance for rare drops from boss chests and monsters
    // Formula = baseDropChance * (1 + (MagicFind / 100))
    // Base Value: 0%
    public float MagicFind;

    // Base Damage is given by the weapon being used, and is what is being multiplied by strength and crit damage,
    //      then increased by other percentages and fero



}
