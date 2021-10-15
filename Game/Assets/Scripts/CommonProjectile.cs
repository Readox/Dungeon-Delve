using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonProjectile : MonoBehaviour
{ 
    
    public float damage;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player" && collision.tag != "PlayerProjectile")
        {
            if (collision.GetComponent<EnemyDamageReception>() != null)
            {
                collision.GetComponent<EnemyDamageReception>().DealDamage(damage); 
            }
            Destroy(gameObject);
        }
    }

}
