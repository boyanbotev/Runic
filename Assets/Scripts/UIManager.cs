using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    UIDocument uiDoc;
    VisualElement root;
    VisualElement mainEl;
    VisualElement linesEl;
    VisualElement lettersEl;
    MagicManager magicManager;

    private void Awake()
    {
        uiDoc = FindObjectOfType<UIDocument>();
        root = uiDoc.rootVisualElement;
        mainEl = root.Q(className: "main");
        linesEl = root.Q(className: "writing-lines");
        lettersEl = root.Q(className: "letters");

        magicManager = FindObjectOfType<MagicManager>();

        Application.targetFrameRate = 60;

        CreateLetters();
    }

    void CreateLetters()
    {
        string[] letters = { "s", "a", "t", "p", "i", "n" };
        
        foreach (string letter in letters)
        {
            DraggableLetter draggableLetter = new(letter, mainEl);
            lettersEl.Add(draggableLetter);

            draggableLetter.RegisterCallback<PointerDownEvent>(evt => magicManager.SpawnEffect(letter));
        }
    }
}
