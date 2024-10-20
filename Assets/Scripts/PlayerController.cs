using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

enum PlayerState
{
    Idle,
    Moving,
}

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 1f;
    [SerializeField] private float dashSpeed = 3f;
    PlayerState playerState = PlayerState.Idle;
    Vector2 moveDir = Vector2.down;
    Boolean isDashing = false;

    private void OnEnable()
    {
        JoystickManager.onMove += SetDirection;
        JoystickManager.onActivate += SetMoving;
        JoystickManager.onDeactivate += SetIdle;
        MagicManager.onEffectSpawn += OnSpawnEffect;

    }

    private void OnDisable()
    {
        JoystickManager.onMove -= SetDirection;
        JoystickManager.onActivate -= SetMoving;
        JoystickManager.onDeactivate -= SetIdle;
        MagicManager.onEffectSpawn -= OnSpawnEffect;
    }

    private void Update()
    {
        if (playerState == PlayerState.Moving)
        {
            Move();
        }
    }

    void OnSpawnEffect(string letter)
    {
        if (letter == "n")
        {
            SetDash();
        }
    }

    void SetDirection(Vector2 moveDir)
    {
        this.moveDir = new Vector2(moveDir.x, -moveDir.y).normalized;
    }

    void SetIdle()
    {
        playerState = PlayerState.Idle;
    }

    void SetMoving()
    {
        playerState = PlayerState.Moving;
    }

    void SetDash()
    {
        isDashing = true;
        playerState = PlayerState.Moving;
        StartCoroutine(DashRoutine());
    }

    public void Move()
    {
        var moveSpeed = isDashing ? dashSpeed : speed;
        transform.Translate(moveDir * moveSpeed * Time.deltaTime);
    }

    IEnumerator DashRoutine()
    {
        yield return new WaitForSeconds(0.25f);
        isDashing = false;

        // check if mouse is in joystick head
        if (FindObjectOfType<JoystickManager>().joystickState == JoystickState.Dragging)
        {
            playerState = PlayerState.Moving;
        }
        else
        {
            playerState = PlayerState.Idle;
        }
        
    }
}
