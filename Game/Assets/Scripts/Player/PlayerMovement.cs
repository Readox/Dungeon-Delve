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
    private RoomGenerationManager rmg_Script;
    private PlayerStats playerStats_script;
    public GameObject puffAnimation;
    // Entity Values
    public float playerSpeed;
    public float originalSpeed; // for storing the original speed value for slowness condition
    private bool frozen; // For freezing the animation 
    public float dodgeImmunityLength;
    public float dodgeCost;
    public float dodgeSpeed;
    private bool dodgeAnimationLock = false;

    private Collider2D doorCol;
    private bool canInteract = true; // is true when the player CAN interact with things
    public GameObject UIInteractTextObject;
    // Move Vector
    private Vector2 animationVec;
    public Animator animator;

    public Rigidbody2D rb;
    private Vector3 moveDirection;
    private Vector3 dodgeDirection;


    // Start is called before the first frame update
    void Awake()
    {
        // Get animator component from player
        playerStats_script = GameObject.FindWithTag("GameController").GetComponent<PlayerStats>();
        animator = GetComponent<Animator>();
        originalSpeed = playerSpeed;
        //StartCoroutine(Timer(1f));
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Doorway"))
        {
            doorCol = col;
            UIInteractTextObject.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Doorway"))
        {
            doorCol = null;
            UIInteractTextObject.SetActive(false);
        }
    }

    // Fixed Update called at regular times (set amount), more consistent than Update(), do physics here
    void FixedUpdate()
    {
        
    }
    

    // Update is called once per frame (depends on current framerate), less consistent than FixedUpdate(), better for processing inputs
    void Update()
    {
        ProcessInputs();
        if (dodgeAnimationLock)
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
        
        moveDirection = new Vector3(moveX, moveY, 0).normalized; // To fix diagonal movement

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
            // Uncommenting the code makes a trail of animations, which could probably be used to deal damage as a fire trail or something
        }
        if (Input.GetKey(KeyCode.LeftShift) && playerStats_script.getCurrentEndurancePool() >= dodgeCost && !playerStats_script.GetEvadingStatus())
        {
            StartCoroutine(Dodge());            
        }
        if (Input.GetKey(KeyCode.F) && doorCol != null && canInteract)
        {
            doorCol.gameObject.transform.parent.parent.gameObject.GetComponent<RoomManager>().IsDoorwayAccessible(doorCol.gameObject);
            StartCoroutine(InteractionTimer(1f));
        }
    }

    IEnumerator InteractionTimer(float time)
    {
        canInteract = false;
        yield return new WaitForSeconds(time);
        canInteract = true;
    }

    IEnumerator Timer(float time)
    {
        yield return new WaitForSeconds(time);
        // Do something here
        StartCoroutine(Timer(1f));
    }

    IEnumerator DodgeAnimationLock(float time)
    {
        dodgeAnimationLock = true;
        yield return new WaitForSeconds(time);
        dodgeAnimationLock = false;
    }

    IEnumerator Dodge()
    {
        if (moveDirection == Vector3.zero) // If player is not currently moving
        {
            dodgeDirection = new Vector3(0, -1, 0);
        }
        else
        {
            dodgeDirection = moveDirection.normalized;
        }
        StartCoroutine(DodgeAnimationLock(dodgeImmunityLength / 2)); // This is here to lock the player into a dodge for a different time length than invulnerability
        playerStats_script.EnduranceExpend(dodgeCost);
        Instantiate(puffAnimation, gameObject.transform.position, Quaternion.identity);
        //transform.Translate(animationVec * (playerSpeed * 3) * Time.deltaTime);

        playerStats_script.FlipEvading(); // sets isEvading to true for [dodgeCooldown] length
        yield return new WaitForSeconds(dodgeImmunityLength);
        playerStats_script.FlipEvading(); // sets isEvading back to false
    }

    private void StartDodgeRoll()
    {
        float tempSpeed = dodgeSpeed;
        transform.position += dodgeDirection * dodgeSpeed * Time.deltaTime;
        dodgeSpeed -= dodgeSpeed * 0.1f * Time.deltaTime;
        if (dodgeSpeed < playerSpeed)
        {
            playerStats_script.FlipEvading();
            dodgeSpeed = tempSpeed;
        }
    }

    void Move()
    {
        rb.velocity = new Vector3(moveDirection.x * playerSpeed, moveDirection.y * playerSpeed, 0);

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

    public void FreezePlayerForTime(float time)
    {
        StartCoroutine(FreezeForTime(time));
    }

    IEnumerator FreezeForTime(float time)
    {
        frozen = true;
        float tempSpeed = playerSpeed;
        playerSpeed = 0;
        yield return new WaitForSeconds(time);
        frozen = false;
        playerSpeed = tempSpeed;
    }

    private void AnimatorMovement(Vector2 animationVec)
    {
        if (!frozen)
        {
            animator.SetLayerWeight(1, 1);
            animator.SetFloat("XDir", animationVec.x);
            animator.SetFloat("YDir", animationVec.y);
        }
        else
        {
            animator.SetLayerWeight(1, 0); // Set to idle animation
        }
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
