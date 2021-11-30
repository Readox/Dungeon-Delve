using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMeleeEnemyMoveAttack : MonoBehaviour
{

    public GameObject player;

    public float moveSpeed;

    void Awake()
    {
        player = GameObject.FindWithTag("Player");
        StartCoroutine(EnemyMove());
    }

    IEnumerator EnemyMove()
    {
        //while(GetComponent<EnemyDamageReception>().)
        Vector2 enemyPos = transform.position;
        Vector2 targetPos = player.transform.position;
        Vector2 direction = (targetPos - enemyPos).normalized;
        this.GetComponent<Rigidbody2D>().velocity = direction * moveSpeed;

        yield return null;
    }


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
