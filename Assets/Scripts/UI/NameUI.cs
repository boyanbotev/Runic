using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NameUI : MonoBehaviour
{
    TextMeshPro nameText;

    void Awake()
    {
        nameText = GetComponent<TextMeshPro>();
    }

    private void Start()
    {
        this.gameObject.SetActive(false);
    }
    public void UpdateName(string name)
    {
        nameText.text = name;
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }
}
