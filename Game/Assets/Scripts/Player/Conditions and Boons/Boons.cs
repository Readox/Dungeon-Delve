using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boons : MonoBehaviour
{
    public string effectName;
    public string affectedStat;
    public int effectStacks;
    public float duration;
    public Coroutine damageTickCoroutine;
    public float originalStat;

    private PlayerMovement playerMovementScript;
    private PlayerStats playerStats_script;

    public Boons(string effectName, string affectedStat, int effectStacks, float duration)
    {
        this.effectName = effectName;
        this.affectedStat = affectedStat;
        this.effectStacks = effectStacks;
        this.duration = duration;
    }

    public Boons(string effectName, int effectStacks, float duration)
    {
        this.effectName = effectName;
        this.effectStacks = effectStacks;
        this.duration = duration;
    }

    public void DoEffect() // Make sure to subtract duration;
    {
        duration -= 1f;
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



}
