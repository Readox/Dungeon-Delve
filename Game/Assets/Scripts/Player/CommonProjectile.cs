using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CommonProjectile : CommonAttack
{ 
    
    public float weaponDamage;
    public float removeDelay;

    public GameObject ferocityLineObject;

    public AudioClip feroAudioClip;

    EnemyStats enemyStats_script;

    void Start()
    {
        StartCoroutine(RemoveObject());
        base.gameManager = GameObject.FindWithTag("GameController");
        
        //ferocityLineObject = GameObject.FindWithTag("FerocityLine");
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player" && collision.tag != "PlayerProjectile" && collision is BoxCollider2D)
        {
            if (collision.GetComponent<EnemyStats>() != null) // Do this multiple times for ferocity procs
            {
                enemyStats_script = collision.GetComponent<EnemyStats>();
                enemyStats_script.DealDamage(CalculateDamage(weaponDamage)); // initial attack
                
                for (int i = GetFerocityProcs(); i > 0; i--) // All ferocity procs
                {
            
                    enemyStats_script.DealDamage(CalculateDamage(weaponDamage));
                    GameObject ferocityLine = Instantiate(ferocityLineObject, collision.transform.position, Quaternion.identity);

                    AudioSource.PlayClipAtPoint(feroAudioClip, collision.transform.position, 1); // plays ferocity proc audio
                    ferocityLine.transform.SetParent(enemyStats_script.gameObject.transform);
                    
                    
                    // Sets Ferocity Line to be a child so that it gets hidden when enemy gets killed, so it doesn't stick around
            

                }
            }
            
            Destroy(gameObject);
        }
    }

    IEnumerator DoFerocity(Collider2D collision)
    {
        
        yield return null;
    }

    IEnumerator RemoveObject()
    {
        yield return new WaitForSeconds(removeDelay);
        Destroy(gameObject);
    }


}
