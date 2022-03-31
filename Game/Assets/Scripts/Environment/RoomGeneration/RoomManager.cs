using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    private RoomGenerationManager rmg_Script;

    public List<SpawnPoint> enemySpawnerList = new List<SpawnPoint>();
    public List<GameObject> doorList = new List<GameObject>(); // Stores the doors in a room
    private List<GameObject> connectedDoorList = new List<GameObject>(); // Uses index from doorList to find the connected door

    [HideInInspector] public int roomMapX;
    [HideInInspector] public int roomMapY;

    void Awake()
    {
        rmg_Script = GameObject.FindWithTag("GameController").GetComponent<RoomGenerationManager>();
        for (int i = 0; i < doorList.Count; i++) // initializes list to prevent errors
        {
            connectedDoorList.Add(null);
        }
    }

    public void IsDoorwayAccessible(GameObject accessedDoor) // Maybe in the future I could play an animation of chains unlocking the door when conditions are met
    {
        int ticker = enemySpawnerList.Count;
        foreach(SpawnPoint sp in enemySpawnerList)
        {
            if (sp.isSpawnCycleCompleted)
            {
                ticker -= 1;
            }
        }
        if (ticker == 0)
        {
            rmg_Script.DoRoomChange(roomMapX, roomMapY, accessedDoor.transform.parent.gameObject.name);
            /*
            GameObject connectedDoor = CheckForConnectedDoors(accessedDoor);
            if (connectedDoor != null)
            {
                rmg_Script.HandleRoomChange(connectedDoor); // Goes and finds the correct room to go into, and handles it
            }
            else
            {
                GameObject addedConnectedDoor = rmg_Script.HandleRoomChange(accessedDoor.transform.parent.gameObject.name); // Generates a new room and then returns the connected door
                //connectedDoorList[connectedDoorReplaceIndex] = addedConnectedDoor;
            }
            */
        }
    }

    int connectedDoorReplaceIndex = -1;
    private GameObject CheckForConnectedDoors(GameObject accessedDoor)
    {
        for (int i = 0; i < doorList.Count; i++)
        {
            if (doorList[i] == accessedDoor)
            {
                connectedDoorReplaceIndex = i;
                if (connectedDoorList[i] != null) // This works because I initialize this list with null values to prevent an error
                {
                    return connectedDoorList[i];
                }
            }
        }
        return null;
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
