using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MeleeAttacks : CommonAttack
{
    public LayerMask enemyLayers;

    public float weaponDamage;
    public float attackRange; 
    public float attackRate;
    float nextAttackTime = 0f;
    Coroutine inCombatCoroutine;
    
    //Vector3 attackDir;
    //Vector3 attackPosition;

    Animator anim;
    PlayerMovement pm;
    Vector2 mousePos;
    Rigidbody2D rb;
    EnemyStats enemyStats_script;
    public AudioClip feroAudioClip;
    private Camera cam;
    public Vector3 offsetFromPlayer;
    public GameObject directionIndicatorObject;
    public float indicatorOffsetDistance;
    public float attackAngleOffset;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        pm = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        //mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        //mousePos = mousePos.normalized;
        //mousePos = mousePos * attackRange;
        //Debug.DrawLine(rb.position, mousePos, Color.blue);   

        DirectionIndicatorRotation();

        if (Time.time >= nextAttackTime) // from https://www.youtube.com/watch?v=sPiVz1k-fEs
        {
            if (Input.GetMouseButtonDown(0) && Time.timeScale != 0)
            {
                pm.DoPlayerCombatSpeed(0.4f);
                anim.SetTrigger("Attack");
                if (inCombatCoroutine != null)
                {
                    StopCoroutine(inCombatCoroutine);
                }
                inCombatCoroutine = StartCoroutine(ExitCombat(8f));
                CalculatePoints();
                DrawLines(); // For debugging
                CheckForHits();
                //OldAttack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
    }

    IEnumerator ExitCombat(float time)
    {
        pm.combatIdle = true;
        yield return new WaitForSeconds(time);
        pm.combatIdle = false;
    }

    private void DirectionIndicatorRotation()
    {
        Vector2 dir = mousePos - (Vector2) (transform.position + offsetFromPlayer);
        float rads = Mathf.Atan2(dir.y, dir.x);
        float angle = rads * Mathf.Rad2Deg;
        directionIndicatorObject.transform.localPosition = new Vector3(Mathf.Cos(rads), Mathf.Sin(rads) + indicatorOffsetDistance, 0);
        //Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        directionIndicatorObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle)); // rotation;
        //ConfigureSwordOffset(dir);
    }


    public int segments;
    public float curveDegrees;
    private float calcAngle;
    private List<Vector2> nodes = new List<Vector2>();

    private void CalculatePoints()
    {
        nodes.Clear();
        Vector2 mouseDirection = (mousePos - (Vector2)(transform.position + offsetFromPlayer));
        //nodes.Add(mouseDirection);

        calcAngle = Vector2.SignedAngle(transform.right + offsetFromPlayer, mouseDirection.normalized);
        float offsetAngle = calcAngle + attackAngleOffset;
        Vector2 startDir = Vector2FromAngle(offsetAngle);

        // Flips player sprite in direction of attack
        if ((calcAngle + attackAngleOffset) <= 90 && (calcAngle + attackAngleOffset) >= -90)
        {
            pm.FlipPlayerSpriteTo(true);
        }
        else
        {
            pm.FlipPlayerSpriteTo(false);
        }

        float startAngle = offsetAngle - (curveDegrees / 2);
        float endAngle = offsetAngle + (curveDegrees / 2);
        /*
        nodes.Add(Vector2FromAngle(startAngle));
        nodes.Add(Vector2FromAngle(endAngle));
        */
        nodes.Add(startDir); // this works 
        for (float i = startAngle; i <= endAngle; i += (curveDegrees/segments))
        {
            nodes.Add(Vector2FromAngle(i));
        }
    }

    //private List<GameObject> attackedEnemies = new List<GameObject>(); // For cleaving attacks, add enemies to this and use hits on them

    private void CheckForHits()
    {
        //attackedEnemies.Clear();
        RaycastHit2D hit;
        for (int i = 0; i <= nodes.Count - 1; i++)
        {
            hit = Physics2D.Raycast(transform.position + offsetFromPlayer, nodes[i], attackRange, enemyLayers.value);
            
            //hit = Physics2D.CircleCast(rb.position, 1f, (mousePos - rb.position).normalized, attackRange, enemyLayers.value);
            //hit = Physics2D.Linecast(rb.position, (mousePos - rb.position).normalized, enemyLayers.value);
            if (hit)
            {
                Attack(hit);
                //Debug.Log("Hit: " + hit.collider.gameObject.name); VERY helpful debug
                break;
            }
            
        }
    }

    private void DrawLines()
    {
        for (int i = 0; i <= nodes.Count - 1; i++)
        {
            Debug.DrawRay(transform.position + offsetFromPlayer, nodes[i], Color.red, attackRange);
            //Debug.DrawLine(nodes[i], nodes[i + 1], Color.red, 1.5f);
        }
    }

    void Attack(RaycastHit2D hit)
    {
        if (hit.collider.GetComponent<EnemyStats>() != null)
        {
            EnemyStats tempES = hit.collider.GetComponent<EnemyStats>();

            //SpawnMeleeAnimation(tempES.gameObject.transform, (mousePos - rb.position).normalized, calcAngle);

            if (!tempES.invulnerable)
            {
                enemyStats_script = tempES;
                enemyStats_script.DealDamage(CalculateDamage(weaponDamage, tempES.gameObject.transform, tempES.defense)); // initial attack

                for (int i = GetFerocityProcs(); i > 0; i--) // All ferocity procs
                {   
                    enemyStats_script.DealDamage(CalculateDamage(weaponDamage, tempES.gameObject.transform, tempES.defense));
                    //GameObject ferocityLine = Instantiate(ferocityLineObject, collision.transform.position, Quaternion.identity);
                    SpawnFerocityAnimation(tempES.gameObject.transform);

                    AudioSource.PlayClipAtPoint(feroAudioClip, hit.collider.gameObject.transform.position, 1); // plays ferocity proc audio
                    //ferocityLine.transform.SetParent(enemyStats_script.gameObject.transform);
            

                }
            }
            
        }
    }

    /*
    IEnumerator ShowSword(float time)
    {
        playerSword.SetActive(true);
        //ConfigurePlayerSword(playerSword, (mousePos - rb.position).normalized, calcAngle);
        yield return new WaitForSeconds(time);
        playerSword.SetActive(false);
    }
    */

    private Vector2 Vector2FromAngle(float a)
    {
        a *= Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(a), Mathf.Sin(a));
    }




    /*
    public float attackSizeX;
    public float attackSizeY;
    public float attackWidth; // Circlecast radius

    void OldAttack()
    {
        // Physics2D.Raycast(Vector2 origin, width, Vector2 DIRECTION, float distance, int LayerMask {bit shifting involved}, ......)
        RaycastHit2D collision = Physics2D.CircleCast(rb.position, attackWidth, (mousePos - rb.position).normalized, attackRange, enemyLayers.value);
        //Debug.DrawRay(rb.position, (mousePos - rb.position).normalized, Color.red, 3f);
        if (collision.collider != null)
        {
            if (collision.collider.tag == "Enemy" && collision.collider is BoxCollider2D)
            {
                if (collision.collider.GetComponent<EnemyStats>() != null)
                {
                    
                    SpawnMeleeAnimation(collision.collider.GetComponent<EnemyStats>().gameObject.transform, (mousePos - rb.position).normalized, calcAngle);

                    enemyStats_script = collision.collider.GetComponent<EnemyStats>();
                    float finalDamage = CalculateDamage(weaponDamage, collision.collider.GetComponent<EnemyStats>().gameObject.transform);
                    enemyStats_script.DealDamage(finalDamage); // initial attack

                    for (int i = GetFerocityProcs(); i > 0; i--) // All ferocity procs
                    {   
                        enemyStats_script.DealDamage(CalculateDamage(weaponDamage, collision.collider.GetComponent<EnemyStats>().gameObject.transform));
                        //GameObject ferocityLine = Instantiate(ferocityLineObject, collision.transform.position, Quaternion.identity);
                        SpawnFerocityAnimation(collision.collider.GetComponent<EnemyStats>().gameObject.transform);

                        AudioSource.PlayClipAtPoint(feroAudioClip, collision.transform.position, 1); // plays ferocity proc audio
                        //ferocityLine.transform.SetParent(enemyStats_script.gameObject.transform);
                        
                        
                        
                        // Sets Ferocity Line to be a child so that it gets hidden when enemy gets killed, so it doesn't stick around
                

                    }
                }
            }
        }

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
    }
    */

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
