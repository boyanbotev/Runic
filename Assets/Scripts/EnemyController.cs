using TMPro;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

enum EnemyState
{
    Chasing,
}

public class EnemyController : MonoBehaviour
{
    public static event Action onTouchPlayer;
    public string word = string.Empty;
    public Transform target;
    [SerializeField] float minApproachDistance = 10;
    [SerializeField] float minAttackDistance = 3;
    private EnemyState state;
    private Rigidbody2D rb;
    [SerializeField] private float speed = 1f;
    [SerializeField] private TextMeshPro healthDisplay;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        state = EnemyState.Chasing;
    }

    void FixedUpdate()
    {
        if (state == EnemyState.Chasing)
        {
            MoveToTarget();
        }
    }

    void MoveToTarget()
    {
        Vector2 moveDir = target.position - transform.position;
        //rb.AddForce(moveDir.normalized * speed);
        //rb.velocity = Vector2.ClampMagnitude(rb.velocity, speed);
        transform.Translate(moveDir.normalized * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        var player = target.gameObject;

        if (collider.gameObject == player)
        {
            onTouchPlayer?.Invoke();
        }
    }
}
