using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
//[RequireComponent(typeof(CircleCollider2D))] // doesnt work with just having the collider in the child EnemyView
[RequireComponent(typeof(Animator))]
public class Patrol : MonoBehaviour
{
    public float wanderSpeed;
    public float patrolRadius;
    float currentSpeed;

    public float directionChangeInterval; 
    // For patrolling small area, this should be small; if big area, this should be big

    Coroutine moveCoroutine;
    Coroutine patrolCoroutine;

    Rigidbody2D rb;
    Animator animator;

    Transform targetTransform = null;
    Vector3 endPosition;
    Vector3 startPosition;
    float currentAngle = 0;
    
    TestEnemyRangedAttack attack_script;
    //The problem with the enemy pathfinding might be the end position or target position not being set properly, so they just pathfind to the origin

    // Start is called before the first frame update
    void Start()
    {
        endPosition = transform.position;
        attack_script = GetComponent<TestEnemyRangedAttack>();
        startPosition = transform.position;
        animator = GetComponent<Animator>();
        currentSpeed = wanderSpeed;
        rb = GetComponent<Rigidbody2D>();
        patrolCoroutine = StartCoroutine(PatrolRoutine());
    }

    public IEnumerator PatrolRoutine()
    {
        while(true)
        {
            ChooseNewEndpoint();

            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }
            
            yield return new WaitForSeconds(1); //Thank goodness this doesnt cause a crash, waits between Move operations

            moveCoroutine = StartCoroutine(Move(rb, currentSpeed)); 

            yield return new WaitForSeconds(directionChangeInterval); //time before move coroutine ended
        }
    }

    void ChooseNewEndpoint() // not including an access modifier makes the function private by default
    {
        currentAngle += Random.Range(0,360);
        currentAngle = Mathf.Repeat(currentAngle, 360);
        //Debug.Log("Current Angle: " + currentAngle);
        this.endPosition += Vector3FromAngle(currentAngle);
        //Debug.Log(endPosition.ToString());
    }

    Vector3 Vector3FromAngle(float inputDeg)
    {
        float radianVal = inputDeg * Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(radianVal)*5, Mathf.Sin(radianVal)*5, 0);
        // I still don't know why I have to multiply by a certain amount to get the enemy to actually pathfind in a direction
        // I will probably figure something else out later
        // This can just be a placeholder script I guess?
    }

    public IEnumerator Move(Rigidbody2D rb, float speed)
    {
        float distRemain = (transform.position - endPosition).sqrMagnitude;

        while (distRemain > float.Epsilon)
        {
            if(targetTransform != null)
            {
                endPosition = targetTransform.position;
            }

            if (rb != null)
            {
                animator.SetBool("isWalking", true);

                Vector3 newPos = Vector3.MoveTowards(rb.position, endPosition, speed * Time.deltaTime);

                this.rb.MovePosition(newPos);

                distRemain = (transform.position - endPosition).sqrMagnitude;
            }


            // Credit to CarterG81 on https://answers.unity.com/questions/1309521/how-to-keep-an-object-within-a-circlesphere-radius.html
            // for this elegant solution to my problem of pathfinding within a circle
            float distance = Vector3.Distance(endPosition, startPosition);
            if (distance > patrolRadius)
            {
            Vector3 fromOriginToObject = endPosition - startPosition;
            fromOriginToObject *= patrolRadius / distance;
            endPosition = startPosition + fromOriginToObject;
            }
            //

            yield return new WaitForFixedUpdate();
        }

        animator.SetBool("isWalking", false);
    }




    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
                StopCoroutine(patrolCoroutine);
                attack_script.StartRangedAttack();

            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
                patrolCoroutine = StartCoroutine(PatrolRoutine());
                attack_script.StopRangedAttack();
            }
        }
    }

    void Update()
    {
        //Debug.DrawLine(rb.position, endPosition, Color.red);
    }


}
