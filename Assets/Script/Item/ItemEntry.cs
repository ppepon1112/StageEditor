using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemEntry
{
    public string itemID;
    public int Count;

    public ItemEntry() { }
    public ItemEntry(string name, int count = 1)
    {
        this.itemID = name;
        this.Count = count;
    }
}
