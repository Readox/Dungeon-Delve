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

    


    void Awake()
    {
        damage = GetComponent<EnemyStats>().GetDamage();
        anim = GetComponent<Animator>();
        conditionManager_Script = GameObject.FindWithTag("GameController").GetComponent<ConditionManager>();
        playerStats_script = GameObject.FindWithTag("GameController").GetComponent<PlayerStats>();
        //StartCoroutine(EnemyMove());
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player") /*&& collision is BoxCollider2D*//*&& !collision is CircleCollider2D*/) 
        {
            if (damageCoroutine == null)
            {
                damageCoroutine = StartCoroutine(DamagePlayer(damage, 1.0f)); 
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player") /*&& collision is BoxCollider2D*//*&& !collision is CircleCollider2D*/) 
        {
            if (damageCoroutine == null)
            {
                damageCoroutine = StartCoroutine(DamagePlayer(damage, 1.0f)); 
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player") /*&& collision is BoxCollider2D*//*&& !collision is CircleCollider2D*/)
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
        while(true)
        {
            anim.SetBool("Attack", true);
            playerStats_script.DealDamage(damage);
            if (gameObject.name.Substring(0,8).Equals("Scorpion"))
            {
                Debug.Log("Added Poison");
                // new Conditions("Effect Name", # Effect Stacks, Duration)
                // new Conditions("Effect Name", Duration)

                //conditionManager_Script.AddCondition(new Conditions("Poison", 3, 3));
                conditionManager_Script.AddCondition(new Conditions("Slowness", 3));
            }
            if (interval > float.Epsilon)
            {
                yield return new WaitForSeconds(interval);
            }
            else
            {
                break;
            }
        }
    }
}
