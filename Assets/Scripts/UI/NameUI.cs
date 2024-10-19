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

    public void UpdateName(string name)
    {
        nameText.text = name;
    }
}
