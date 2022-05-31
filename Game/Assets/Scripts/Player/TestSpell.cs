using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestSpell : MonoBehaviour
{
 
    public GameObject projectile;
    public float projectileSpeed;
    public float weaponDamage;
    public float removeDelay;
    public float attackRate;
    float nextAttackTime;
    public Vector2 offsetFromPlayer;

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(RemoveObject());
    }

    /* This is a Co-Routine function thingy that runs alongside stuff
    IEnumerator RemoveObject()
    {
        yield return new WaitForSeconds(2f);
        Destroy spell;
    }
    */
    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextAttackTime) // from https://www.youtube.com/watch?v=sPiVz1k-fEs
        {
            if (Input.GetMouseButtonDown(1) /*&& SceneManager.GetActiveScene().name.Equals("Level 0")*/ && Time.timeScale != 0)
            {
                Vector2 playerPos = transform.position;
                GameObject spell = Instantiate(projectile, playerPos + offsetFromPlayer, Quaternion.identity);
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 direction = (mousePos - playerPos + offsetFromPlayer).normalized;
                spell.GetComponent<Rigidbody2D>().velocity = direction * projectileSpeed;
                spell.GetComponent<CommonProjectile>().weaponDamage = this.weaponDamage;
                nextAttackTime = Time.time + 1f / attackRate;
                
                //Destroy(spell, removeDelay);
            }
        }
        // 0 is for left click, 1 is for right click
        
        /*
        if (Input.GetMouseButtonDown(0) && Time.timeScale != 0)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 attackDir = (mousePos - transform.position).normalized;
            Debug.Log("Attack Direction: " + attackDir);
        }
        */    
    }
}
