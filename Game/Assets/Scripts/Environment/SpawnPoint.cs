using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{

    public GameObject prefab;

    public float repeatInterval;

    // Start is called before the first frame update
    public void Start()
    {
        //SpawnObject(); // spawn one object on game start
        if (repeatInterval > 0)
        {
            InvokeRepeating("SpawnObject", 0.0f, repeatInterval);
        }
    }

    public GameObject SpawnObject()
    {
        if (prefab != null)
        {
            return Instantiate(prefab, transform.position, Quaternion.identity);
        }

        return null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
