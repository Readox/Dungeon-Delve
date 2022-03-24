using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectAnimation : MonoBehaviour
{
    private Transform target;
    private Animator anim;
    public bool showConstantly;
    public bool followPlayer;
    public float destroyTime;
    public float repeatInterval;
    public float activateAfterTime;
    public float xOffset;
    public float yOffset;
    private Coroutine c;

    void Awake()
    {
        target = GameObject.FindWithTag("Player").GetComponent<Transform>();
        anim = GetComponent<Animator>();
        if (destroyTime > 0)
        {
            anim.SetBool("Trigger", true);
            StartCoroutine(DestroyAfterTime());
        }
        else if (activateAfterTime > 0)
        {
            StartCoroutine(ActivateAfterTime(activateAfterTime));
        }
        if (showConstantly)
        {
            c = StartCoroutine(ShowEffectConstantly(repeatInterval));
        }
        /* // I don't know whether this is necessary to any of my effects, and I can't be bothered to test it
        else
        {
            anim.SetBool("Trigger", true);
        }
        */
    }

    IEnumerator ActivateAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        anim.SetBool("Trigger", true);
    }

    IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(gameObject);
    }

    IEnumerator ShowEffectConstantly(float repeatInterval)
    {
        if (!anim.GetBool("Trigger"))
        {
            anim.SetBool("Trigger", true);
            if (gameObject.name.Substring(0,5).Equals("Aegis"))
            {
                anim.SetBool("AegisEffect", true);
            }
        }
        
        yield return new WaitForSeconds(repeatInterval);

        c = StartCoroutine(ShowEffectConstantly(repeatInterval));
    }

    // Update is called once per frame
    void Update()
    {
        if (followPlayer)
        {
            transform.position = target.position;
            //transform.position.y += 1;
            Vector3 tempVector = transform.position; // For properly centering stuff
            tempVector.y += yOffset;
            tempVector.x += xOffset;
            transform.position = tempVector;
        }
        /*
        if (!anim.GetBool("Trigger") && showConstantly)
        {
            anim.SetBool("Trigger", true);
        }
        */
    }
}
