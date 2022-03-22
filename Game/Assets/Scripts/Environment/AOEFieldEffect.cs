using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEFieldEffect : MonoBehaviour
{
    public float hitInterval;
    public string targetTag; // Possible values: "Player": will hit player     "Enemy": will hit enemies
    private ConditionManager conditionManager_script;
    private PlayerStats playerStats_script;
    
    private Collider2D[] targets;

    // Here are the damage and condition variables 
    public bool applyDamage; 
    public bool applyCondition;
    public bool applyBoon;
    public float damage; // This is affected by player defense and other damage reduction
    public string effectName; // "Poison", "Bleeding", "Slowness", "Aegis", etc.
    public int effectStacks;
    public float effectDuration;
    public float destroyAfterTime;
    public float effectAfterTime;

    void Awake()
    {
        conditionManager_script = GameObject.FindWithTag("GameController").GetComponent<ConditionManager>();
        playerStats_script = GameObject.FindWithTag("GameController").GetComponent<PlayerStats>();
        if (destroyAfterTime > 0)
        {
            StartCoroutine(DestroyAfterTime());
        }
        if (effectAfterTime == 0)
        {
            StartCoroutine(FindTargets());
        }
        else
        {
            StartCoroutine(EffectAfterTime());
        }
    }

    IEnumerator EffectAfterTime()
    {
        yield return new WaitForSeconds(effectAfterTime);
        targets = Physics2D.OverlapCircleAll(transform.position, transform.localScale.x/2 /*gets radius*/); // I can't get the layer masks to work, so this is slower than it should be
        DoAOEEffect(targets);
    }

    IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(destroyAfterTime);
        Destroy(gameObject);
    }

    IEnumerator FindTargets()
    {
        targets = Physics2D.OverlapCircleAll(transform.position, transform.localScale.x/2 /*gets radius*/); // I can't get the layer masks to work, so this is slower than it should be
        DoAOEEffect(targets);
        yield return new WaitForSeconds(hitInterval);
        StartCoroutine(FindTargets());
    }

    private void DoAOEEffect(Collider2D[] targets)
    {
        // Need to use if statements because of differing methods of applying damage to entities, which is an unfortunate product of me not knowing what I was doing
        if (targetTag.Equals("Player")) // Does the following for players
        {
            foreach (Collider2D cd in targets)
            {
                if (cd.gameObject.tag.Equals("Player"))
                {
                    if (applyDamage)
                    {
                        //cd.parent.gameObject.GetComponent<PlayerStats>().DealDamage(damage);
                        playerStats_script.DealDamage(damage);
                    }
                    if (applyCondition)
                    {
                        // new Conditions("Effect Name", # Effect Stacks, Duration)
                        // new Conditions("Effect Name", Duration)
                        conditionManager_script.AddCondition(new Conditions(effectName, effectStacks, effectDuration));
                    }
                    if (applyBoon)
                    {
                        conditionManager_script.AddBoon(new Boons(effectName, effectStacks, effectDuration));
                    }
                }
                
            }
        }
        else // Does the following for enemies, does not apply conditions, but maybe later can implement a slow or immobilize field or something
        {
            foreach (Collider2D cd in targets)
            {
                if (applyDamage)
                {
                    cd.gameObject.GetComponent<EnemyStats>().DealDamage(damage);
                }
            }
        }
        
    }

}
