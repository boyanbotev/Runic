using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private void OnEnable()
    {
        EnemyController.onTouchPlayer += GameOver;
    }

    private void OnDisable()
    {
        EnemyController.onTouchPlayer -= GameOver;
    }

    void GameOver()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
