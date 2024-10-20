using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

enum PlayerState
{
    Idle,
    Moving
}

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 1f;
    PlayerState playerState = PlayerState.Idle;
    Vector2 moveDir;

    private void OnEnable()
    {
        JoystickManager.onMove += SetDirection;
        JoystickManager.onActivate += SetMoving;
        JoystickManager.onDeactivate += SetIdle;
    }

    private void OnDisable()
    {
        JoystickManager.onMove -= SetDirection;
        JoystickManager.onActivate -= SetMoving;
        JoystickManager.onDeactivate -= SetIdle;
    }

    private void Update()
    {
        if (playerState == PlayerState.Moving)
        {
            Move();
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

    public void Move()
    {
        transform.Translate(moveDir * speed * Time.deltaTime);
    }

}
