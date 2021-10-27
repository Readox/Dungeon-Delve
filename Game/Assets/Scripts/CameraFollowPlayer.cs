using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{

    private Transform playerLoc;
    public float smoothing;
    public Vector3 offset;


    public void Awake()
    {
        playerLoc = GameObject.Find("Player").transform;
    }

    void FixedUpdate()
    {
        Vector3 finalPos = Vector3.Lerp(transform.position, playerLoc.transform.position + offset, smoothing);
        transform.position = finalPos;
    }
}
