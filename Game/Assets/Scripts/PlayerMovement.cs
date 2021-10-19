using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    // To make things easier for me, I think what I will do is also do abilities in this script for now
    // I can move things around later when I make abilities for other class
    // public PlayerAbilities script_PlayerAbilities = GameObject.FindObjectOfType(typeof(PlayerAbilities)) as PlayerAbilities; 

    //private PlayerStats script_PlayerStats;

    public GameObject gameManagerObject;

    // Entity Values
    public float playerSpeed;

    // Move Vector
    private static Vector2 moveVec;

    public Animator animator;

    // Fixed Update called at regular times (set amount), more consistent than Update(), do physics here
    void FixedUpdate()
    {
        
    }

    // Update is called once per frame (depends on current framerate), less consistent than FixedUpdate(), better for processing inputs
    void Update()
    {
        ProcessInputs();
        Move();
    }

    void ProcessInputs()
    {
        moveVec = Vector2.zero;

        if (Input.GetKey(KeyCode.W))
        {
            moveVec += Vector2.up;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveVec += Vector2.down;
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveVec += Vector2.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveVec += Vector2.right;
        }
        if (Input.GetKey(KeyCode.Space))
        {

            /*
            script_PlayerAbilities instPlayerAbilities = new script_PlayerAbilities(); 
            instPlayerAbilities.PreUseAbility();
            */
            UseFighterAbility();
        }
    }


    void Move()
    {
        moveVec = moveVec.normalized;
        transform.Translate(moveVec * playerSpeed * Time.deltaTime);

        if (moveVec.x != 0 )
        {
            moveVec.y = 0;
            AnimatorMovement(moveVec);
        }
        else if (moveVec.y != 0)
        {
            AnimatorMovement(moveVec);
        }
        else
        {
            // Layers are 0 based, so Idle is 0 and Walking is 1
            animator.SetLayerWeight(1, 0);
        }

        
    }

    private void AnimatorMovement(Vector2 moveVec)
    {
        animator.SetLayerWeight(1, 1);
        animator.SetFloat("XDir", moveVec.x);
        animator.SetFloat("YDir", moveVec.y);
    }


    // Start is called before the first frame update
    void Start()
    {
        // Get animator component from player
        animator = GetComponent<Animator>();

    }



    public float fighterDashSpeed;
    public float fighterDashCost; 

    private void UseFighterAbility()
    {
        //Debug.Log("Current Ability Pool: " + gameManagerObject.GetComponent<PlayerStats>().getCurrentAbilityPool());
        gameManagerObject.GetComponent<PlayerStats>().AbilityExpend(fighterDashCost);
        if (gameManagerObject.GetComponent<PlayerStats>().getCurrentAbilityPool() > 0)
        {
            transform.Translate(moveVec * fighterDashSpeed * Time.deltaTime);
            //Debug.Log("Current Ability Pool: " + gameManagerObject.GetComponent<PlayerStats>().getCurrentAbilityPool());
        }
        //moveVec = PlayerMovement.GetPlayerMoveVec();
        
    }






}






/*
 * rigidbody.velocity = new Vector2(moveVec.x * playerSpeed, moveVec.y * playerSpeed);
 * 
 * 
 * float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveVec = new Vector2(moveX, moveY).normalized;
 * 
 */
