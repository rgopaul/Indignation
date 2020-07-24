using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportTo : MonoBehaviour
{
    // Pretty Basic Teleport Feature
    // Should be Reusable for multiple things.
    public GameObject objectToTeleport;
    public Transform objectToGoTo;

    void Update()
    {
        if (Input.GetKeyDown("3"))
        {
            objectToTeleport.transform.position = objectToGoTo.transform.position;
            Physics.SyncTransforms(); //Forces the Engine to set the new position (otherwise the teleport wouldn't happen)
        }
    }
}
