using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    public int space = 3;
    public List<Item> items = new List<Item>();
    public delegate void OnItemchanged();           //variable that updates whenever inventory is increased or decreased
    public OnItemchanged onItemChangedCallBack;
    public GameObject healthOrb;                    //takes in and reveals raw images


    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of inventory found");
        }
        instance = this;
    }
    
    public bool Add (Item item)
    {
        if(!item.isDefaultItem)
        {
            if(items.Count >= space)
            {
                //if there isn't enough room ADD() returns as a false boolean.
                Debug.Log("not enough room");
                return false;
            }
            // NOTES: Still need to find a way to update multiple RawImages
            // that are not hardcoded. But I should hardcode if this takes
            // far too much time. Ideally use an array?
            items.Add(item);
            healthOrb.SetActive(true); //something like healthOrb.next????
            if (onItemChangedCallBack != null)
            {
                onItemChangedCallBack.Invoke();
            }
            Debug.Log("WE GOT: " + items.Count + " HEALS");
        }
        return true;
    }

    public void Remove(Item item)
    {
        items.Remove(item);
        if (onItemChangedCallBack != null)
        {
            onItemChangedCallBack.Invoke();
        }
    }
}
