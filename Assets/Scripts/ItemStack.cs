using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemStack
{
    public ItemObject Item;
    public string name
    {
        get { return Item.itemName; }
    }
    public Sprite sprite
    {
        get { return Item.sprite; }
    }
    public ItemType type
    {
        get
        {
            return Item.type;
        }
    }

    public float data
    {
        get { return Item.data; }
    }
    public int count = 0;

    public void clear()
    {
        Item = null;
        count = 0;
    }
}

