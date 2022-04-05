using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{

    public GameObject prefab;
    private List<GameObject> spawnedEnemyList = new List<GameObject>();

    public bool spawnImmediately;
    public float repeatInterval;

    public float maxEnemyCount;
    float enemyCount;
    public bool isSpawnCycleCompleted = false;

    // Start is called before the first frame update
    void Awake()
    {
        if (spawnImmediately && prefab != null && !(enemyCount >= maxEnemyCount))
        {
            enemyCount += 1;
            GameObject enemy = Instantiate(prefab, transform.position, Quaternion.identity);
            enemy.GetComponentInChildren<EnemyStats>().SetHomeSpawner(gameObject);
            spawnedEnemyList.Add(enemy);
        }
        //SpawnObject(); // spawn one object on game start
        if (repeatInterval > 0)
        {
            InvokeRepeating("SpawnObject", 2f, repeatInterval);
        }
    }

    public GameObject SpawnObject()
    {
        if (prefab != null && !(enemyCount >= maxEnemyCount))
        {
            enemyCount += 1;
            GameObject enemy = Instantiate(prefab, transform.position, Quaternion.identity);
            enemy.GetComponentInChildren<EnemyStats>().SetHomeSpawner(gameObject);
            spawnedEnemyList.Add(enemy);
            return enemy;
        }
        
        if (spawnedEnemyList.Count == 0)
        {
            isSpawnCycleCompleted = true;
        }

        return null;
    }

    public void RemoveFromList(GameObject g)
    {
        spawnedEnemyList.Remove(g);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
