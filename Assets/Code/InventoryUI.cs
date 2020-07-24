using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Transform itemsParent;
    HealthOrbSlot[] slots;
    Inventory inventory;

    void Start()
    {
        inventory = Inventory.instance;
        //forces UpdateUI() to run when onItemChangedCallBack is called.
        inventory.onItemChangedCallBack += UpdateUI;

        slots = itemsParent.GetComponentsInChildren<HealthOrbSlot>();
    }
    public void UpdateUI()
    {
        Debug.Log("UPDATING UI");

        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.items.Count)
            {
                slots[i].AddItem(inventory.items[i]);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
        
    }
    public void OnRemoveAt()
    {
        UpdateUI();
    }
}
