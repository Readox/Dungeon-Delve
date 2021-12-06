using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
//[RequireComponent(typeof(CircleCollider2D))] // doesnt work with just having the collider in the child EnemyView
[RequireComponent(typeof(Animator))]

public class Wander : MonoBehaviour
{

    public float pursuitSpeed;
    public float wanderSpeed;
    float currentSpeed;

    public float directionChangeInterval;

    public bool followPlayer;

    Coroutine moveCoroutine;

    Rigidbody2D rb;
    Animator animator;

    Transform targetTransform = null;
    Vector3 endPosition;
    float currentAngle = 0;

    public float damage;

    //The problem with the enemy pathfinding might be the end position or target position not being set properly, so they just pathfind to the origin

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        currentSpeed = wanderSpeed;
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(WanderRoutine());
    }

    public IEnumerator WanderRoutine()
    {
        while(true)
        {
            ChooseNewEndpoint();

            if (moveCoroutine != null)
            {
                Debug.Log("Stopped Wander Coroutine");
                StopCoroutine(moveCoroutine);
            }

            moveCoroutine = StartCoroutine(Move(rb, currentSpeed));

            yield return new WaitForSeconds(directionChangeInterval);
        }
    }

    void ChooseNewEndpoint() // not including an access modifier makes the function private by default
    {
        currentAngle += Random.Range(0,360);
        currentAngle = Mathf.Repeat(currentAngle, 360);
        this.endPosition += Vector3FromAngle(currentAngle);
    }

    Vector3 Vector3FromAngle(float inputDeg)
    {
        float radianVal = inputDeg * Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(radianVal), Mathf.Sin(radianVal), 0);
    }

    public float GetDamage()
    {
        return damage;
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

            yield return new WaitForFixedUpdate();
        }

        animator.SetBool("isWalking", false);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && followPlayer)
        {
            currentSpeed = pursuitSpeed;
            targetTransform = collision.gameObject.transform;

            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }

            moveCoroutine = StartCoroutine(Move(rb, currentSpeed));
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            animator.SetBool("isWalking", false);
            currentSpeed = wanderSpeed;

            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
                //StartCoroutine(WanderRoutine());
            }

            targetTransform = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(rb.position, endPosition, Color.red);
    }
}
