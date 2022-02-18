using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectAnimation : MonoBehaviour
{
    private Transform target;
    private Animator anim;
    public bool showConstantly;
    public float repeatInterval;
    private Coroutine c;

    void Awake()
    {
        target = GameObject.FindWithTag("Player").GetComponent<Transform>();
        anim = GetComponent<Animator>();
        if (showConstantly)
        {
            c = StartCoroutine(ShowEffectConstantly(repeatInterval));
        }
        else
        {
            anim.SetBool("Trigger", true);
        }
    }

    IEnumerator ShowEffectConstantly(float repeatInterval)
    {
        if (!anim.GetBool("Trigger"))
        {
            anim.SetBool("Trigger", true);
        }
        
        yield return new WaitForSeconds(repeatInterval);

        c = StartCoroutine(ShowEffectConstantly(repeatInterval));
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = target.position;
        /*
        if (!anim.GetBool("Trigger") && showConstantly)
        {
            anim.SetBool("Trigger", true);
        }
        */
    }
}
