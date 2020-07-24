using UnityEngine;
using UnityEngine.UI;


public class HealthOrbSlot : MonoBehaviour
{
    public Image icon;

    Item item;
    public void AddItem(Item newItem)
    {
        item = newItem;

        icon.sprite = item.icon;
        icon.enabled = true;
    }

    public void ClearSlot()
    {
        item = null;

        icon.sprite = null;
        icon.enabled = false;
    }

    public void ClearItem()
    { 
        Debug.Log("slot cleared");
        item.Use();
        Inventory.instance.Remove(item);
    }


}
