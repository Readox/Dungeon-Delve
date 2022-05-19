using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float damage; // This is affected by player defense and other damage reduction
    public float removeDelay;
    public bool playEffectOnHit;
    public GameObject effect;
    
    public string targetTag; // Possible values: "Player": will hit player     "Enemy": will hit enemies
    private ConditionManager conditionManager_script;
    private PlayerStats playerStats_script;

    // Here are the damage and condition variables  
    public bool applyCondition;
    public bool applyBoon;
    public string effectName; // "Poison", "Bleeding", "Slowness", "Aegis", etc.
    public int effectStacks;
    public float effectDuration;

    void Start()
    {
        StartCoroutine(RemoveObject());

        playerStats_script = GameObject.FindWithTag("GameController").GetComponent<PlayerStats>();
        if (applyCondition || applyBoon)
        {
            conditionManager_script = GameObject.FindWithTag("GameController").GetComponent<ConditionManager>();
        }
    }
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //gameManager.GetComponent<PlayerStats>().DealDamage(damage);
            playerStats_script.DealDamage(damage);
            if (applyCondition)
            {
                conditionManager_script.AddCondition(new Conditions(effectName, effectStacks, effectDuration));
            }
            if (applyBoon)
            {
                conditionManager_script.AddBoon(new Boons(effectName, effectStacks, effectDuration));
            }
            if(playEffectOnHit)
            {
                Instantiate(effect, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }
        if (collision.tag == "PlayerProjectile" || collision.tag == "Wall") // If the projectile hits either the player's projectile or a wall
        {
            if(playEffectOnHit)
            {
                Instantiate(effect, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }
    }

    public void SetConditions(string effectName, int effectStacks, float effectDuration)
    {
        this.effectName = effectName;
        this.effectStacks = effectStacks;
        this.effectDuration = effectDuration;
    }


    IEnumerator RemoveObject()
    {
        yield return new WaitForSeconds(removeDelay);
        Destroy(gameObject);
    }

}
