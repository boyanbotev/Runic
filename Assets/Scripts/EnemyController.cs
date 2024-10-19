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
    KnockedBack,
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
    [SerializeField] private float health = 7;
    [SerializeField] private float freezeTime = 3f;
    private HealthBar healthBar;
    private NameUI nameUI;
    private Color originalColor;
    private Color freezeColor = new Color(0.5f, 0.5f, 1f, 1f);
    private Vector2 knockBackVector;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        state = EnemyState.Chasing;
        originalColor = GetComponent<SpriteRenderer>().color;
        healthBar = GetComponentInChildren<HealthBar>();
        nameUI = GetComponentInChildren<NameUI>();
    }

    private void OnEnable()
    {
        UIManager.onWordSent += OnWordSent;
    }

    private void OnDisable()
    {
        UIManager.onWordSent -= OnWordSent;
    }

    private void Start()
    {
        healthBar.maxHealth = health;
        healthBar.UpdateHealth(health);
        nameUI.UpdateName(word);
    }

    void FixedUpdate()
    {
        if (state == EnemyState.Chasing)
        {
            MoveToTarget();
        }
        else if (state == EnemyState.KnockedBack)
        {
            MoveBack();
        }
    }

    void OnWordSent(string word)
    {
        Debug.Log("Word sent: " + word + ", this.word:" + this.word);

        if (word == this.word)
        {
            TakeDamage(1000);
        }
    }

    void MoveToTarget()
    {
        Vector2 moveDir = target.position - transform.position;
        transform.Translate(moveDir.normalized * speed * Time.deltaTime);
    }

    void MoveBack()
    {
        transform.Translate(knockBackVector * Time.deltaTime);
        knockBackVector = Vector2.Lerp(knockBackVector, Vector2.zero, 0.1f);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        var player = target.gameObject;
        var effect = collider.gameObject.GetComponentInParent<AttackEffect>();
        var freeze = collider.gameObject.GetComponentInParent<Freeze>();

        if (freeze)
        {
            state = EnemyState.Frozen;
            GetComponent<SpriteRenderer>().color = freezeColor;
            StopCoroutine("Unfreeze");
            StartCoroutine("Unfreeze");
            TakeDamage(freeze.damage);
        }
        else if (effect)
        {
            TakeDamage(effect.damage);
        }

        if (effect?.knockbackForce > 0 && state == EnemyState.Chasing)
        {
            state = EnemyState.KnockedBack;
            knockBackVector = (transform.position - effect.transform.position).normalized * effect.knockbackForce;
            StopCoroutine("KnockBackRoutine");
            StartCoroutine("KnockBackRoutine");
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

    void TakeDamage(float damage)
    {
        health -= damage;
        healthBar.UpdateHealth(health);

        if (health <= 0)
        {
            onDestroyed?.Invoke(gameObject);
            Destroy(gameObject);
        }
    }

    IEnumerator KnockBackRoutine()
    {
        yield return new WaitForSeconds(0.2f);
        state = EnemyState.Chasing;
    }
}
