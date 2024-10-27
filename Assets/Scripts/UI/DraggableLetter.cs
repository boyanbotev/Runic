using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UIElements;

public enum ButtonState
{
    Idle,
    Pressed
}

public class DraggableLetter : VisualElement
{
    public static event Action<DraggableLetter> onSelect;
    public static event Action<DraggableLetter> onRelease;
    public WritingLine line;
    public Vector2 originalPos;
    public string value;
    public ButtonState state = ButtonState.Idle;
    public Coroutine holdButtonCoroutine;
    private string draggableLetterClassName = "draggable-letter";
    private VisualElement bodyEl;

    public DraggableLetter(string letter, VisualElement mainEl)
    {
        value = letter;
        AddToClassList(draggableLetterClassName);

        var letterLabel = new Label(letter);
        Add(letterLabel);
        this.bodyEl = mainEl;

        RegisterCallback<PointerDownEvent>(evt =>
        {
            onSelect?.Invoke(this);
            state = ButtonState.Pressed;
            AddToClassList("pressed");
        });

        RegisterCallback<PointerUpEvent>(evt =>
        {
            onRelease?.Invoke(this);
            state = ButtonState.Idle;
            RemoveFromClassList("pressed");
        });

        RegisterCallback<PointerLeaveEvent>(evt =>
        {
            onRelease?.Invoke(this);
            state = ButtonState.Idle;
            RemoveFromClassList("pressed");
        });
    }

    public void Select()
    {
        onSelect?.Invoke(this);
        CalculatePos();
    }
    private void CalculatePos()
    {
        Vector2 pos = worldTransform.GetPosition();
        originalPos = new Vector2(pos.x - style.left.value.value, pos.y - style.top.value.value);
    }

    public void Reset()
    {
        style.left = 0;
        style.top = 0;
        line = null;
    }

    public void SetDraggedPos(Vector2 pos)
    {
        float elementWidth = resolvedStyle.width;
        float elementHeight = resolvedStyle.height;

        var clampedPos = new Vector2(
            Math.Clamp(pos.x, bodyEl.worldBound.x + elementWidth / 2, bodyEl.worldBound.x + bodyEl.worldBound.width - elementWidth / 2),
            Math.Clamp(pos.y, bodyEl.worldBound.y + elementHeight / 2, bodyEl.worldBound.y + bodyEl.worldBound.height - elementHeight / 2)
        );

        var adjustedPos = new Vector2(
            clampedPos.x - originalPos.x - elementWidth / 2,
            clampedPos.y - originalPos.y - elementHeight / 2
        );

        style.left = adjustedPos.x;
        style.top = adjustedPos.y;
    }
}
