using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("Stats")]
    public Stat maxHealth;
    public Stat healthRegenRate;
    public Stat maxStamina;
    public Stat staminaRegenRate;
    public Stat staminaRegenDelay;
    public Stat rageDecayRate;
    public Stat rageDecayDelay;

    [Header("Modifier Stats")]
    public Stat damage;
    public Stat armor;
    public Stat healingAmount;

    //Max Rage, Stamina Timer, and Rage Timer
    private float maxRage = 100;
    private float staminaRegenTimer = 0.0f;
    private float rageDecayTimer = 0.0f;

    //Boolean to greenlight Healing
    private bool HealingUp;

    //Getters / Setters for Current Status of Core Stats
    public float currentHealth { get; private set; }
    public float currentStamina { get; private set; }
    public float currentRage { get; private set; }

    //Events
    public event System.Action<float, float> OnHealthChanged;
    public event System.Action<float, float> OnRageChanged;
    public event System.Action<bool> InvokeEnrage;
    public event System.Action<float, float> OnStaminaChanged;

    private void Awake()
    {
        currentHealth = maxHealth.GetValue();
        currentStamina = maxStamina.GetValue();
        currentRage = 0;
    }

    //Handle Stamina Regen and Rage Decay
    void LateUpdate()
    {
        //Don't bother if there's nothing to Regen or Decay for the Object (i.e: Bosses, Enemies, etc)
        //if (staminaRegenRate.GetValue() == 0 || rageDecayRate.GetValue() == 0)
            //return;

        if (staminaRegenTimer >= staminaRegenDelay.GetValue())
        {
            StamRegen();
        } 
        else
            staminaRegenTimer += Time.deltaTime;

        if (rageDecayTimer >= rageDecayDelay.GetValue())
        {
            RageDecay();
        }
        else
            rageDecayTimer += Time.deltaTime;

        if(Input.GetKeyDown("1"))
        {
            TakeDamage(50f);
        }
        if(Input.GetKeyDown("2"))
        {
            IncreaseRage(50f);
        }

        if(HealingUp == true)
        {
            HealDamage(healingAmount.GetValue());
        }

    }

    /* [Health Methods] */
    public void TakeDamage(float damage)
    {
        damage -= armor.GetValue();
        damage = Mathf.Clamp(damage, 1, float.MaxValue);

        Debug.Log(transform.name + " takes " + damage + " damage");
        currentHealth -= damage;

        OnHealthChanged?.Invoke(maxHealth.GetValue(), currentHealth);

        if (currentHealth <= 0)
        {
            Death();
        }
    }

    public void HealDamage(float healup)
    {
        currentHealth = (float)Mathf.Clamp(currentHealth + (healup * Time.deltaTime), 0.0f, maxHealth.GetValue());
        OnHealthChanged?.Invoke(maxHealth.GetValue(), currentHealth);
        Debug.Log("current Health: " +  currentHealth);
    }
    public void InvokeHeal(bool healing)
    {
        if (healing == false)
        {
            HealingUp = false;
        }
        if (currentHealth < maxHealth.GetValue() && healing == true)
        {
            HealingUp = true;
        }

            
    }

    //public bool CanHeal()
    //{
    //    //include item stipulation here
    //    if(
    //        return true;
    //    else
    //        return false;
    //}

    /* [Stamina Methods] */
    public void DecreaseStamina(float decrease)
    {
        decrease = Mathf.Clamp(decrease, 0, maxStamina.GetValue());
        currentStamina -= decrease;

        OnStaminaChanged?.Invoke(maxStamina.GetValue(), currentStamina);
        resetStaminaTimer();
    }

    private void StamRegen()
    {
        currentStamina = (float)Mathf.Clamp(currentStamina + (staminaRegenRate.GetValue() * Time.deltaTime), 0.0f, maxStamina.GetValue());
        OnStaminaChanged?.Invoke(maxStamina.GetValue(), currentStamina);
    }

    public bool OutOfStam()
    {
        if (currentStamina <= 0)
            return true;
        else
            return false;
    }

    public void resetStaminaTimer()
    {
        staminaRegenTimer = 0.0f;
    }

    /* [Rage Methods] */
    public void IncreaseRage(float increase)
    {
        //increase = Mathf.Clamp(increase, 0, maxRage);
        currentRage = Mathf.Clamp(currentRage + increase, 0, maxRage);

        OnRageChanged?.Invoke(maxRage, currentRage);
        resetRageTimer();

        if (currentRage == maxRage)
            InvokeEnrage?.Invoke(true);

        //Debug.Log("Current Rage: " + currentRage);
    }

    private void RageDecay()
    {
        currentRage = (float)Mathf.Clamp(currentRage - (rageDecayRate.GetValue() * Time.deltaTime), 0.0f, maxRage);
        OnRageChanged?.Invoke(maxRage, currentRage);
    }

    public void DecreaseRage(float decrease)
    {
        decrease = Mathf.Clamp(decrease, 0, maxRage);
        currentRage -= decrease;

        OnRageChanged?.Invoke(maxRage, currentRage);
        resetRageTimer();
    }

    public bool OutOfRage()
    {
        if (currentRage <= 0)
            return true;
        else
            return false;
    }

    public void resetRageTimer()
    {
        rageDecayTimer = 0.0f;
    }

    //Meant to be Overridden
    public virtual void Death ()
    {
        BroadcastMessage("Die");
        Debug.Log(transform.name + " has died.");
    }
}