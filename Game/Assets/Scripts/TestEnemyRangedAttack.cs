using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyRangedAttack : MonoBehaviour
{
    public GameObject projectile;
    public Transform playerLoc;
    public float minDamage;
    public float maxDamage;
    public float projectileSpeed;
    public float attackCooldown;

    public float removeDelay;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ShootPlayer());
        
    }

    IEnumerator ShootPlayer()
    {
        yield return new WaitForSeconds(attackCooldown);
        if (playerLoc != null)
        {
            GameObject spell = Instantiate(projectile, transform.position, Quaternion.identity);
            Vector2 enemyPos = transform.position;
            Vector2 targetPos = playerLoc.position;
            Vector2 direction = (targetPos - enemyPos).normalized;

            spell.GetComponent<Rigidbody2D>().velocity = direction * projectileSpeed;
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
