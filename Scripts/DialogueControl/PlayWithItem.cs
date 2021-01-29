using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayWithItem : MonoBehaviour
{
    private InventoryManager script_InventoryManager;
    public bool playTimeLine_WithItem;        // not need to initiallize it if it's not nessessary
    [HideInInspector]
    public GameObject item_timeLine;          // not need to initiallize it if it's not nessessary
    private string name_item_timeLine;
    [HideInInspector]
    private bool hasItem;

    void Start()
    {
        if (item_timeLine != null)
        {
            name_item_timeLine = item_timeLine.name;
        }
        else
        {
            name_item_timeLine = "null";
        }
    }
        

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            if (script_InventoryManager == null)
            {
                script_InventoryManager = other.GetComponentInChildren<InventoryManager>();
            }

            //if gameObject has the item and play the time line with it
            if (script_InventoryManager.Has_item(name_item_timeLine) && playTimeLine_WithItem)
            {
                hasItem = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            script_InventoryManager = null;
        }
    }


    public bool GetValue_hasItem()
    {
        return hasItem;
    }
}