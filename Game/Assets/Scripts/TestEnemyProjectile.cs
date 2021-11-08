using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyProjectile : MonoBehaviour
{

    public float damage;
    public float removeDelay;
    //public GameObject gameManager;

    void Start()
    {
        StartCoroutine(RemoveObject());
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.tag != "Enemy")
        {
            if (collision.tag == "Player")
            {
                //gameManager.GetComponent<PlayerStats>().DealDamage(damage);
                PlayerStats.playerStats.DealDamage(damage);
            }
            Destroy(gameObject);
        }
    }


    IEnumerator RemoveObject()
    {
        yield return new WaitForSeconds(removeDelay);
        Destroy(gameObject);
    }



}
