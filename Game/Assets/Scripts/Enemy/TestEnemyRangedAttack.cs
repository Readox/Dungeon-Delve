using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyRangedAttack : MonoBehaviour
{
    public GameObject projectile;
    private GameObject player;
    float minDamage;
    float maxDamage;
    float baseDamage;
    public float projectileSpeed;
    public float attackCooldown;

    public float removeDelay;

    Coroutine attackCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(ShootPlayer());  
        player = FindObjectOfType<PlayerMovement>().gameObject;
        baseDamage = GetComponent<EnemyStats>().GetDamage();
        minDamage = baseDamage - 2;
        maxDamage = baseDamage + 2;
    }

    public void StartRangedAttack()
    {
        attackCoroutine = StartCoroutine(AttackPlayer());
    }

    public void StopRangedAttack()
    {
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
        }
        
    }

/*
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Debug.Log("Player Entered CC");
            attackCoroutine = StartCoroutine(AttackPlayer());
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Debug.Log("Player Exited CC");
            if (attackCoroutine != null)
            {
                StopCoroutine(attackCoroutine);
            }
        }
    }
*/
    /*
    IEnumerator ShootPlayer()
    {
        if (player != null)
        {
            yield return new WaitForSeconds(attackCooldown);
            GameObject spell = Instantiate(projectile, transform.position, Quaternion.identity);
            Vector2 enemyPos = transform.position;
            Vector2 targetPos = player.transform.position;
            Vector2 direction = (targetPos - enemyPos).normalized;

            spell.GetComponent<Rigidbody2D>().velocity = direction * projectileSpeed;

            // I am going to overhaul the damage system later, and there will be randomization for the damage
            // Most of the damage will be based on the stats of the Enemy 
            // The enemies will not critically hit, except maybe bosses, but they will have strength and base damage
            spell.GetComponent<TestEnemyProjectile>().damage = (int)Random.Range(minDamage, maxDamage);

            attackCoroutine = StartCoroutine(ShootPlayer());
            //StartCoroutine(SpellTimeout(spell));  // Moved to TestEnemyProjectile

            
        }

    }
    */

    IEnumerator AttackPlayer()
    {
        if (player != null)
        {
            while(true)
            {
                //Debug.Log("Player Damage Coroutine Started");
                
                GameObject spell = Instantiate(projectile, transform.position, Quaternion.identity);
                Vector2 enemyPos = transform.position;
                Vector2 targetPos = player.transform.position;
                Vector2 direction = (targetPos - enemyPos).normalized;
                spell.GetComponent<Rigidbody2D>().velocity = direction * projectileSpeed;
                spell.GetComponent<TestEnemyProjectile>().damage = (int)Random.Range(minDamage, maxDamage);

                if (attackCooldown > float.Epsilon)
                {
                    yield return new WaitForSeconds(attackCooldown);
                }
                else
                {
                    break;
                }
            }
        }
    }

    /*
    IEnumerator SpellTimeout(GameObject spell)
    {
        yield return new WaitForSeconds(removeDelay);
        Destroy(spell);
    }
    */

    // Update is called once per frame
    void Update()
    {
        
    }
}
