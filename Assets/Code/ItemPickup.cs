using UnityEngine;

public class ItemPickup : Interactable
{
    /// <summary>
    /// Handles non-permanant item pickups, such as health
    /// </summary>

    public Item item;
    public override void Interact()
    {
        base.Interact();
        PickUp();
    }

    void PickUp()
    {
        
        Debug.Log("picking up " + item.name);
        bool wasPickedUp = Inventory.instance.Add(item);
        if(wasPickedUp)
            Destroy(gameObject);
    }
}
