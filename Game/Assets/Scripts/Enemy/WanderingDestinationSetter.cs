using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class WanderingDestinationSetter : MonoBehaviour
{
    IAstarAI ai; // for seeing if AI is doing something
    AIPath aiPath; //For setting speed
    public Transform attackTarget; // The potential target (always player, but for multiplayer might be other players)
    public Transform self; // This is self-explanatory

    public float searchRadius; // Searches for point in radius to wander to
    public float targetDistance;  // Distance before it "sees" the attackTarget
    public float wanderSpeed; // speed during wander
    public float attackSpeed; // speed during attack
    float dist; // used to store calculation of distance between attackTarget and self

   

    // Start is called before the first frame update
    void Start()
    {
        ai = GetComponent<IAstarAI>();
        aiPath = GetComponent<AIPath>();
    }

    Vector3 RandomPoint()
    {
        var point = Random.insideUnitSphere * searchRadius;

        point.z = 0;
        point += ai.position;
        return point;
    }

    // Update is called once per frame
    void Update() 
    {
        dist = Vector3.Distance(attackTarget.position, self.position);
        
        if (dist <= targetDistance)
        {
            aiPath.maxSpeed = attackSpeed;
            AIDestinationSetter destSet = GetComponent<AIDestinationSetter>(); // temp variable
            destSet.enabled = true; // activates AIDestinationSetter
            destSet.target = attackTarget;
            GetComponent<WanderingDestinationSetter>().enabled = false; //Deactivates this script, enemy will follow player until one dies
        }
        else if (!ai.pathPending && (ai.reachedEndOfPath || !ai.hasPath)) // From https://arongranberg.com/astar/documentation/dev_4_3_8_84e2f938/old/wander.php
        {
            aiPath.maxSpeed = 0; // if this happens, enemy is idle, this is here for the purpose of 
            ai.destination = RandomPoint();
            ai.SearchPath();
        }
        else
        {
            aiPath.maxSpeed = wanderSpeed;
        }
    }


    // I scrapped this idea because the players can use it to easily cheese enemies
    //public bool persistentEnemy; 
        // If true, will activate AIDestinationSetter to make it follow player until it dies
        // If false, once player leaves view range, it will stop and go about its' business
}
