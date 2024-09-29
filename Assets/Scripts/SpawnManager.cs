using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnManager : MonoBehaviour
{
    public static event Action<GameObject> onEnemySpawned;
    [SerializeField] private GameObject enemyPrefab;
    // define an area in the inspector where enemies can be spawned
    [SerializeField] private Vector4 spawnRange;
    [SerializeField] private float spawnTime = 3;
    [SerializeField] Transform player;

    void Start()
    {
        // spawn an enemy every 3 seconds
        InvokeRepeating(nameof(SpawnEnemy), 1, spawnTime);
    }

    void SpawnEnemy()
    {
        float spawnX = UnityEngine.Random.Range(spawnRange.x, spawnRange.z);
        float spawnY = UnityEngine.Random.Range(spawnRange.y, spawnRange.w);

        Vector2 spawnPosition = new(spawnX, spawnY);

        var enemy = Instantiate(enemyPrefab, spawnPosition, enemyPrefab.transform.rotation);
        onEnemySpawned?.Invoke(enemy);
        enemy.GetComponent<EnemyController>().target = player;
    }
}
