using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParryCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        LancerController LC = other.transform.root.GetComponent<LancerController>();

        if (LC == null)
            return;

        LC.EnemyParried();
    }
}
