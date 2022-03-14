using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boons
{
    public string effectName;
    public int effectStacks;
    public float duration;

    private ConditionManager cm;

    public Boons(string effectName, int effectStacks, float duration)
    {
        this.effectName = effectName;
        this.effectStacks = effectStacks;
        this.duration = duration;
        cm = GameObject.FindWithTag("GameController").GetComponent<ConditionManager>();
    }

    public Boons(string effectName, float duration)
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
        if (effectName.Equals("Aegis"))
        {
            cm.ActivateAegis();
        }
    }

    public void OnDurationExpire()
    {
        if (effectName.Equals("Aegis"))
        {
            cm.RemoveAegis();
            cm.RemoveEffectAnimation(effectName);
        }
    }


    public void PersistentEffect()
    {

    }



}
