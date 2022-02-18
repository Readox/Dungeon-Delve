using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conditions
{
    public string effectName;
    public int effectStacks;
    public float duration;

    private ConditionManager cm;

    public Conditions(string effectName, int effectStacks, float duration)
    {
        this.effectName = effectName;
        this.effectStacks = effectStacks;
        this.duration = duration;
        cm = GameObject.FindWithTag("GameController").GetComponent<ConditionManager>();
    }

    public Conditions(string effectName, float duration)
    {
        this.effectName = effectName;
        this.duration = duration;
        cm = GameObject.FindWithTag("GameController").GetComponent<ConditionManager>();
    }

    public void DoEffect() // Make sure to subtract duration;
    {
        duration -= 1f;
    }

    public void OnStart()
    {
        if (effectName.Equals("Bleeding"))
        {
            // Do something special here
        }
        else if (effectName.Equals("Poison"))
        {
            cm.StopHealthRegen();
        }
        else if (effectName.Equals("Burning"))
        {
            // Do something special here
        }
        else if (effectName.Equals("Slowness"))
        {
            cm.DoSlowness();
        }
        else if (effectName.Equals("Immobility"))
        {
            cm.DoImmobility();
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
            if (!cm.CheckForInstanceOf(effectName))
            {
                cm.RemoveEffectAnimation(effectName);
            }
        }
        else if (effectName.Equals("Poison"))
        {
            cm.StartHealthRegen();
            if (!cm.CheckForInstanceOf(effectName))
            {
                cm.RemoveEffectAnimation(effectName);
            }
        }
        else if (effectName.Equals("Burning"))
        {
            
        }
        else if (effectName.Equals("Slowness"))
        {
            cm.ResetSpeed();
        }
        else if (effectName.Equals("Immobility"))
        {
            cm.ResetSpeed();
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
