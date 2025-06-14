using System.Collections.Generic;
using UnityEngine;

public class GInventory 
{
    private List<GameObject> items = new List<GameObject>();

    public void AddItem(GameObject item)
    {
        items.Add(item);
    }

    public GameObject FindItemWithTag(string tag)
    {
        foreach (GameObject item in items)
        {
            if (item == null) break;

            if (item.tag == tag)
            {
                return item;
            }
        }
        return null;
    }

    public void RemoveItem(GameObject item)
    {
        int indexToRemove = -1;

        foreach (GameObject itemToRemove in items)
        {
            indexToRemove++;

            if (itemToRemove == item)
            {
                break;
            }
        }

        if (indexToRemove >= -1)
        {
            items.RemoveAt(indexToRemove);
        }
    }
}
