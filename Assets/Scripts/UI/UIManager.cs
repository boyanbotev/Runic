using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    public static event Action<string> onWordSent;
    [SerializeField] private float buttonHoldTime = 1;

    UIDocument uiDoc;
    VisualElement root;
    VisualElement mainEl;
    VisualElement linesEl;
    VisualElement lettersEl;
    VisualElement spellingTextBarEl;
    VisualElement spellingTextButtonEl;
    Label spellingTextLabel;
    MagicManager magicManager;

    private void OnEnable()
    {
        DraggableLetter.onSelect += OnLetterSelect;
        DraggableLetter.onRelease += CancelLetterHold;
        MagicManager.onEffectSpawn += ClearSpellingText;
    }

    private void OnDisable()
    {
        DraggableLetter.onSelect -= OnLetterSelect;
        DraggableLetter.onRelease -= CancelLetterHold;
        MagicManager.onEffectSpawn -= ClearSpellingText;
    }

    private void Awake()
    {
        uiDoc = FindObjectOfType<UIDocument>();
        root = uiDoc.rootVisualElement;
        mainEl = root.Q(className: "main");
        linesEl = root.Q(className: "writing-lines");
        lettersEl = root.Q(className: "letters");
        spellingTextBarEl = root.Q(className: "spelling-text-bar");
        spellingTextButtonEl = root.Q(className: "spelling-text-button");

        magicManager = FindObjectOfType<MagicManager>();

        Application.targetFrameRate = 60;

        CreateLetters();
        CreateSpellingText();
    }

    void CreateLetters()
    {
        string[] letters = { "s", "a", "t", "p", "i", "n" };

        foreach (string letter in letters)
        {
            DraggableLetter draggableLetter = new(letter, mainEl);
            lettersEl.Add(draggableLetter);
        }
    }

    void CreateSpellingText()
    {
        spellingTextBarEl.Clear();
        spellingTextLabel = new Label();
        spellingTextBarEl.Add(spellingTextLabel);

        spellingTextButtonEl.RegisterCallback<PointerDownEvent>(evt =>
        {
            onWordSent?.Invoke(spellingTextLabel.text);
            spellingTextLabel.text = "";
        });
    }

    void ClearSpellingText(string letter)
    {
        spellingTextLabel.text = "";
    }

    void OnLetterSelect(DraggableLetter draggableLetter) {
        spellingTextLabel.text += draggableLetter.value;
    }

    void CancelLetterHold(DraggableLetter draggableLetter)
    {

    }

    public IEnumerator HoldButtonRoutine(DraggableLetter draggableLetter)
    {
        yield return new WaitForSeconds(buttonHoldTime);
        if (draggableLetter.state == ButtonState.Pressed)
        {
            magicManager.SpawnEffect(draggableLetter.value);
        }
    }
}

/*
 * TODO:
 * Implemented:
 * hold letter to charge letter spell
 * tap letter quickly to spell word
 * joystick movement
 * 
 *  Nice to have:
 *  cooldown or charging animation on the spells
 *  nauthiz dash
 */