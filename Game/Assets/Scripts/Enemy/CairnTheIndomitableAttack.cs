using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class CairnTheIndomitableAttack : MonoBehaviour
{
    Coroutine damageCoroutine;
    Animator anim;
    ConditionManager conditionManager_Script;
    PlayerStats playerStats_script;
    AIPath AIPath_script;
    EnemyStats enemyStats_script;
    public GameObject cairnProjectile;
    public float cairnProjectileSpeed;
    public float projectileRemoveDelay;
    public GameObject cairnAOE;

    float damage;
    int attackCounter;

    private Transform target;
    private Coroutine attackCheckCoroutine;
    public float attackDelay;
    public float attackRange;
    public float attackAnimationLength; // not quite sure if this is fully necessary
    private float savedSpeed;

    void Awake()
    {
        enemyStats_script = GetComponent<EnemyStats>();
        damage = GetComponent<EnemyStats>().GetDamage();
        anim = GetComponent<Animator>();
        target = GameObject.FindWithTag("Player").GetComponent<Transform>();
        conditionManager_Script = GameObject.FindWithTag("GameController").GetComponent<ConditionManager>();
        playerStats_script = GameObject.FindWithTag("GameController").GetComponent<PlayerStats>();
        AIPath_script = transform.parent.gameObject.GetComponent<AIPath>();
        
        attackCheckCoroutine = StartCoroutine(CheckForAttacks());
    }


    IEnumerator CheckForAttacks()
    {
        Debug.Log("Cairn Check for Attacks");
        float dist = Vector3.Distance(transform.position, target.position);
        //Debug.Log("Distance: " + dist);
        if (dist < attackRange)
        {
            anim.SetBool("Attack", true);
            //Debug.Log("In Range");
            yield return new WaitForSeconds(attackAnimationLength);
        }
        yield return new WaitForSeconds(attackDelay); // Pretty sure that the Attack Delay goes here

        attackCheckCoroutine = StartCoroutine(CheckForAttacks());
    }

    public void AnimationEventDamage()
    {
        float dist = Vector3.Distance(transform.position, target.position);
        if (attackCounter % 3 == 0) // this is used in addition to other attacks
        {
            for (int i = 0; i < 360; i += 45)
            {
                GameObject projectile = Instantiate(cairnProjectile, transform.position, Quaternion.identity);
                float pdxp = transform.position.x + Mathf.Sin((i * Mathf.PI) / 180) * 1;
                float pdyp = transform.position.y + Mathf.Cos((i * Mathf.PI) / 180) * 1;
                Vector2 self = transform.position;
                Vector2 pVec = new Vector2 (pdxp, pdyp);
                Vector2 direction = (pVec - self).normalized;
                projectile.GetComponent<Rigidbody2D>().velocity = direction  * cairnProjectileSpeed;
                projectile.GetComponent<TestEnemyProjectile>().damage = enemyStats_script.baseDamage;
                projectile.GetComponent<TestEnemyProjectile>().removeDelay = projectileRemoveDelay;
                projectile.transform.rotation = Quaternion.LookRotation(Vector3.back, direction);
            }
        }
        if (attackCounter % 8 == 0 && attackCounter != 0) // RockFall Attack
        {
            rockTicker = 0;
            StartCoroutine(InvulnerabilityTimer(5f));
            StartCoroutine(RockFallAttack(2)); // Takes in argument for rocks per tick
            attackCounter += 1;
        }
        if (dist < attackRange * 1.5)
        {
            playerStats_script.DealDamage(damage);
            attackCounter += 1; 
        }
        /*
        else // Don't complete attack animation if player is not in range
        {
            anim.SetBool("Walk", true);
            //anim.SetFloat("")
            //anim.Play("Walk", 0, 0.1f); // This seems to just delay the animation from occuring and locks the entity from moving
            anim.SetBool("Attack", false);
            AnimationStartMovement();
        }
        */
    }

    void Update()
    {
        /*
        if (enemyStats_script.currentHealth <= (enemyStats_script.maxHealth / 4) * 3) // At 75% health
        {
            // Play Animation
            // Chaotic Release ability : spawn green AOE safe field that applies Aegis buff (blocks attacks)
            // Deal damage equal to player 99% of player health
        }
        */
    }

    private int rockTicker;
    
    IEnumerator RockFallAttack(int rocksPerTick)
    {
        yield return new WaitForSeconds(0.2f);

        for (int i = 0; i <= rocksPerTick; i++) // I put this down here because it eliminates some king of bug where some AOE fields spawn in and then disappear almost immediately
        {
            Instantiate(cairnAOE, new Vector3(transform.position.x + Random.Range(-10f, 10f), transform.position.y + Random.Range(-10f, 10f), 0), Quaternion.identity);
        }
        if (rockTicker < 30)
        {
            rockTicker += 1;
            StartCoroutine(RockFallAttack(rocksPerTick));
        }
    }

    IEnumerator InvulnerabilityTimer(float time)
    {
        StopCoroutine(attackCheckCoroutine);
        enemyStats_script.invulnerable = true; 
        yield return new WaitForSeconds(time);
        enemyStats_script.invulnerable = false;
        attackCheckCoroutine = StartCoroutine(CheckForAttacks());
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
