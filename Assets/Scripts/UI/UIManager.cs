using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    [SerializeField] private float buttonHoldTime = 1;

    UIDocument uiDoc;
    VisualElement root;
    VisualElement mainEl;
    VisualElement linesEl;
    VisualElement lettersEl;
    MagicManager magicManager;

    private void OnEnable()
    {
        DraggableLetter.onSelect += OnLetterSelect;
        DraggableLetter.onRelease += CancelLetterHold;
    }

    private void OnDisable()
    {
        DraggableLetter.onSelect -= OnLetterSelect;
        DraggableLetter.onRelease -= CancelLetterHold;
    }

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
        }
    }

    void OnLetterSelect(DraggableLetter draggableLetter)
    {
        //if (draggableLetter.state == ButtonState.Idle)
            //StartCoroutine(HoldButtonRoutine(draggableLetter));
    }

    void CancelLetterHold(DraggableLetter draggableLetter)
    {
        //StopCoroutine(HoldButtonRoutine(draggableLetter));
        //Debug.Log("cancel hold");
        //StopAllCoroutines();
    }

    public IEnumerator HoldButtonRoutine(DraggableLetter draggableLetter)
    {
        Debug.Log("start coroutine");
        yield return new WaitForSeconds(buttonHoldTime);
        Debug.Log("reach end of coroutine");
        if (draggableLetter.state == ButtonState.Pressed)
        {
            magicManager.SpawnEffect(draggableLetter.value);
        }
    }
}
