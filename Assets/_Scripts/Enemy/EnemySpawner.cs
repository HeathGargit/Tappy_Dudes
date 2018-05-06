using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    //enemy type
    public GameObject m_EnemyType;
    //where enemy will spawn in row
    private Vector3 m_SpawnPoint;
    //how fast the enemies in this row move
    private float m_MoveSpeed;
    //spawn time bits.
    private float m_RespawnMin;
    private float m_RespawnMax;
    private float m_TimeToNextSpawn;

    public void Awake()
    {
        m_RespawnMin = 3.0f;
        m_RespawnMax = 6.0f;
        m_TimeToNextSpawn = Random.Range(m_RespawnMin, m_RespawnMax);
    }

    public void Init(Vector3 spawnPoint, float moveSpeed)
    {
        //init variables
        m_SpawnPoint = spawnPoint;
        m_MoveSpeed = moveSpeed;

        //randomly choose if the enemy is going left > right or right > left
        if (Random.Range(0, 2) == 1)
        {
            m_MoveSpeed *= -1.0f;
            m_SpawnPoint.x *= -1.0f;
            //m_EnemyType.gameObject.transform.Rotate(new Vector3(0, 1, 0), 180.0f);
        }

        SpawnEnemy();
    }

    private void SpawnEnemy()
    {
        //instantiate the enemy
        GameObject thisEnemy = Instantiate(m_EnemyType, m_SpawnPoint, Quaternion.identity);
        thisEnemy.GetComponent<EnemyMovement>().Init(1.0f, m_MoveSpeed);
        
        
        //if this enemy is moving reverse direction across the board, we need to rotate the transform to make it look like it's swimming forward.
        if (m_MoveSpeed < 0)
        {
            thisEnemy.transform.Rotate(new Vector3(0, 1, 0), 180.0f);
        }
        
        //set parent to the row, so that when the row is destroyed, all the enemies on that row are destroyed.
        thisEnemy.gameObject.transform.parent = gameObject.transform;
    }

    private void Update()
    {
        //take away spawn time
        m_TimeToNextSpawn -= Time.deltaTime;

        //if it's time to spawn
        if(m_TimeToNextSpawn <= 0.0f)
        {
            //spawn an enemy
            SpawnEnemy();
            m_TimeToNextSpawn = Random.Range(m_RespawnMin, m_RespawnMax);
        }
    }
}
