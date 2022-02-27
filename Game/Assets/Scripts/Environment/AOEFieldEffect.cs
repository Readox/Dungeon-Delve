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
    public float damage; // This is affected by player defense and other damage reduction
    public string conditionName; // "Poison", "Bleeding", "Slowness", etc.
    public int conditionStacks;
    public float conditionDuration;


    void Awake()
    {
        conditionManager_script = GameObject.FindWithTag("GameController").GetComponent<ConditionManager>();
        playerStats_script = GameObject.FindWithTag("GameController").GetComponent<PlayerStats>();
        StartCoroutine(FindTargets());
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
                        conditionManager_script.AddCondition(new Conditions(conditionName, conditionStacks, conditionDuration));
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
