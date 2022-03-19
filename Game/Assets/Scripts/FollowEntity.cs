using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowEntity : MonoBehaviour
{

    public Transform target;
    public float xOffset;
    public float yOffset;

    // Update is called once per frame
    void Update()
    {
        transform.position = target.position + new Vector3(xOffset, yOffset, 0);
    }
}
