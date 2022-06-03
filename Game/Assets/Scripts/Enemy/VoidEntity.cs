using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidEntity : MonoBehaviour
{
    public GameObject coreGameObject;
    public Transform attackTarget; // The potential target (always player, but for multiplayer might be other players)
    public Transform self; // This is self-explanatory (badum-tss)
    public Animator anim;
    private SpriteRenderer sr;


    void Start()
    {
        attackTarget = GameObject.FindWithTag("Player").gameObject.transform;
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (transform.position.x >= attackTarget.position.x)
        {
            sr.flipX = false;
        }
        else if (transform.position.x <= attackTarget.position.x)
        {
            sr.flipX = true;
        }
    }
}
