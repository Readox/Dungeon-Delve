using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageIndicators : MonoBehaviour
{

    public void AnimationEventDestroy()
    {
        Destroy(gameObject);
    }

    public void StartDestroyTimer(float t) // takes in time and sends it to destroy function
    {
        StartCoroutine(DestroyAfterTime(t));
    }

    IEnumerator DestroyAfterTime(float t)
    {
        yield return new WaitForSeconds(t);
        Destroy(gameObject);
    }

}
