using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// General Script to encompass all interactable objects, items, and entities
/// </summary>
public class Interactable : MonoBehaviour
{
    public float radius = 3f;
    public Transform interactionTransfrom;
    bool isFocus = false;
    bool hasInteracted = false;
    Transform player;

    public virtual void Interact()
    {
        Debug.Log("Interaction with " + interactionTransfrom.name);
    }

    private void Update()
    {
        if (isFocus && !hasInteracted)
        {
            float distance = Vector3.Distance(player.position, interactionTransfrom.position);
            if (distance <= radius)
            {
                Debug.Log("Initiated Interaction");
                Interact();
                hasInteracted = true;
            }
        }
    }

    public void OnFocused (Transform playerTransform)
    {
        isFocus = true;
        player = playerTransform;
        hasInteracted = false;
    }

    public void OnDefocused()
    {
        isFocus = false;
        player = null;
        hasInteracted = false;
    }

    void OnDrawGizmosSelected()
    {
        if(interactionTransfrom == null)
        {
            interactionTransfrom = transform;
        }
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(interactionTransfrom.position, radius);
    }
}
