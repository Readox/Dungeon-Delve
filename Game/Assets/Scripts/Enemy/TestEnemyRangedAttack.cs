using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyRangedAttack : MonoBehaviour
{
    public GameObject projectile;
    private GameObject player;
    public float minDamage;
    public float maxDamage;
    public float projectileSpeed;
    public float attackCooldown;

    public float removeDelay;

    Coroutine attackCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(ShootPlayer());  
        player = FindObjectOfType<PlayerMovement>().gameObject;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player Entered CC");
            attackCoroutine = StartCoroutine(ShootPlayer());
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player Exited CC");
            if (attackCoroutine != null)
            {
                StopCoroutine(attackCoroutine);
            }
        }
    }

    IEnumerator ShootPlayer()
    {
        if (player != null)
        {
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


            yield return new WaitForSeconds(attackCooldown);
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
