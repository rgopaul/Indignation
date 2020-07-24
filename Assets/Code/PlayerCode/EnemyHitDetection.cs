using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitDetection : MonoBehaviour
{
    private bool hasCollided = false;
    private CharacterStats charStats;

    private void OnEnable()
    {
        hasCollided = false;
        charStats = transform.root.GetComponent<CharacterStats>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.tag == "Enemy" && other.tag != "EnemyWep")
        {
            if (hasCollided == false)
            {
                hasCollided = true;
                other.transform.root.gameObject.BroadcastMessage("Hitstun");
                other.transform.root.gameObject.GetComponent<CharacterStats>().TakeDamage(charStats.damage.GetValue());
                other.transform.root.gameObject.GetComponent<CharacterStats>().IncreaseRage(5);
                Debug.Log("hit");
                StartCoroutine(CollisionTimer());
            }
        }
    }

    //Consider calling this from knight?
    public void HasCollidedSwitch()
    {
        if (hasCollided)
            hasCollided = false;
        else
            hasCollided = true;
    }

    private IEnumerator CollisionTimer()
    {
        yield return new WaitForSeconds(0.38f);
        hasCollided = false;
    }

   

}
