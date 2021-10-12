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

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ShootPlayer());
        player = FindObjectOfType<PlayerMovement>().gameObject;
    }

    IEnumerator ShootPlayer()
    {
        yield return new WaitForSeconds(attackCooldown);
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

            StartCoroutine(ShootPlayer());
            StartCoroutine(SpellTimeout(spell));



        }

    }

    ///*
    IEnumerator SpellTimeout(GameObject spell)
    {
        yield return new WaitForSeconds(removeDelay);
        Destroy(spell);
    }
    //*/

    // Update is called once per frame
    void Update()
    {
        
    }
}
