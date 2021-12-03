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
        // 0 is for left click, 1 is for right click
        if (Input.GetMouseButtonDown(1) /*&& SceneManager.GetActiveScene().name.Equals("Level 0")*/ && Time.timeScale != 0)
        {
            GameObject spell = Instantiate(projectile, transform.position, Quaternion.identity);
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 playerPos = transform.position;
            Vector2 direction = (mousePos - playerPos).normalized;
            spell.GetComponent<Rigidbody2D>().velocity = direction * projectileSpeed;
            spell.GetComponent<CommonProjectile>().weaponDamage = this.weaponDamage;
            
            //Destroy(spell, removeDelay);

        }
        if (Input.GetMouseButtonDown(0) && Time.timeScale != 0)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 attackDir = (mousePos - transform.position).normalized;
            Debug.Log("Attack Direction: " + attackDir);
        }
             
    }
}
