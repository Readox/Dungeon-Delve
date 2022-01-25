using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MeleeAttacks : CommonAttack
{
    //public float attackCooldownTime;
    //float attackTime;
    
    //float attackOffset = 25f;
    // public string attackType; // Maybe later

    public float attackSizeX;
    public float attackSizeY;
    public LayerMask enemyLayers;

    public float weaponDamage;
    public float attackRange;
    public float attackWidth; // Circlecast radius

    Vector2 mousePos;
    //Vector3 attackDir;
    //Vector3 attackPosition;

    Rigidbody2D rb;
    EnemyStats enemyStats_script;
    public AudioClip feroAudioClip;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //mousePos = mousePos.normalized;
        //mousePos = mousePos * attackRange;
        //Debug.DrawLine(rb.position, mousePos, Color.blue);

        if (Input.GetMouseButtonDown(0) && Time.timeScale != 0)
        {
            Attack();
        }
    }

    void Attack()
    {
        // Physics2D.Raycast(Vector2 origin, Vector2 DIRECTION, float distance, int LayerMask {bit shifting involved}, ......)
        RaycastHit2D collision = Physics2D.CircleCast(rb.position, attackWidth, (mousePos - rb.position).normalized, attackRange, enemyLayers.value);
        //Debug.DrawRay(rb.position, (mousePos - rb.position).normalized, Color.red, 3f);
        if (collision.collider != null)
        {
            if (collision.collider.tag == "Enemy" && collision.collider is BoxCollider2D)
            {
                if (collision.collider.GetComponent<EnemyStats>() != null)
                {
                    enemyStats_script = collision.collider.GetComponent<EnemyStats>();
                    enemyStats_script.DealDamage(CalculateDamage(weaponDamage)); // initial attack

                    for (int i = GetFerocityProcs(); i > 0; i--) // All ferocity procs
                    {   
                
                        enemyStats_script.DealDamage(CalculateDamage(weaponDamage));
                        //GameObject ferocityLine = Instantiate(ferocityLineObject, collision.transform.position, Quaternion.identity);

                        AudioSource.PlayClipAtPoint(feroAudioClip, collision.transform.position, 1); // plays ferocity proc audio
                        //ferocityLine.transform.SetParent(enemyStats_script.gameObject.transform);
                        
                        
                        // Sets Ferocity Line to be a child so that it gets hidden when enemy gets killed, so it doesn't stick around
                

                    }
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
