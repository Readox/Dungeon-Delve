using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    private RoomGenerationManager rmg_Script;

    public List<SpawnPoint> enemySpawnerList = new List<SpawnPoint>();

    [HideInInspector] public int roomMapX;
    [HideInInspector] public int roomMapY;

    void Awake()
    {
        rmg_Script = GameObject.FindWithTag("GameController").GetComponent<RoomGenerationManager>();
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
        }
    }

    public void CreateRoomContent()
    {
        
    }



}
