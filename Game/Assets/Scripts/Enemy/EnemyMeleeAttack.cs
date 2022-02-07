using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeAttack : MonoBehaviour
{
    Coroutine damageCoroutine;
    Animator anim;
    ConditionManager conditionManager_Script;
    PlayerStats playerStats_script;

    float damage;

    private Transform target;
    public float attackDelay;
    public float attackRange;
    private float lastTime;

    void Update()
    {
        float dist = Vector3.Distance(transform.position, target.position);
        if (dist < attackRange)
        {
            Debug.Log("In Range");
            if (Time.time > lastTime + attackDelay)
            {
                anim.SetBool("Attack", true);
                playerStats_script.DealDamage(damage); Debug.Log("Dealt Damage");
                if (gameObject.name.Substring(0,8).Equals("Scorpion"))
                {
                    // new Conditions("Effect Name", # Effect Stacks, Duration)
                    // new Conditions("Effect Name", Duration)
                    //Debug.Log("Added Bleeding");
                    //conditionManager_Script.AddCondition(new Conditions("Poison", 3, 3));
                    //conditionManager_Script.AddCondition(new Conditions("Slowness", 3));
                    //conditionManager_Script.AddCondition(new Conditions("Bleeding", 5, 10));
                }
                lastTime += Time.time;
            }
        }
    }


    void Awake()
    {
        damage = GetComponent<EnemyStats>().GetDamage();
        anim = GetComponent<Animator>();
        target = GameObject.FindWithTag("Player").GetComponent<Transform>();
        conditionManager_Script = GameObject.FindWithTag("GameController").GetComponent<ConditionManager>();
        playerStats_script = GameObject.FindWithTag("GameController").GetComponent<PlayerStats>();
    }

    /*

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player")) 
        {
            if (damageCoroutine == null)
            {
                damageCoroutine = StartCoroutine(DamagePlayer(damage, 1.0f)); 
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player")) 
        {
            if (damageCoroutine == null)
            {
                damageCoroutine = StartCoroutine(DamagePlayer(damage, 1.0f)); 
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            //anim.SetBool("Attack", false);
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;
            }
        }
    }

    IEnumerator DamagePlayer(float damage, float interval)
    {
        
        anim.SetBool("Attack", true);
        playerStats_script.DealDamage(damage);
        if (gameObject.name.Substring(0,8).Equals("Scorpion"))
        {
            // new Conditions("Effect Name", # Effect Stacks, Duration)
            // new Conditions("Effect Name", Duration)
            Debug.Log("Added Poison");
            conditionManager_Script.AddCondition(new Conditions("Poison", 3, 3));
            //conditionManager_Script.AddCondition(new Conditions("Slowness", 3));
            //conditionManager_Script.AddCondition(new Conditions("Bleeding", 5, 10));
        }
        yield return new WaitForSeconds(interval);
    }

    */
}
