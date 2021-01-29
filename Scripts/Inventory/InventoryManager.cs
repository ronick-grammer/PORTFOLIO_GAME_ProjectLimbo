using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public Item[] item;
    private bool hasItem;

    public bool Has_item(string name)  // check if gameObject has an item with the name in it's inventory
    {
        hasItem = false;
        for (int i = 0; i < item.Length; i++)
        {
            if (item[i].item.name.Equals(name)) // retrieve
            {
                hasItem= item[0].hasIt;
                break;
            }
        }
        return hasItem;
    }

    public void Set_hasItem(string name, bool hasIt) // set the variable "hasItem" 
    {
        for (int i = 0; i < item.Length; i++) // retrieve 
        {
            if (item[i].item.name.Equals(name))
            {
                item[i].hasIt = hasIt;
                break;
            }
        }
    }
}
