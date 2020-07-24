using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stamina : MonoBehaviour
{
    public float stamina;
    public float maxStamina;
    public float staminaRegen;
    public float TimeToRegen = 2.0f;
    public float staminaSprintDrain = 0.1f;

    private float regenTimer = 0.0f;

    public GameObject StamBarUI;
    public Slider slider;

    void Start()
    {
        stamina = maxStamina;
        slider.value = CalculateStamina();
    }

    void LateUpdate()
    {
        if (regenTimer >= TimeToRegen)
            stamina = Mathf.Clamp(stamina + (staminaRegen * Time.deltaTime), 0.0f, maxStamina);
        else
            regenTimer += Time.deltaTime;

        if (stamina > maxStamina)
        {
            stamina = maxStamina;
        }
        slider.value = CalculateStamina();
    }

    float CalculateStamina()
    {
        return stamina / maxStamina;
    }

    public float GetStamina()
    {
        return stamina;
    }

    public bool OutOfStam()
    {
        if (stamina <= 0)
            return true;
        else
            return false;
    }

    public void setTimer()
    {
        regenTimer = 0.0f;
    }

    public void DecreaseStam(float decrease)
    {
        stamina -= decrease;
        setTimer();
    }
}
