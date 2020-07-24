using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatUI : MonoBehaviour
{
    public GameObject HUD;
    public Slider healthSlider;
    public Slider stamSlider;
    public Slider rageSlider;

    void Start()
    {
        GetComponent<CharacterStats>().OnHealthChanged += OnHealthChanged;
        GetComponent<CharacterStats>().OnStaminaChanged += OnStaminaChanged;
        GetComponent<CharacterStats>().OnRageChanged += OnRageChanged;
        rageSlider.value = 0;
    }

    void OnHealthChanged(float maxHealth, float currentHealth)
    {
        healthSlider.value = (float)currentHealth / maxHealth;
    }

    void OnStaminaChanged(float maxStamina, float currentStamina)
    {
        stamSlider.value = (float)currentStamina / maxStamina;
    }

    void OnRageChanged(float maxRage, float currentRage)
    {
        rageSlider.value = (float)currentRage / maxRage;
    }
}
