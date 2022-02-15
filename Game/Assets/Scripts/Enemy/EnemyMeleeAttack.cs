using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyMeleeAttack : MonoBehaviour
{
    Coroutine damageCoroutine;
    Animator anim;
    ConditionManager conditionManager_Script;
    PlayerStats playerStats_script;
    AIPath AIPath_script;

    float damage;

    private Transform target;
    public float attackDelay;
    public float attackRange;
    public float attackAnimationLength; // not quite sure if this is fully necessary
    private float savedSpeed;

    void Awake()
    {
        damage = GetComponent<EnemyStats>().GetDamage();
        anim = GetComponent<Animator>();
        target = GameObject.FindWithTag("Player").GetComponent<Transform>();
        conditionManager_Script = GameObject.FindWithTag("GameController").GetComponent<ConditionManager>();
        playerStats_script = GameObject.FindWithTag("GameController").GetComponent<PlayerStats>();
        AIPath_script = transform.parent.gameObject.GetComponent<AIPath>();
        
        StartCoroutine(CheckForAttacks());
    }


    IEnumerator CheckForAttacks()
    {
        float dist = Vector3.Distance(transform.position, target.position);
        //Debug.Log("Distance: " + dist);
        if (dist < attackRange)
        {
            anim.SetBool("Attack", true);
            //Debug.Log("In Range");
            yield return new WaitForSeconds(attackAnimationLength);
        }
        yield return new WaitForSeconds(1);

        StartCoroutine(CheckForAttacks());
    }

    public void AnimationEventDamage()
    {
        float dist = Vector3.Distance(transform.position, target.position);
        if (dist < (attackRange * 1.5)) // Multiply the attack range to give the enemy a slight boost in range to combat diagonal movement
        {
            playerStats_script.DealDamage(damage); 
            if (gameObject.name.Substring(0,8).Equals("Scorpion"))
            {
                // new Conditions("Effect Name", # Effect Stacks, Duration)
                // new Conditions("Effect Name", Duration)
                conditionManager_Script.AddCondition(new Conditions("Poison", 3, 3));
                //conditionManager_Script.AddCondition(new Conditions("Slowness", 3));
                //conditionManager_Script.AddCondition(new Conditions("Bleeding", 5, 10));
            }
        }
        else // Don't complete attack animation if player is not in range
        {
            anim.SetBool("Walk", true);
            //anim.SetFloat("")
            //anim.Play("Walk", 0, 0.1f); // This seems to just delay the animation from occuring and locks the entity from moving
            //anim.Play("KlackonAltWalk", 0, 0); // This crashed Unity
            anim.SetBool("Attack", false);
            AnimationStartMovement();
        }
    }

    public void AnimationStopMovement()
    {
        savedSpeed = AIPath_script.maxSpeed;
        AIPath_script.maxSpeed = 0;
    }

    public void AnimationStartMovement()
    {
        AIPath_script.maxSpeed = savedSpeed;
    }

    public void EndAltWalk()
    {
        anim.SetBool("Walk", true);
        anim.SetBool("AltWalk", false);
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
