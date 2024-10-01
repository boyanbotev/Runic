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
    Frozen,
}

public class EnemyController : MonoBehaviour
{
    public static event Action onTouchPlayer;
    public static event Action<GameObject> onDestroyed;
    public string word = string.Empty;
    public Transform target;
    [SerializeField] float minApproachDistance = 10;
    [SerializeField] float minAttackDistance = 3;
    private EnemyState state;
    private Rigidbody2D rb;
    [SerializeField] private float speed = 1f;
    [SerializeField] private TextMeshPro healthDisplay;
    [SerializeField] private float freezeTime = 3f;
    private Color originalColor;
    private Color freezeColor = new Color(0.5f, 0.5f, 1f, 1f);

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        state = EnemyState.Chasing;
        originalColor = GetComponent<SpriteRenderer>().color;
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
        var effect = collider.gameObject.GetComponentInParent<AttackEffect>();
        var freeze = collider.gameObject.GetComponentInParent<Freeze>();

        if (freeze)
        {
            Debug.Log("freeze");
            state = EnemyState.Frozen;
            GetComponent<SpriteRenderer>().color = freezeColor;
            StartCoroutine(Unfreeze());
        }
        else if (effect)
        {
            onDestroyed?.Invoke(gameObject);
            Destroy(gameObject);
        }

        if (collider.gameObject == player)
        {
            onTouchPlayer?.Invoke();
        }
    }

    private IEnumerator Unfreeze()
    {
        yield return new WaitForSeconds(freezeTime);
        state = EnemyState.Chasing;
        GetComponent<SpriteRenderer>().color = originalColor;
    }
}
