using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerAbilities : MonoBehaviour
{

    //public PlayerStats script_PlayerStats;
    //public PlayerMovement script_PlayerMovement;

    public string playerClass;

    private Vector2 moveVec;

    public float fighterDashSpeed;


    private void UseFighterAbility()
    {
        //moveVec = PlayerMovement.GetPlayerMoveVec();
        transform.Translate(moveVec * fighterDashSpeed * Time.deltaTime);
    }

    private void UseMageAbility()
    {

    }

    private void UseArcherAbility()
    {
        
    }

    private void UseTankAbility()
    {

    }



    public void PreUseAbility()
    {
        if (playerClass.Equals("fighter"))
        {
            UseFighterAbility();
        }
        else if (playerClass.Equals("mage"))
        {
            UseMageAbility();
        }
        else if (playerClass.Equals("archer"))
        {
            UseArcherAbility();
        }
        else if (playerClass.Equals("tank"))
        {
            UseTankAbility();
        }
        else
        {
            Debug.Log("Select a class please!");
        }
    }




    // Start is called before the first frame update
    void Start()
    {
        //script_PlayerStats = GameObject.FindObjectOfType(typeof(PlayerStats)) as PlayerStats;
        //script_PlayerMovement = GameObject.FindObjectOfType(typeof(PlayerMovement)) as PlayerMovement;

        //playerClass = PlayerStats.GetClass();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
