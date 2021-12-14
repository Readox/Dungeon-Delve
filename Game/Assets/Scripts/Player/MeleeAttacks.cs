using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MeleeAttacks : CommonAttack
{
    public float attackCooldownTime;
    float attackTime;
    
    float attackOffset = 25f;
    // public string attackType; // Maybe later

    public float attackSizeX;
    public float attackSizeY;
    public LayerMask enemyLayers;

    public float weaponDamage;
    public float attackRange;

    Vector3 mousePos;
    Vector3 attackDir;
    Vector3 attackPosition;

    Rigidbody2D rb;
    EnemyStats enemyStats_script;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Input.mousePosition; /*Camera.main.ScreenToWorldPoint(Input.mousePosition)*/;
        //mousePos = mousePos.normalized;
        //mousePos = mousePos * attackRange;
        Debug.DrawLine(rb.position, mousePos, Color.red);

        if (attackTime <= 0)
        {
            if (Input.GetMouseButtonDown(0) && Time.timeScale != 0)
            {
                Attack();
            }
        }
        else
        {
            attackTime -= Time.deltaTime;
        }
        
    }

    void Attack()
    {
        
        RaycastHit2D collision = Physics2D.Raycast(rb.position, Input.mousePosition, attackRange);

        if (collision.collider != null)
        {
            Debug.Log("Raycast hit something");
            if (collision.collider.tag != "Player" && collision.collider.tag != "PlayerProjectile" && collision.collider is BoxCollider2D)
            {
                if (collision.collider.GetComponent<EnemyStats>() != null) // Do this multiple times for ferocity procs
                {
                    enemyStats_script = collision.collider.GetComponent<EnemyStats>();
                    enemyStats_script.DealDamage(CalculateDamage(weaponDamage)); // initial attack
                }
            }
        }

        /*
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        attackDir = (mousePos - transform.position).normalized;
        
        Debug.Log("Attack Direction: " + attackDir);
        
        attackPosition = transform.position + attackDir * attackOffset;
        float angle = Vector3.Angle(attackDir, transform.forward);
        Collider2D[] enemiesToAttack = Physics2D.OverlapBoxAll(attackPosition, new Vector2(attackSizeX, attackSizeY), angle, enemyLayers);

        foreach (Collider2D enemy in enemiesToAttack)
        {
            enemy.GetComponent<EnemyStats>().DealDamage(CalculateDamage(weaponDamage));
        }

        attackTime = attackCooldownTime;
        */
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackPosition, new Vector3(attackSizeX, attackSizeY, 1));
    }



    /*
    void PiercingAttack()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 attackDir = (mousePos - transform.position).normalized;

        // OverlapAreaAll() is for a rectangular area
        Collider2D[] attackedEnemies = Physics2D.OverlapAreaAll(attackPoint.position, attackRange, enemyLayers);

        Debug.Log("Attack Direction: " + attackDir);
    }

    void SlashingAttack()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 attackDir = (mousePos - transform.position).normalized;
        Debug.Log("Attack Direction: " + attackDir);
    }
    */

}
