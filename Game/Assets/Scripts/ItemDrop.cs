using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    public float baseSpeed;

    public float attractDist;

    int coinTier = 1;
    
    GameObject gameManager;

    Transform playerLoc;
    
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindWithTag("GameController");
        playerLoc = GameObject.FindWithTag("Player").gameObject.transform;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if (coinTier > 0)
            {
                gameManager.GetComponent<PlayerStats>().AddUpgradeCurrency(coinTier);
                Debug.Log("Player picked up coin");
                Destroy(gameObject);
            }
            else
            {
                // Add item to inventory here;
            }
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position, playerLoc.position) < attractDist)
        {
            float step =  baseSpeed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, playerLoc.position, step);
        }
        
    }
}
