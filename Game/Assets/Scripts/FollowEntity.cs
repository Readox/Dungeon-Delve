using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowEntity : MonoBehaviour
{

    public Transform target;
    public float xOffset;
    public float yOffset;
    public float distance;
    public bool testRot;

    // Update is called once per frame
    void Update()
    {
        transform.position = target.position + new Vector3(xOffset, yOffset, 0);

        /*
        if (testRot) // Rotates on player movement. If looking for orbit, check MeleeAttacks() script
        {
            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");

            float rads = Mathf.Atan2(y, x);
            float degrees = rads * Mathf.Rad2Deg;

            transform.localPosition = new Vector3(Mathf.Cos(rads) * distance, Mathf.Sin(rads) * distance, 0);

            transform.localEulerAngles = new Vector3(0, 0, degrees - 90);
        }
        else
        {
            transform.position = target.position + new Vector3(xOffset, yOffset, 0);
        }
        */
    }
}
