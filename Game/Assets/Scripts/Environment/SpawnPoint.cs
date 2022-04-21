using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public SpriteRenderer sr;
    public Animator anim;
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
        if (spawnImmediately)
        {
            sr.enabled = false;
            anim.enabled = false;
        }
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
            InvokeRepeating("StartAnimation", 2f, repeatInterval);
        }
    }

    public GameObject StartAnimation()
    {
        if (isSpawnCycleCompleted)
        {
            SetFinished();
        }
        else
        {
            SetRunSpawn(true);
        }

        return null;
    }

    public void SetFinished()
    {
        anim.SetBool("Finished", true);
    }

    public void SetRunSpawn(bool val)
    {
        anim.SetBool("RunSpawn", val);
    }

    public void SetRunSpawnFalse()
    {
        anim.SetBool("RunSpawn", false);
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
