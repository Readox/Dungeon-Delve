using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class CommonAttack : MonoBehaviour
{

    public GameObject gameManager;
    public Transform playerPos;
    public float meleeOffset;
    private PlayerStats playerStats_script;
    public GameObject damageIndicator; // damageIndicator prefab
    public GameObject ferocityAnimation;
    public GameObject meleeAnimation;

    public float CalculateDamage(float weaponDamage, Transform targetPos) // Takes in Transform for damage indicator location
    {
        float damage = 10 + weaponDamage; // Base Weapon Damage = 10
        bool criticalHit = false;
        if (GetRandFloat(0,100) > GetCritChance()) // No Critical Hit
        {
            damage = (10 + weaponDamage) * (1 + GetStrength()/100);
        }
        else // Critical Hit
        {
            damage = (10 + weaponDamage) * (1 + (GetStrength()/100)) * (1 + (GetCritDamage()/100));
            criticalHit = true;
        }

        if (GetCritChance() > 100)
        {
            damage *= 1 + ((GetCritChance()-100)/100);
        }
        //Debug.Log("Final Damage: " + damage);

        float finalDamage =float.Parse(damage.ToString("N0")); // Truncates Decimals

        GameObject di = Instantiate(damageIndicator, targetPos.position, Quaternion.identity);
        ConfigureDamageIndicator(di, targetPos, finalDamage, criticalHit);

        return finalDamage;
    }


    private void ConfigureDamageIndicator(GameObject di, Transform newParent, float damage, bool criticalHit) // The new parent is the attacked entity
    {
        di.transform.SetParent(newParent.parent);
        di.GetComponent<TextMeshPro>().text = damage.ToString(); 
        if (criticalHit)
        {
            di.GetComponent<TextMeshPro>().color = new Color32(255, 0, 0, 255);
        }
        else
        {
            di.GetComponent<TextMeshPro>().color = new Color32(255, 108, 0, 255);
        }
    }

    public void SpawnFerocityAnimation(Transform newParent)
    {
        bool result  = (UnityEngine.Random.value > 0.5f); // https://gamedev.stackexchange.com/questions/110332/is-there-a-random-command-for-boolean-variables-in-unity-c
        GameObject fero = Instantiate(ferocityAnimation, newParent.position, Quaternion.identity);
        if (result) // selects which anim state to use
        {
            fero.GetComponent<Animator>().SetFloat("Type", 1);
        }
        else
        {
            fero.GetComponent<Animator>().SetFloat("Type", 0);
        }
        fero.transform.SetParent(newParent);
        //fero.gameObject.transform.localScale = new Vector3(newParent.transform.localScale.x, newParent.transform.localScale.y, newParent.transform.localScale.z);
        fero.transform.position = newParent.position; // this might be redundant
    }

    public void SpawnMeleeAnimation(Transform newParent, Vector2 direction2, float angle) // help with this acquired from: https://stackoverflow.com/questions/58050314/i-have-a-problem-with-instantiate-direction-and-offset
    { 
        Vector3 origin = playerPos.position; // + additional stuff?
        Vector3 direction = direction2;
        origin += direction * meleeOffset;
        //bool result  = (UnityEngine.Random.value > 0.5f); // https://gamedev.stackexchange.com/questions/110332/is-there-a-random-command-for-boolean-variables-in-unity-c
        GameObject melee = Instantiate(meleeAnimation, origin, Quaternion.identity);
        melee.transform.Rotate(0, 0, angle, Space.World);

        melee.transform.SetParent(newParent);
        //melee.gameObject.transform.localScale = new Vector3(newParent.transform.localScale.x, newParent.transform.localScale.y, newParent.transform.localScale.z);
        //melee.transform.position = newParent.position; // this might be redundant
    }

    public bool MakeCriticalHitAttempt()
    {
        bool r = false;
        if (GetRandFloat(0,100) > GetCritChance()) // No Critical Hit
        {
            r = false;
        }
        else // Critical Hit
        {
            r = true;
        }
        return r;
    }


    public int GetFerocityProcs()
    {
        float fero = GetFerocity();
        int procs = (int)((fero - (fero % 100))/100);
        if (GetRandFloat(0,100) < (fero % 100))
        {
            procs += 1;
        }
        /*
        if ((fero / 100) < 1)
        if (GetRandFloat(0,100) > (fero % 100))
        {
            procs += 1;
        }
        */

        //Debug.Log("Ferocity: " + fero + "\nProcs: " + procs);
        return procs;
    }

    public float GetStrength()
    {
        return playerStats_script.Strength;
    }

    public float GetCritChance()
    {
        return playerStats_script.CritChance;
    }

    public float GetCritDamage()
    {
        return playerStats_script.CritDamage;
    }

    public float GetFerocity()
    {
        return playerStats_script.Ferocity;
    }

    public float GetRandFloat(float min, float max)
    {
        System.Random random = new System.Random();
        double val = (random.NextDouble() * (max - min) + min);
        return (float) val;
    }



    // Start is called before the first frame update
    void Start()
    {
        //gameManager = GameObject.FindWithTag("GameController");
        //playerStats_script = gameManager.GetComponent<PlayerStats>();
    }

    void Awake()
    {
        gameManager = GameObject.FindWithTag("GameController");
        playerStats_script = gameManager.GetComponent<PlayerStats>();
        //playerStats_script = gameManager.GetComponent<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Older version of above
    public float CalculateDamage(float weaponDamage) // Eventually this will take weapons and such as args (maybe)
    {
        float damage = 10 + weaponDamage; // Base Weapon Damage = 10
        //bool criticalHit = false;
        if (GetRandFloat(0,100) > GetCritChance()) // No Critical Hit
        {
            damage = (10 + weaponDamage) * (1 + GetStrength()/100);
        }
        else // Critical Hit
        {
            damage = (10 + weaponDamage) * (1 + (GetStrength()/100)) * (1 + (GetCritDamage()/100));
            //criticalHit = true;
        }

        if (GetCritChance() > 100)
        {
            damage *= 1 + ((GetCritChance()-100)/100);
        }
        //Debug.Log("Final Damage: " + damage);
        return damage;
    }
}
