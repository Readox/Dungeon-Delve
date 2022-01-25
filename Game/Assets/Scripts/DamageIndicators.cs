using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageIndicators : MonoBehaviour
{
    public float removeDelay;

    void Awake()
    {
        StartCoroutine(DestroyAfterTime());
    }

    IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(removeDelay);
        Destroy(gameObject);
    }

}
