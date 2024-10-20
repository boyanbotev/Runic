using System.Collections;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

enum JoystickState
{
    Idle,
    Dragging
}

public class JoystickManager : MonoBehaviour
{
    public static event Action<Vector2> onMove;
    public static event Action onActivate;
    public static event Action onDeactivate;
    VisualElement joystick;
    VisualElement joystickHead;
    VisualElement joystickBg;
    Vector2 originalPos;
    Vector2 currentPos;
    JoystickState joystickState = JoystickState.Idle;

    private void Awake()
    {
        var uiDoc = FindObjectOfType<UIDocument>();
        var root = uiDoc.rootVisualElement;
        joystick = root.Q(className: "joystick");
        joystickHead = root.Q(className: "joystick-head");
        joystickBg = root.Q(className: "joystick-bg");

        SetupJoystick();
    }

    void SetupJoystick()
    {
        joystickHead.RegisterCallback<PointerDownEvent>(evt => OnJoystickPointerDown(evt));
        joystickHead.RegisterCallback<PointerMoveEvent>(evt => OnJoystickPointerMove(evt));
        joystickHead.RegisterCallback<PointerUpEvent>(evt => OnJoystickPointerUp(evt));
        joystickHead.RegisterCallback<PointerLeaveEvent>(evt => OnJoystickLeave(evt));
    }

    void OnJoystickPointerDown(PointerDownEvent evt)
    {
        if (joystickState != JoystickState.Idle) return;

        onActivate?.Invoke();

        CalculatePos();

        joystickState = JoystickState.Dragging;
    }

    void OnJoystickPointerMove(PointerMoveEvent evt)
    {
        if (joystickState == JoystickState.Idle) return;

        SetJoystickPos(evt.position); 

        Vector2 moveDirection = currentPos - originalPos;
        onMove?.Invoke(moveDirection);
    }

    void OnJoystickPointerUp(PointerUpEvent evt)
    {
        Deactivate();
    }

    void OnJoystickLeave(PointerLeaveEvent evt)
    {
        Deactivate();
    }

    void Deactivate()
    {
        if (joystickState == JoystickState.Idle) return;

        joystickState = JoystickState.Idle;
        joystickHead.style.left = originalPos.x;
        joystickHead.style.top = originalPos.y;

        onDeactivate?.Invoke();
    }

    void SetJoystickPos(Vector2 pos)
    {
        float elementWidth = joystickHead.resolvedStyle.width;
        float elementHeight = joystickHead.resolvedStyle.height;

        var clampedPos = new Vector2(
            Mathf.Clamp(pos.x, joystickBg.worldBound.x + elementWidth / 3, joystickBg.worldBound.x + joystickBg.resolvedStyle.width - elementWidth / 3),
            Mathf.Clamp(pos.y, joystickBg.worldBound.y + elementHeight / 3, joystickBg.worldBound.y + joystickBg.resolvedStyle.height - elementHeight / 3)
        );

        var adjustedPos = new Vector2(
            clampedPos.x - joystickBg.worldBound.x - elementWidth / 2,
            clampedPos.y - joystickBg.worldBound.y - elementHeight / 2
        );

        joystickHead.style.left = adjustedPos.x;
        joystickHead.style.top = adjustedPos.y;

        currentPos = adjustedPos;
    }

    private void CalculatePos()
    {
        Vector2 pos = new Vector2(joystickHead.worldBound.x, joystickHead.worldBound.y);
        Vector2 bgPos = new Vector2(joystickBg.worldBound.x, joystickBg.worldBound.y);

        originalPos = new Vector2(pos.x - bgPos.x, pos.y - bgPos.y);
    }
}
