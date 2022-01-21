using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conditions : Effect
{
    public string effectName;
    public string affectedStat;
    public int effectStacks;
    public float duration;
    public Coroutine damageTickCoroutine;
    public float originalStat;

    private PlayerMovement playerMovementScript;
    public PlayerStats playerStats_script;

    public Conditions(string effectName, string affectedStat, int effectStacks, float duration)
    {
        this.effectName = effectName;
        this.affectedStat = affectedStat;
        this.effectStacks = effectStacks;
        this.duration = duration;
        playerStats_script = GameObject.FindWithTag("GameController").GetComponent<PlayerStats>();
    }

    public Conditions(string effectName, int effectStacks, float duration)
    {
        this.effectName = effectName;
        this.effectStacks = effectStacks;
        this.duration = duration;
        playerStats_script = GameObject.FindWithTag("GameController").GetComponent<PlayerStats>();
    }

    public void DoEffect() // Make sure to subtract duration;
    {
        duration -= 1f;
    }

    IEnumerator DamageTick()
    {
        yield return new WaitForSeconds(1);
        float finalDamage = effectStacks;
        playerStats_script.DealConditionDamage(finalDamage);
    }

    public void OnStart()
    {
        if (effectName.Equals("Bleeding"))
        {
            damageTickCoroutine = StartCoroutine(DamageTick());
        }
        else if (effectName.Equals("Poison"))
        {
            base.playerStats_script = this.playerStats_script;

            base.StopHealthRegen();

            /*
            playerStats_script.StopCoroutine(playerStats_script.healthRegenCoroutine);
            damageTickCoroutine = StartCoroutine(DamageTick());
            */
        }
        else if (effectName.Equals("Burning"))
        {
            damageTickCoroutine = StartCoroutine(DamageTick());
        }
        else if (effectName.Equals("Slowness"))
        {
            playerMovementScript = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
            originalStat = playerMovementScript.playerSpeed;
            playerMovementScript.playerSpeed = originalStat / 2;
        }
        else if (effectName.Equals("Immobility"))
        {
            playerMovementScript = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
            originalStat = playerMovementScript.playerSpeed;
            playerMovementScript.playerSpeed = 0;
        }
        else if (effectName.Equals("Weakness"))
        {
            Debug.Log("Not Implemented Yet");
        }
        else
        {
            Debug.Log("No Effect Name defined");
        }
    }

    public void OnDurationExpire()
    {
        if (effectName.Equals("Bleeding"))
        {
            StopCoroutine(damageTickCoroutine);
        }
        else if (effectName.Equals("Poison"))
        {
            playerStats_script.healthRegenCoroutine = playerStats_script.StartCoroutine(playerStats_script.HealthRegeneration());
            StopCoroutine(damageTickCoroutine);
        }
        else if (effectName.Equals("Burning"))
        {
            StopCoroutine(damageTickCoroutine);
        }
        else if (effectName.Equals("Slowness"))
        {
            if(playerMovementScript.playerSpeed > originalStat / 2) // Accounts for a change during condition
            {
                playerMovementScript.playerSpeed = originalStat + (playerMovementScript.playerSpeed - (originalStat / 2));
            }
            else
            {
                playerMovementScript.playerSpeed = originalStat;
            }
        }
        else if (effectName.Equals("Immobility"))
        {
            if(playerMovementScript.playerSpeed > 0) // Accounts for a change during condition
            {
                playerMovementScript.playerSpeed = originalStat + playerMovementScript.playerSpeed;
            }
            else
            {
                playerMovementScript.playerSpeed = originalStat;
            }
        }
        else if (effectName.Equals("Weakness"))
        {
            Debug.Log("Not Implemented Yet");
        }
        else
        {
            Debug.Log("No Effect Name defined");
        }
    }


    public void PersistentEffect()
    {

    }


}
