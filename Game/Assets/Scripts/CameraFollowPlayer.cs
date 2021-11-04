using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{

    private Transform playerLoc;
    public float smoothing;
    public Vector3 offset;

    void Start()
    {
        GetPlayerLoc();
    }
    public void Awake()
    {
        
    }

    public void GetPlayerLoc()
    {
        playerLoc = GameObject.FindWithTag("Player").transform;
    }

    void FixedUpdate()
    {
        if (playerLoc != null)
        {
            Vector3 finalPos = Vector3.Lerp(transform.position, playerLoc.transform.position + offset, smoothing);
            transform.position = finalPos;
        }
        
    }
}
