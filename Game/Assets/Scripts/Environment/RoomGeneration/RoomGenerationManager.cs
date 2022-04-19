using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class RoomGenerationManager : MonoBehaviour
{
    // This script handles creation of rooms, while the RoomManager handles creation of stuff inside that room
    public bool disableMobSpawning;
    public Transform playerPos;
    public GameObject twentyByTwenty_Room;
    public AstarPath pathfinder_Script;

    private Vector3 currentRoomCenterPos; // is set to startingRoom.position to prevent starting room from getting shuffled around
    public Transform startingRoom;
    public List<GameObject> enemyObjects;

    public int maximumMapX;
    public int maximumMapY;
    public int startingRoomX;
    public int startingRoomY;
    [HideInInspector] public GameObject[,] roomMap; // this seems to not be 0 based?
    private int currentX;
    private int currentY;


    // Boss spawners are not in enemyObjects list because they are not spawned regularly
    public GameObject cairnTheIndomitable;
    public GameObject reaper;

    void Start()
    {
        DisableOutOfBoundsDoors(startingRoom.gameObject);
    }

    void Awake()
    {
        currentRoomCenterPos = startingRoom.position;
        InitializeRoomMap();

        //PrintRoomMap();
    }

    private void InitializeRoomMap() // Initializes the RoomMap with null values and the starting room
    {
        roomMap = new GameObject[maximumMapX, maximumMapY];
        for (int i = 0; i < roomMap.GetLength(0); i++)
        {
            for (int j = 0; j < roomMap.GetLength(1); j++)
            {
                roomMap[i, j] = null;
            }
        }
        roomMap[startingRoomX, startingRoomY] = startingRoom.gameObject;
        currentX = startingRoomX;
        currentY = startingRoomY;
        startingRoom.GetComponent<RoomManager>().roomMapX = startingRoomX;
        startingRoom.GetComponent<RoomManager>().roomMapY = startingRoomY;
        DisableOutOfBoundsDoors(startingRoom.gameObject);
    }

    // This function is called by RoomManagers when a room is exited with a door, and finds whether the room is occupied or not
    // If the room is occupied, then the player is moved into the room with HandleRoomChange()
    // If the room is not occupied, then a new room is created, and the player is moved into it
    public void DoRoomChange(int roomX, int roomY, string doorDirection) // Note to self, do NOT name the variables the same thing as the ones in the script bc/ it will prioritize the inputs
    {
        //gameObject.GetComponent<PlayerStats>().ResetHealthPotionAmount(); // I have this here in case I want to reset health potions on room change instead of on dungeon start
        GetMapChangeFromDirection(doorDirection);
        if (roomMap[currentX, currentY] != null)
        {
            HandleRoomChange(roomMap[currentX, currentY], doorDirection);
        }
        else
        {
            roomMap[currentX, currentY] = CreateNewRoom(doorDirection);
            HandleRoomChange(roomMap[currentX, currentY], doorDirection);
        }
        
        //PrintRoomMap(); // Method for debugging
    }

    // This method creates a new room based on the direction of the door entered by the player
    // It sets all necessary values after instantiating it and disables any doorways that should not be accessible
    private GameObject CreateNewRoom(string doorDirection)
    {
        string connectedDoorOrientation = GetConnectedDoorDirection(doorDirection);
        Vector3 newPos = GetNewRoomPosition(doorDirection); // Must put in this value or else it will flip stuff around and things will get GOOFY fast
        GameObject newRoom = Instantiate(twentyByTwenty_Room, newPos, Quaternion.identity);
        newRoom.GetComponent<RoomManager>().roomMapX = currentX;
        newRoom.GetComponent<RoomManager>().roomMapY = currentY;
        DisableOutOfBoundsDoors(newRoom);
        if (!disableMobSpawning)
        {
            newRoom.GetComponent<RoomManager>().CreateRoomContent(Random.Range(1, 5)); // Puts in pseudo-random number from 1 - 5
        }
        // rooms.Add(newRoom);

        return newRoom;
    }

    // This moves the player into a new room, though it currently moves them to the center
    // Takes in the destination room GameObject and the direction of the door that was entered from
    private void HandleRoomChange(GameObject destination, string doorDirection) // For generating a new room, returns the connected door
    {
        MovePathfindingGraph(destination.transform.position);
        currentRoomCenterPos = destination.transform.position;
        playerPos.position = destination.transform.Find(GetConnectedDoorDirection(doorDirection)).Find("Doorway").Find("EntryPos").position;
    }



    public GameObject GenerateRandomSpawner(float x, float y, int internalDifficultyLevel)
    {
        GameObject temp = Instantiate(enemyObjects[Random.Range(0, enemyObjects.Count)], new Vector3(x + Random.Range(-8f, 8f), y + Random.Range(-8f, 8f), 0), Quaternion.identity);
        temp.SetActive(false);
        SpawnPoint sp = temp.GetComponent<SpawnPoint>();
        sp.repeatInterval = Random.Range(1, 9);
        sp.maxEnemyCount = Random.Range(1, 3);

        return temp;
    }

    public GameObject GenerateBossSpawner(float x, float y, bool floorBossRoom)
    {
        if (floorBossRoom)
        {
            GameObject temp = Instantiate(cairnTheIndomitable, new Vector3(x, y, 0), Quaternion.identity);
            temp.SetActive(false);
            return temp;
        }
        else
        {
            GameObject temp = Instantiate(reaper, new Vector3(x + Random.Range(-3f, 3f), y + Random.Range(-3f, 3f), 0), Quaternion.identity);
            temp.SetActive(false);
            return temp;
        }
    }




    // This method is key to making sure that the RoomMap is not exceeded, and disables any doors that do not lead anywhere
    // There might be a faster way to do this, but whatever
    private void DisableOutOfBoundsDoors(GameObject room)
    {
        if (room.GetComponent<RoomManager>().roomMapX == maximumMapX - 1)
        {
            room.transform.Find("East").Find("Doorway").gameObject.SetActive(false);
        }
        else if (room.GetComponent<RoomManager>().roomMapX == 1)
        {
            room.transform.Find("West").Find("Doorway").gameObject.SetActive(false);
        }
        if (room.GetComponent<RoomManager>().roomMapY == maximumMapY - 1)
        {
            room.transform.Find("South").Find("Doorway").gameObject.SetActive(false);
        }
        else if (room.GetComponent<RoomManager>().roomMapY == 1)
        {
            room.transform.Find("North").Find("Doorway").gameObject.SetActive(false);
        }
    }

    // Changes the currentX and currentY values based on the direction of the door the player left from
    private void GetMapChangeFromDirection(string doorDirection) // Changes the current map values, returns whether player is at farthest out room
    {
        if (doorDirection.Equals("North"))
        {
            currentY -= 1;
        }
        else if (doorDirection.Equals("South"))
        {
            currentY += 1;
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


    // Creates the position a room needs to be spawned at
    // The offset value can be changed if more space is needed for oddly sized rooms
    private Vector3 GetNewRoomPosition(string doorDirection) // I may have to increase the dimensions of stuff if some rooms are really weirdly shaped intentionally
    {
        int offset = 25;
        if (doorDirection.Equals("North"))
        {
            return new Vector3(currentRoomCenterPos.x, currentRoomCenterPos.y + offset, 0);
        }
        else if (doorDirection.Equals("South"))
        {
            return new Vector3(currentRoomCenterPos.x, currentRoomCenterPos.y - offset, 0);
        }
        else if (doorDirection.Equals("West"))
        {
            return new Vector3(currentRoomCenterPos.x - offset, currentRoomCenterPos.y, 0);
        }
        else // "East"
        {
            return new Vector3(currentRoomCenterPos.x + offset, currentRoomCenterPos.y, 0);
        }
    }

    // Gets the string name of the connected door that the player is going to
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

    // Is a method for debugging, and prints a map of the dungeon with "1" meaning a room exists and "0" meaning nothing has been generated
    private void PrintRoomMap()
    {
        string toPrint = "";
        for (int i = 0; i < roomMap.GetLength(0); i++)
        {
            for (int j = 0; j < roomMap.GetLength(1); j++)
            {
                if (roomMap[i, j] != null)
                {
                    toPrint += "1";
                }
                else
                {
                    toPrint += "0";
                }
            }
            toPrint += "|\n";
        }

        Debug.Log(toPrint);
    }

    // Moves the singular pathfinding graph to the new room
    private void MovePathfindingGraph(Vector3 roomCenter)
    {
        var pathGraph = AstarPath.active.data.gridGraph;
        pathGraph.center = roomCenter;
        AstarPath.active.Scan();
        //pathGraph.SetDimensions() // Changes the graph's dimenstions if the room is differently shaped
    }





}
