using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    public static event Action<string> onWordSent;
    public bool hasSpelling = true;
    [SerializeField] private float buttonHoldTime = 1;
    [SerializeField] private string[] letters = { "s", "a", "t", "p", "i", "n" };

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

        if (hasSpelling)
        {
            CreateSpellingText();
        }
        else
        {
            spellingTextBarEl.style.display = DisplayStyle.None;
            spellingTextButtonEl.style.display = DisplayStyle.None;
        }
    }

    void CreateLetters()
    {
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
            if (!hasSpelling) return;

            onWordSent?.Invoke(spellingTextLabel.text);
            spellingTextLabel.text = "";
        });
    }

    void ClearSpellingText(string letter)
    {
        if (hasSpelling)
        {
            spellingTextLabel.text = "";
        }
    }

    void OnLetterSelect(DraggableLetter draggableLetter) {
        if (hasSpelling)
        {
            spellingTextLabel.text += draggableLetter.value;
        }
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
 * 
 *  cooldown or charging animation on the spells
 */