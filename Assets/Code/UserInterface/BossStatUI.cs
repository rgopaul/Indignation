using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossStatUI : MonoBehaviour
{
    public GameObject HUD;
    public Slider healthSlider;

    void Start()
    {
        GetComponent<CharacterStats>().OnHealthChanged += OnHealthChanged;
    }

    void OnHealthChanged(float maxHealth, float currentHealth)
    {
        healthSlider.value = (float)currentHealth / maxHealth;
    }
}
