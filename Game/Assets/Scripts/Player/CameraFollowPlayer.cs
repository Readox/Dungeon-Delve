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
        Vector3 playerPos = playerLoc.transform.position;
        Vector3 startPos = new Vector3(playerPos.x, playerPos.y, playerPos.z);
        transform.position = startPos;
    }
    public void Awake()
    {
        
    }

    public Transform GetPlayerLoc()
    {
        playerLoc = GameObject.FindWithTag("Player").transform;
        return playerLoc;
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
