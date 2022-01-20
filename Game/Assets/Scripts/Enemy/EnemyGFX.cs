using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyGFX : MonoBehaviour
{
    public AIPath aiPath;
    Animator anim;
    WanderingDestinationSetter wanderScript;

    void Awake()
    {
        anim = GetComponent<Animator>();
        wanderScript = gameObject.transform.parent.gameObject.GetComponent<WanderingDestinationSetter>();
    }

    // Update is called once per frame
    void Update()
    {
        // These are for setting orientation
        if (aiPath.desiredVelocity.x >= 0.01f)
        {
            transform.localScale = new Vector3(-1f,1f,1f);
        }
        else if (aiPath.desiredVelocity.x <= -0.01f)
        {
            transform.localScale = new Vector3(1f,1f,1f);
        }
    }

    void FixedUpdate()
    {
        // These are for setting animation states
        if (aiPath.maxSpeed == wanderScript.wanderSpeed)
        {
            anim.SetBool("Walk", true);
        }
    }
}
