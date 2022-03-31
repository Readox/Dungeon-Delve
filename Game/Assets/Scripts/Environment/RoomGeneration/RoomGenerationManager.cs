using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class RoomGenerationManager : MonoBehaviour
{
    
    public Transform playerPos;
    public GameObject twentyByTwenty_Room;
    public AstarPath pathfinder_Script;

    private Vector3 currentRoomCenterPos; // is set to startingRoom.position to prevent starting room from getting shuffled around
    public Transform startingRoom;

    public List<GameObject> rooms = new List<GameObject>();

    [HideInInspector] public GameObject[,] roomMap = new GameObject[10,10];
    private int currentX;
    private int currentY;

    void Awake()
    {
        currentRoomCenterPos = startingRoom.position;
        InitializeRoomMap();
    }

    private void InitializeRoomMap()
    {
        for (int i = 0; i < roomMap.GetLength(0); i++)
        {
            for (int j = 0; j < roomMap.GetLength(1); j++)
            {
                roomMap[i, j] = null;
            }
        }
        roomMap[5,5] = startingRoom.gameObject;
        currentX = 5;
        currentY = 5;
    }

    public void DoRoomChange(int currentX, int currentY, string doorDirection)
    {
        GetMapChangeFromDirection(doorDirection);
        if (roomMap[currentX, currentY] != null)
        {
            HandleRoomChange(roomMap[currentX, currentY]);
        }
        else
        {
            roomMap[currentX, currentY] = CreateNewRoom(doorDirection);
            HandleRoomChange(roomMap[currentX, currentY]);
        }
        
        //TODO
        //PrintRoomMap(); // Add method for debugging
    }

    private GameObject CreateNewRoom(string doorDirection)
    {
        string connectedDoorOrientation = GetConnectedDoorDirection(doorDirection);
        Vector3 newPos = GetNewRoomPosition(doorDirection); // Must put in this value or else it will flip stuff around and things will get GOOFY fast
        GameObject newRoom = Instantiate(twentyByTwenty_Room, newPos, Quaternion.identity);
        newRoom.GetComponent<RoomManager>().roomMapX = currentX;
        newRoom.GetComponent<RoomManager>().roomMapY = currentY;
        // rooms.Add(newRoom);

        return newRoom;
    }

    private void HandleRoomChange(GameObject destination) // For generating a new room, returns the connected door
    {
        MovePathfindingGraph(destination.transform.position);
        currentRoomCenterPos = destination.transform.position;
        playerPos.position = currentRoomCenterPos;
    }


    private void GetMapChangeFromDirection(string doorDirection) // Changes the current map values
    {
        if (doorDirection.Equals("North"))
        {
            currentY += 1;
        }
        else if (doorDirection.Equals("South"))
        {
            currentY -= 1;
        }
        else if (doorDirection.Equals("West"))
        {
            currentX -= 1;
        }
        else // "East"
        {
            currentX += 1;
        }
    }

    private Vector3 GetNewRoomPosition(string doorDirection) // I may have to increase the dimensions of stuff if some rooms are really weirdly shaped intentionally
    {
        if (doorDirection.Equals("North"))
        {
            return new Vector3(currentRoomCenterPos.x, currentRoomCenterPos.y + 25, 0);
        }
        else if (doorDirection.Equals("South"))
        {
            return new Vector3(currentRoomCenterPos.x, currentRoomCenterPos.y - 25, 0);
        }
        else if (doorDirection.Equals("West"))
        {
            return new Vector3(currentRoomCenterPos.x - 25, currentRoomCenterPos.y, 0);
        }
        else // "East"
        {
            return new Vector3(currentRoomCenterPos.x + 25, currentRoomCenterPos.y, 0);
        }
    }

    private string GetConnectedDoorDirection(string doorDirection)
    {
        if (doorDirection.Equals("North"))
        {
            return "South";
        }
        else if (doorDirection.Equals("South"))
        {
            return "North";
        }
        else if (doorDirection.Equals("West"))
        {
            return "East";
        }
        else // "East"
        {
            return "West";
        }
    }


    private void MovePathfindingGraph(Vector3 roomCenter)
    {
        var pathGraph = AstarPath.active.data.gridGraph;
        pathGraph.center = roomCenter;
        AstarPath.active.Scan();
        //pathGraph.SetDimensions() // Changes the graph's dimenstions if the room is differently shaped
    }

    

    /* Old

    public void HandleRoomChange()
    {
        Vector3 newPos = new Vector3(currentRoomCenterPos.x, currentRoomCenterPos.y + 20, 0);
        GameObject newRoom = Instantiate(twentyByTwenty_Room, newPos, Quaternion.identity);
        MovePathfindingGraph(newRoom.transform.position);
        currentRoomCenterPos = newRoom.transform.position;
        playerPos.position = currentRoomCenterPos;
    }

    public GameObject HandleRoomChange(string doorDirection) // For generating a new room, returns the connected door
    {
        string connectedDoorOrientation = GetConnectedDoorDirection(doorDirection);
        Vector3 newPos = GetNewRoomPosition(doorDirection); // Must put in this value or else it will flip stuff around and things will get GOOFY fast
        GameObject newRoom = Instantiate(twentyByTwenty_Room, newPos, Quaternion.identity);
        rooms.Add(newRoom);
        MovePathfindingGraph(newRoom.transform.position);
        currentRoomCenterPos = newRoom.transform.position;
        playerPos.position = currentRoomCenterPos;

        return newRoom.transform.Find(connectedDoorOrientation).gameObject;
    }

    public void HandleRoomChange(GameObject connectedDoor) // For going to a previously created room
    {
        foreach (GameObject g in rooms)
        {
            if (g.transform.Find(connectedDoor) != null)
            {
                MovePathfindingGraph(g.transform.position);
                currentRoomCenterPos = g.transform.position;
                playerPos.position = currentRoomCenterPos;
            }
        }
    }

    */
    



}
