using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionManager : MonoBehaviour
{
    
    private List<Conditions> conditionsList = new List<Conditions>();
    private List<Boons> boonsList = new List<Boons>();

    public Coroutine bleedingTickCoroutine; 
    public Coroutine poisonTickCoroutine;
    public Coroutine burningTickCoroutine;

    public PlayerMovement playerMovement_script;
    public PlayerStats playerStats_script;



    void Awake()
    {
        conditionsList.Clear();
        boonsList.Clear();
        InvokeRepeating("EffectTick", 1f, 1f); // For Conditions and Boons
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

    public void AddCondition(Conditions c)
    {
        /*
        for(int i = 0; i < conditionsList.Count; i++)
        {
            if (conditionsList[i].effectName.Equals(c.effectName))
            {
                conditionsList[i].effectStacks += c.effectStacks;
                conditionsList[i].duration += c.duration;
                conditionsList[i].ReStart(); //TODO need restart method
                break();
            }
        }
        */
        if (c.effectName.Equals("Slowness") || c.effectName.Equals("Immobility"))
        {
            bool inList = false;
            for(int i = 0; i < conditionsList.Count; i++)
            {
                if (conditionsList[i].effectName.Equals(c.effectName))
                {
                    inList = true;
                    conditionsList[i].duration += c.duration;
                    break;
                }
            }
            if (inList == false)
            {
                conditionsList.Add(c);
                conditionsList[conditionsList.Count-1].OnStart();
            }
        }

        
    }




    IEnumerator ConditionDamageTick()
    {
        yield return new WaitForSeconds(1);
        playerStats_script.DealConditionDamage(1); // TODO do animations, do proper damage, take in effect name for anim, take in effectstacks for damage
    }


    public void StartConditionDamageCoroutine(string cName)
    {
        if (cName.Equals("Bleeding"))
        {
            bleedingTickCoroutine = StartCoroutine(ConditionDamageTick());
        }
        else if (cName.Equals("Poison"))
        {
            poisonTickCoroutine = StartCoroutine(ConditionDamageTick());
        }
        else if (cName.Equals("Burning"))
        {
            burningTickCoroutine = StartCoroutine(ConditionDamageTick());
        }
        else
        {
            Debug.Log("Invalid Condition!");
        }
    }
    public void StopConditionDamageCoroutine(string cName)
    {
        if (cName.Equals("Bleeding"))
        {
            StopCoroutine(bleedingTickCoroutine);
        }
        else if (cName.Equals("Poison"))
        {
            StopCoroutine(poisonTickCoroutine);
        }
        else if (cName.Equals("Burning"))
        {
            StopCoroutine(burningTickCoroutine);
        }
        else
        {
            Debug.Log("Invalid Condition!");
        }
    }

    public void DoSlowness()
    {
        playerMovement_script.DoSlowness();
    }
    public void DoImmobility()
    {
        playerMovement_script.DoImmobility();
    }
    public void ResetSpeed()
    {
        playerMovement_script.ResetSpeed();
    }


    public void StartHealthRegen()
    {
        playerStats_script.StartHealthRegen();
    }
    public void StopHealthRegen()
    {
        playerStats_script.StopHealthRegen();
    }


}
