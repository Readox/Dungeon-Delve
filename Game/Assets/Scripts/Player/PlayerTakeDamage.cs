using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTakeDamage : MonoBehaviour
{

    Coroutine damageCoroutine;
    private GameObject gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindWithTag("GameController");
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(/*collision.gameObject.CompareTag("Enemy") &&*/ collision is BoxCollider2D)
        {
            if (collision.tag == "Enemy")
            {
                if (damageCoroutine == null)
                {
                    float damage = collision.gameObject.GetComponent<EnemyStats>().GetDamage();
                    damageCoroutine = StartCoroutine(DamagePlayer(damage, 1.0f));
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy") && collision is BoxCollider2D)
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
