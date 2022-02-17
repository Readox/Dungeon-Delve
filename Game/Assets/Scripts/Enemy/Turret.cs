using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public GameObject projectile;
    private Transform target;
    public Animator anim;

    Coroutine attackCoroutine;

    float minDamage;
    float maxDamage;
    float baseDamage;
    public float projectileSpeed;
    public float attackDelay;
    public float removeDelay;
    public float attackRange;
    public float attackAnimationLength;
    

    void Awake()
    {
        target = GameObject.FindWithTag("Player").GetComponent<Transform>();
        baseDamage = GetComponent<EnemyStats>().GetDamage();
        minDamage = baseDamage - 2;
        maxDamage = baseDamage + 2;

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
        yield return new WaitForSeconds(attackDelay); // Pretty sure that the attackDelay goes here

        StartCoroutine(CheckForAttacks());
    }

    public void FireProjectile()
    {
        GameObject spell = Instantiate(projectile, transform.position, Quaternion.identity);
        Vector2 enemyPos = transform.position;
        Vector2 targetPos = target.position;
        Vector2 direction = (targetPos - enemyPos).normalized;
        spell.GetComponent<Rigidbody2D>().velocity = direction * projectileSpeed;
        spell.GetComponent<TestEnemyProjectile>().damage = (int)Random.Range(minDamage, maxDamage);
    }

    public void EndAttackAnimation() // I need have an animation event that calls this or the enemy will be stuck in an attack loop
    {
        //anim.SetBool("Idle", true); // Animator for enemies don't have an "Idle" bool
        anim.SetBool("Attack", false);
    }


    /*
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            attackCoroutine = StartCoroutine(AttackPlayer());
            //anim.SetBool("Attack", true);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StopCoroutine(attackCoroutine);
            //anim.SetBool("Attack", false);
        }
    }

    IEnumerator AttackPlayer()
    {
        if (player != null)
        {
            while(true)
            {
                anim.SetBool("Attack", true);
                //Debug.Log("Player Damage Coroutine Started");
                
                GameObject spell = Instantiate(projectile, transform.position, Quaternion.identity);
                Vector2 enemyPos = transform.position;
                Vector2 targetPos = player.transform.position;
                Vector2 direction = (targetPos - enemyPos).normalized;
                spell.GetComponent<Rigidbody2D>().velocity = direction * projectileSpeed;
                spell.GetComponent<TestEnemyProjectile>().damage = (int)Random.Range(minDamage, maxDamage);

                if (attackDelay > float.Epsilon)
                {
                    yield return new WaitForSeconds(attackDelay);
                }
                else
                {
                    break;
                }
            }
        }
    }

    
    IEnumerator SpellTimeout(GameObject spell)
    {
        yield return new WaitForSeconds(removeDelay);
        Destroy(spell);
    }
    */

    
}
