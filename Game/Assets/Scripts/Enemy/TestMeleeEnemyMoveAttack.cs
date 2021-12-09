using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMeleeEnemyMoveAttack : MonoBehaviour
{

    private GameObject gameManager;

    float damage;

    Coroutine damageCoroutine;


    void Awake()
    {
        gameManager = GameObject.FindWithTag("GameController");
        damage = GetComponent<EnemyStats>().GetDamage();
        //StartCoroutine(EnemyMove());
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && collision is BoxCollider2D)
        {
            Debug.Log("Here");
            if (damageCoroutine == null)
            {
                damageCoroutine = StartCoroutine(DamagePlayer(damage, 1.0f));
            }

        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && collision is BoxCollider2D)
        {
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;
            }
        }
    }

    IEnumerator DamagePlayer(float damage, float interval)
    {
        while(true)
        {
            Debug.Log("Player Damage Coroutine Started");
            gameManager.GetComponent<PlayerStats>().DealDamage(damage);
            if (interval > float.Epsilon)
            {
                yield return new WaitForSeconds(interval);
            }
            else
            {
                break;
            }
        }
    }

    /*
    IEnumerator EnemyMove()
    {
        //while(GetComponent<EnemyDamageReception>().)
        Vector2 enemyPos = transform.position;
        Vector2 targetPos = player.transform.position;
        Vector2 direction = (targetPos - enemyPos).normalized;
        this.GetComponent<Rigidbody2D>().velocity = direction * moveSpeed;

        yield return null;
    }
    */

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
