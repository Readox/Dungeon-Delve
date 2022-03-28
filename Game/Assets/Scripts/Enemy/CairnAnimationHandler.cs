using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class CairnAnimationHandler : MonoBehaviour
{
    public Transform attackTarget; // The potential target (always player, but for multiplayer might be other players)
    public Transform self; // This is self-explanatory (badum-tss)
    public Animator anim;
    private SpriteRenderer spriteRenderer;

    public float targetDistance;  // Distance before it "sees" the attackTarget
    float dist; // used to store calculation of distance between attackTarget and self
    bool animBoolSet = false;

   

    // Start is called before the first frame update
    void Start()
    {
        attackTarget = GameObject.FindWithTag("Player").gameObject.transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    // Update is called once per frame
    void Update() 
    {
        dist = Vector3.Distance(attackTarget.position, self.position);
        
        if (dist <= targetDistance)
        {
            anim.SetBool("PlayerTargeted", true);
            animBoolSet = true;
        }


        if (transform.position.x >= attackTarget.position.x && animBoolSet)
        {
            spriteRenderer.flipX = false;
        }
        else if (transform.position.x <= attackTarget.position.x && animBoolSet)
        {
            spriteRenderer.flipX = true;
        }
    }


    // I scrapped this idea because the players can use it to easily cheese enemies
    //public bool persistentEnemy; 
        // If true, will activate AIDestinationSetter to make it follow player until it dies
        // If false, once player leaves view range, it will stop and go about its' business
}
