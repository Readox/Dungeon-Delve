using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CommonProjectile : MonoBehaviour
{ 
    
    public float damage;
    public float removeDelay;

    void Start()
    {
        StartCoroutine(RemoveObject());
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player" && collision.tag != "PlayerProjectile")
        {
            if (collision.GetComponent<EnemyDamageReception>() != null) // Do this multiple times for ferocity procs
            {
                collision.GetComponent<EnemyDamageReception>().DealDamage(damage); 
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
