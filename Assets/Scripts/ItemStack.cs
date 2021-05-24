using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemStack
{
    public ItemObject Item;
    public string name => Item.itemName;

    public Sprite sprite => Item.sprite;
    public ItemType type => Item.type;

    public float data => Item.data;
    public int count = 0;
    public bool spoilable => Item.spoilable;
    [HideInInspector]public float remainTime;
    public void clear()
    {
        Item = null;
        count = 0;
    }
}