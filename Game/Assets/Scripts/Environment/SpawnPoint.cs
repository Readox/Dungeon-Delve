using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public SpriteRenderer sr;
    public Animator anim;
    public Rigidbody2D rb;
    public BoxCollider2D bc;
    public GameObject prefab;
    private List<GameObject> spawnedEnemyList = new List<GameObject>();

    public bool bossSpawner;
    public bool spawnImmediately;
    public float repeatInterval;

    public float maxEnemyCount;
    float enemyCount;
    public bool isSpawnCycleCompleted = false;

    // Start is called before the first frame update
    void Awake()
    {
        if (bossSpawner) 
        {
            sr.enabled = false;
            bc.enabled = false;
            SpawnObject();
        }
        if (spawnImmediately && prefab != null && !(enemyCount >= maxEnemyCount) && !bossSpawner)
        {
            StartAnimation();
            /*
            enemyCount += 1;
            GameObject enemy = Instantiate(prefab, transform.position, Quaternion.identity);
            enemy.GetComponentInChildren<EnemyStats>().SetHomeSpawner(gameObject);
            spawnedEnemyList.Add(enemy);
            */
        }
        //SpawnObject(); // spawn one object on game start
        if (repeatInterval > 0)
        {
            InvokeRepeating("StartAnimation", Random.Range(1f, 3f), repeatInterval);
        }
    }

    public GameObject StartAnimation()
    {
        if (enemyCount >= maxEnemyCount)
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

    public void CheckFinished()
    {
        if (enemyCount >= maxEnemyCount)
        {
            anim.SetBool("Finished", true);
        }
    }

    public void CheckSpawnCycleCompleted()
    {
        if (spawnedEnemyList.Count == 0)
        {
            isSpawnCycleCompleted = true;
        }
    }

    public GameObject SpawnObject()
    {
        if (prefab != null && !(enemyCount >= maxEnemyCount))
        {
            enemyCount += 1;
            GameObject enemy = Instantiate(prefab, new Vector3 (transform.position.x + Random.Range(-1f, 1f), transform.position.y + Random.Range(-1f, 1f), transform.position.z), Quaternion.identity);
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
