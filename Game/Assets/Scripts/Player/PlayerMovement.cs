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
    private PlayerStats playerStats_script;
    public GameObject puffAnimation;
    // Entity Values
    public float playerSpeed;
    public float originalSpeed; // for storing the original speed value for slowness condition
    public float dodgeImmunityLength;
    public float dodgeCost;
    public float dodgeSpeed;

    // Move Vector
    private Vector2 animationVec;
    public Animator animator;

    public Rigidbody2D rb;
    private Vector2 moveDirection;
    private Vector2 dodgeDirection;

    // Fixed Update called at regular times (set amount), more consistent than Update(), do physics here
    void FixedUpdate()
    {
        
    }
    

    // Update is called once per frame (depends on current framerate), less consistent than FixedUpdate(), better for processing inputs
    void Update()
    {
        ProcessInputs();
        if (playerStats_script.GetEvadingStatus())
        {
            rb.velocity = dodgeDirection.normalized * dodgeSpeed;
        }
        else
        {
            Move();
        }
    }

    /*
    void OnCollisionEnter2D(Collision2D collision)
    {
        animationVec = Vector2.zero;
    }
    */

    void ProcessInputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        
        moveDirection = new Vector2(moveX, moveY).normalized; // To fix diagonal movement

        animationVec = Vector2.zero;
        if (Input.GetKey(KeyCode.W))
        {
            animationVec += Vector2.up;
        }
        if (Input.GetKey(KeyCode.S))
        {
            animationVec += Vector2.down;
        }
        if (Input.GetKey(KeyCode.A))
        {
            animationVec += Vector2.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            animationVec += Vector2.right;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            UseFighterAbility();
            // Instantiate(puffAnimation, gameObject.transform.position, Quaternion.identity);
            // Uncommenting the code makes a trail of animations, which could probably be used to deal damage
        }
        if (Input.GetKey(KeyCode.LeftShift) && playerStats_script.getCurrentAbilityPool() > dodgeCost && !playerStats_script.GetEvadingStatus())
        {
            StartCoroutine(Dodge());
        }
    }

    IEnumerator Timer(float time)
    {
        yield return new WaitForSeconds(time);
        // Do something here
        StartCoroutine(Timer(1f));
    }

    IEnumerator Dodge()
    {
        playerStats_script.AbilityExpend(dodgeCost);
        Instantiate(puffAnimation, gameObject.transform.position, Quaternion.identity);
        //transform.Translate(animationVec * (playerSpeed * 3) * Time.deltaTime);
        dodgeDirection = moveDirection;

        playerStats_script.FlipEvading(); // sets isEvading to true for [dodgeCooldown] length
        yield return new WaitForSeconds(dodgeImmunityLength);
        playerStats_script.FlipEvading(); // sets isEvading back to false
    }

    void Move()
    {
        rb.velocity = new Vector2(moveDirection.x * playerSpeed, moveDirection.y * playerSpeed);

        animationVec = animationVec.normalized;
        //transform.Translate(animationVec * playerSpeed * Time.deltaTime);
        if (animationVec.x != 0 )
        {
            animationVec.y = 0;
            AnimatorMovement(animationVec);
        }
        else if (animationVec.y != 0)
        {
            AnimatorMovement(animationVec);
        }
        else
        {
            // Layers are 0 based, so Idle is 0 and Walking is 1
            animator.SetLayerWeight(1, 0);
        }

        
    }

    private void AnimatorMovement(Vector2 animationVec)
    {
        animator.SetLayerWeight(1, 1);
        animator.SetFloat("XDir", animationVec.x);
        animator.SetFloat("YDir", animationVec.y);
    }


    // Start is called before the first frame update
    void Awake()
    {
        // Get animator component from player
        playerStats_script = GameObject.FindWithTag("GameController").GetComponent<PlayerStats>();
        animator = GetComponent<Animator>();
        originalSpeed = playerSpeed;
        //StartCoroutine(Timer(1f));
    }

    public void DoSlowness()
    {
        if (playerSpeed == originalSpeed)
        {
            playerSpeed /= 2;
        }
        else
        {
            Debug.Log("Slowness already applied, player speed not changed");
        }
    }
    public void DoImmobility()
    {
        if (playerSpeed == originalSpeed)
        {
            playerSpeed = 0;
        }
        else
        {
            Debug.Log("Immobility already applied, player speed not changed");
        }
    }
    public void ResetSpeed()
    {
        playerSpeed = originalSpeed;
    }

    

    //public float fighterDashSpeed;
    public float fighterDashCost; 

    private void UseFighterAbility()
    {
        Debug.Log("Disabled Ability. Will be switched out for something else.");
        /*
        //Debug.Log("Current Ability Pool: " + gameManagerObject.GetComponent<PlayerStats>().getCurrentAbilityPool());
        gameManagerObject.GetComponent<PlayerStats>().AbilityExpend(fighterDashCost);
        if (gameManagerObject.GetComponent<PlayerStats>().getCurrentAbilityPool() > 0)
        {
            transform.Translate(animationVec * (playerSpeed * 2) * Time.deltaTime);
            //Debug.Log("Current Ability Pool: " + gameManagerObject.GetComponent<PlayerStats>().getCurrentAbilityPool());
        }
        //animationVec = PlayerMovement.GetPlayeranimationVec();
        */
    }






}






/*
 * rigidbody.velocity = new Vector2(animationVec.x * playerSpeed, animationVec.y * playerSpeed);
 * 
 * 
 * float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        animationVec = new Vector2(moveX, moveY).normalized;
 * 
 */
