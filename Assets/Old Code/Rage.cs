using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rage : MonoBehaviour
{
    [HideInInspector]
    public float RageAmt = 0;
    public float RageLimit = 100;
    private bool RageTrigger = false;

    private void LateUpdate()
    {
        if(RageAmt > RageLimit)
        {
            RageAmt = RageLimit;
        }

        if(RageAmt < 0)
        {
            RageAmt = 0;
        }
    }
    
    public void IncreaseRage(float increase)
    {
        RageAmt += increase;
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
}
