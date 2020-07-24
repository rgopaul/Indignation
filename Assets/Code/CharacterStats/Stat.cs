using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat
{
    [SerializeField] private float baseValue = 0;

    private List<float> modifiers = new List<float>();

    public float GetValue()
    {
        float finalValue = baseValue;
        modifiers.ForEach(x => finalValue += x);
        return finalValue;
    }

    public void SetValue(float newValue)
    {
        baseValue = newValue;
    }

    public void AddModifier(float mod)
    {
        if(mod != 0)
        {
            modifiers.Add(mod);
        }
    }

    public void RemoveModifier(float mod)
    {
        if (mod != 0)
        {
            modifiers.Remove(mod);
        }
    }
}
