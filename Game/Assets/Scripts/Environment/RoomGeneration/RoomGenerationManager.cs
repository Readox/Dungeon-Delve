using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class RoomGenerationManager : MonoBehaviour
{
    
    public Transform playerPos;
    public GameObject twentyByTwenty_Room;
    public AstarPath pathfinder_Script;

    public Transform currentRoomCenterPos; // Should be set to starting room first

    void Awake()
    {

    }

    public void HandleRoomChange()
    {
        Vector3 newPos = new Vector3(currentRoomCenterPos.position.x, currentRoomCenterPos.position.y + 20, 0);
        GameObject newRoom = Instantiate(twentyByTwenty_Room, newPos, Quaternion.identity);
        currentRoomCenterPos.position = newRoom.transform.position;
        playerPos.position = currentRoomCenterPos.position;
    }



    

}
