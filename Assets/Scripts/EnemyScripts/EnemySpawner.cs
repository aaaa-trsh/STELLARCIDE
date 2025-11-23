using System.Collections.Specialized;
using System.Diagnostics;
using System.Security.Cryptography;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private float spawnTimer = 0f;
    private float spawnInterval = 8f;
    public GameObject enemyPrefab;


    void Start()
    {
        UnityEngine.Debug.Log(transform.childCount);
        foreach(Transform child in transform)
        {
            UnityEngine.Debug.Log(child.transform.position);
        }
    }

    void Update()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval)
        {
            SpawnEnemy();
            spawnTimer = 0;
        }
    }

    private void SpawnEnemy()
    {
        foreach(Transform child in transform)
        {
            if (child.transform.childCount == 0)
            {
                UnityEngine.Debug.Log("spawning enemy");
                GameObject enemy = Instantiate(enemyPrefab, child.transform.position, child.transform.rotation);
                enemy.transform.SetParent(child.transform);
            } else {
                UnityEngine.Debug.Log("enemy not dead");
            }
        }
    }
}
