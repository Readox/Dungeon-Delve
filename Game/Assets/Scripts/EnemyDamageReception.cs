using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageReception : MonoBehaviour
{
    public float health;
    public float maxHealth;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    public void DealDamage(float damage)
    {
        health -= damage;
        CheckDeath();
    }

    public void CheckHealthMax()
    {
        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }


    public void CheckDeath()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
