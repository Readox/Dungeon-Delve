using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMeleeEnemyMoveAttack : MonoBehaviour
{

    public GameObject player;

    public float moveSpeed;
    public float damage;

    Coroutine damageCoroutine;

    void Awake()
    {
        player = GameObject.FindWithTag("Player");
        //StartCoroutine(EnemyMove());
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            if (damageCoroutine == null)
            {
                damageCoroutine = StartCoroutine(DamagePlayer(damage, 1.0f));
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
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
            player.GetComponent<PlayerStats>().DealDamage(damage);
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

    IEnumerator EnemyMove()
    {
        //while(GetComponent<EnemyDamageReception>().)
        Vector2 enemyPos = transform.position;
        Vector2 targetPos = player.transform.position;
        Vector2 direction = (targetPos - enemyPos).normalized;
        this.GetComponent<Rigidbody2D>().velocity = direction * moveSpeed;

        yield return null;
    }


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
