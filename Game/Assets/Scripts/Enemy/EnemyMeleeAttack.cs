using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeAttack : MonoBehaviour
{
    GameObject gameManager;
    Coroutine damageCoroutine;
    Animator anim;

    float damage;

    


    void Awake()
    {
        gameManager = GameObject.FindWithTag("GameController");
        damage = GetComponent<EnemyStats>().GetDamage();
        anim = GetComponent<Animator>();
        //StartCoroutine(EnemyMove());
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && collision is BoxCollider2D/*&& !collision is CircleCollider2D*/) 
        {
            if (damageCoroutine == null)
            {
                damageCoroutine = StartCoroutine(DamagePlayer(damage, 1.0f)); 
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && collision is BoxCollider2D/*&& !collision is CircleCollider2D*/)
        {
            //anim.SetBool("Attack", false);
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
            anim.SetBool("Attack", true);
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
}
