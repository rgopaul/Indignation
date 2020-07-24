using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerRage : MonoBehaviour
{
    [HideInInspector]
    public float RageAmt = 0;
    public float RageLimit = 100;
    public float RageDecay = 7f;
    private bool RageTrigger = false;

    public GameObject rageBarUI;
    public Slider slider;

    void Start()
    {
        slider.value = CalculateRage();
        RageAmt = 0;
    }

    private void Update()
    {
        RageAmt = Mathf.Clamp(RageAmt - (RageDecay * Time.deltaTime), 0.0f, RageLimit);

        if (RageAmt > RageLimit)
        {
            RageAmt = RageLimit;
        }

        if (RageAmt < 0)
        {
            RageAmt = 0;
        }

        slider.value = CalculateRage();
        //Debug.Log(RageAmt);
    }

    public void IncreaseRage(float increase)
    {
        RageAmt += increase;
    }

    public void DecreaseRage(float decrease)
    {
        RageAmt -= decrease;
    }

    public bool AtEnrage()
    {
        if (RageAmt == RageLimit && RageTrigger == false)
        {
            RageTrigger = true;
            return true;
        }
        else
            return false;
    }

    private float CalculateRage()
    {
        return RageAmt / RageLimit;
    }
}
