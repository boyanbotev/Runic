using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public List<GameObject> enemies = new List<GameObject>();

    private void OnEnable()
    {
        EnemyController.onTouchPlayer += GameOver;
        EnemyController.onDestroyed += OnEnemyDestroyed;
        SpawnManager.onEnemySpawned += OnEnemySpawned;

        InvokeRepeating("EvaluateGame", 3, 1);
    }

    private void OnDisable()
    {
        EnemyController.onTouchPlayer -= GameOver;
        EnemyController.onDestroyed -= OnEnemyDestroyed;
        SpawnManager.onEnemySpawned -= OnEnemySpawned;
    }

    void OnEnemySpawned(GameObject enemy)
    {
        enemies.Add(enemy);
    }

    void OnEnemyDestroyed(GameObject enemy)
    {
        enemies.Remove(enemy);
    }

    void GameOver()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void EvaluateGame()
    {
        if (enemies.Count == 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
