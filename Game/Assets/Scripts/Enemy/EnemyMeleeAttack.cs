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

    private bool doRangedAttack;

    public bool applyCondition;
    public string effectName; // "Poison", "Bleeding", "Slowness", "Aegis", etc.
    public int effectStacks;
    public float effectDuration;
    public GameObject rangedProjectile;
    public float projectileSpeed;

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
        else if (!gameObject.transform.parent.gameObject.GetComponent<WanderingDestinationSetter>().enabled && rangedProjectile != null) // If enemy "sees" player
        {
            doRangedAttack = true;
            anim.SetBool("Attack", true);
            yield return new WaitForSeconds(attackAnimationLength);
            doRangedAttack = false;
        }
        yield return new WaitForSeconds(attackDelay); // Pretty sure that the Attack Delay goes here

        StartCoroutine(CheckForAttacks());
    }

    public void AnimationEventDamage()
    {
        float dist = Vector3.Distance(transform.position, target.position);
        if (doRangedAttack)
        {
            GameObject projectile = Instantiate(rangedProjectile, transform.position, Quaternion.identity);
            Vector2 direction = (target.position - transform.position).normalized;
            projectile.GetComponent<Rigidbody2D>().velocity = direction * (projectileSpeed);
            //projectile.GetComponent<EnemyProjectile>().SetConditions(effectName, effectStacks, effectDuration); // Conditions are set in prefab object

            // I have different prefab projectiles for each monster because the sprites are different, and I can set different conditions

            projectile.transform.rotation = Quaternion.Euler(new Vector3(0,0,180));
        }
        else
        {
            if (dist < attackRange * 1.5)
            {
                playerStats_script.DealDamage(damage); 
                conditionManager_Script.AddCondition(new Conditions(effectName, effectStacks, effectDuration));
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
    }

    public void AnimationStopMovement()
    {
        savedSpeed = AIPath_script.maxSpeed;
        if (!doRangedAttack)
        {
            AIPath_script.maxSpeed = 0;
        }
    }

    public void AnimationStartMovement()
    {
        AIPath_script.maxSpeed = savedSpeed;
    }

    public void EndAltWalk() // This isnt used I think
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
