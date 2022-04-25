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


    // The generation doesnt take into account the difficulty of floors increasing yet, so I need to create either monster prefabs with higher stats or a script that scales them
    public bool floorBossRoom; // set to true if the floor's final boss spawns in this room

    public void CreateRoomContent(int internalDifficultyLevel) // This goes from 1-5, for the room itself, with 5 being a boss room
    {
        Debug.Log("Internal Difficulty Level: " + internalDifficultyLevel);
        if (internalDifficultyLevel == 5) // then spawn boss monster
        {
            GameObject ns = rmg_Script.GenerateBossSpawner(gameObject.transform, gameObject.transform.position.x, gameObject.transform.position.y, floorBossRoom); 
            enemySpawnerList.Add(ns.GetComponent<SpawnPoint>());
        }
        else // do regular spawners
        {
            for (int i = 0; i < internalDifficultyLevel; i++)
            {
                GameObject ns = rmg_Script.GenerateRandomSpawner(gameObject.transform, gameObject.transform.position.x, gameObject.transform.position.y, internalDifficultyLevel);
                enemySpawnerList.Add(ns.GetComponent<SpawnPoint>()); 
            }
        }


        // Once all that is done, turn on mob spawners
        foreach(SpawnPoint sp in enemySpawnerList)
        {
            sp.gameObject.SetActive(true);
        }
    }



}
