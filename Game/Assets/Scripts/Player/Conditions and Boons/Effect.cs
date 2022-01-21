using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{


    public PlayerStats playerStats_script;

    public void StartHealthRegen()
    {
        
    }

    public void StopHealthRegen()
    {
        StopCoroutine(playerStats_script.healthRegenCoroutine);
    }





















    // This class is unused


    /*
    public string effectName;
    public string affectedStat;
    public int effectStacks;
    public float duration;

    public Effect(string effectName, string affectedStat, int effectStacks, float duration)
    {
        this.effectName = effectName;
        this.affectedStat = affectedStat;
        this.effectStacks = effectStacks;
        this.duration = duration;
    }

    public Effect(string effectName, int effectStacks, float duration)
    {
        this.effectName = effectName;
        this.effectStacks = effectStacks;
        this.duration = duration;
    }
    */

    /*
    public void DoEffect()
    {

    }

    public void OnStart()
    {

    }

    public void OnDurationExpire()
    {

    }


    public void PersistentEffect()
    {

    }
    */


}
