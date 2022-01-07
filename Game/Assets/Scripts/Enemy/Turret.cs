using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public GameObject projectile;
    private GameObject player;
    float minDamage;
    float maxDamage;
    float baseDamage;
    public float projectileSpeed;
    public float attackCooldown;

    public Animator anim;

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
