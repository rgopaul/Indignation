using UnityEngine;
/// <summary>
///holds data for all items in game
/// </summary>

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    new public string name = "New Item";
    public Sprite icon = null;
    public bool isDefaultItem = false;

    public virtual void Use()
    {
        //uses the item
        Debug.Log("Using " + name);
    }
}
