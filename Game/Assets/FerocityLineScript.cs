using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FerocityLineScript : MonoBehaviour
{

    public float removeDelay;
    
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(RemoveObject());
    }
    
    
     IEnumerator RemoveObject()
    {
        yield return new WaitForSeconds(removeDelay);
        Destroy(gameObject);
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
