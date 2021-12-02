using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
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

    // These are for debugging:
    CircleCollider2D cc;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        currentSpeed = wanderSpeed;
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(WanderRoutine());
        cc = GetComponent<CircleCollider2D>();
    }

    public IEnumerator WanderRoutine()
    {
        while(true)
        {
            ChooseNewEndpoint();

            if (moveCoroutine != null)
            {
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
        endPosition += Vector3FromAngle(currentAngle);
    }

    Vector3 Vector3FromAngle(float inputDeg)
    {
        float radianVal = inputDeg * Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(radianVal), Mathf.Sin(radianVal), 0);
    }

    public IEnumerator Move(Rigidbody2D rb1, float speed)
    {
        float distRemain = (transform.position - endPosition).sqrMagnitude;

        while (distRemain > float.Epsilon)
        {
            if(targetTransform != null)
            {
                endPosition = targetTransform.position;
            }

            if (rb1 != null)
            {
                animator.SetBool("isWalking", true);

                Vector3 newPos = Vector3.MoveTowards(rb1.position, endPosition, speed * Time.deltaTime);

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
            Debug.Log("Player entered View Range");
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
            }

            targetTransform = null;
        }
    }

    // For the debugging above:
    void OnDrawGizmos()
    {
        if (cc != null)
        {
            Gizmos.DrawWireSphere(transform.position, cc.radius);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(rb.position, endPosition, Color.red);
    }
}
