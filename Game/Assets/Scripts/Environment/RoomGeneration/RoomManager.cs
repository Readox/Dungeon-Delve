using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    private RoomGenerationManager rmg_Script;

    public List<SpawnPoint> enemySpawnerList = new List<SpawnPoint>();

    void Awake()
    {
        rmg_Script = GameObject.FindWithTag("GameController").GetComponent<RoomGenerationManager>();
    }

    public void IsDoorwayAccessible() // Maybe in the future I could play an animation of chains unlocking the door when conditions are met
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
            rmg_Script.HandleRoomChange();
        }
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
