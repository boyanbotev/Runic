using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
     TextMeshPro healthText;
    public float maxHealth = 10;

    void Awake()
    {
        healthText = GetComponent<TextMeshPro>();
    }
    
    public void UpdateHealth(float health)
    {
        healthText.text = health + " / " + maxHealth;
    }
}
