using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInventory : MonoBehaviour
{
    public List<ItemStack> itemList = new List<ItemStack>(8);
    public int SelectedSlot = 0; //select item rage 0 - 9

    public UnityEvent<int> onSelectSlot;
    public UnityEvent onInvUpdate;
    Player player;
    private void Start()
    {
        if (onSelectSlot == null)
            onSelectSlot = new UnityEvent<int>();
        if (onInvUpdate == null)
            onInvUpdate = new UnityEvent();
        player = GetComponent<Player>();
    }
    public void AddItem(ItemStack item)
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            if (itemList[i].Item == null)
            {
                itemList[i] = item;
                onInvUpdate.Invoke();
                return;
            }
            else if (itemList[i].name == item.name)
            {
                itemList[i].count += 1;
                onInvUpdate.Invoke();
                return;
            }
        }
        throw new System.Exception("There was no inv left ? maybe");
    }

    public void SelectSlot(int slot)
    {
        if (slot < 0 || slot > 9)
        {
            Debug.LogError("Out of range");
            return;
        }
        //if useable use it instead of select
        switch (itemList[slot].type)
        {
            case ItemType.Food:
                player.Eat(Mathf.FloorToInt(itemList[slot].data));
                Debug.Log("Eat " + itemList[slot].name);
                RemoveItem(slot);
                break;
            case ItemType.Water:
                player.Drink(Mathf.FloorToInt(itemList[slot].data));
                Debug.Log("Drink " + itemList[slot].name);
                break;
            case ItemType.Health:
                player.Heal(Mathf.FloorToInt(itemList[slot].data));
                Debug.Log("Use " + itemList[slot].name);
                break;
            case ItemType.Equip:
            case ItemType.Build:
                SelectedSlot = slot;
                onSelectSlot.Invoke(slot);
                break;
            case ItemType.None:
                break;
            default:
                break;
        }
    }

    public void Use()
    {

    }

    public void RemoveItem(int slot)
    {
        ItemStack empty = new ItemStack();
        itemList[slot] = empty;
        onInvUpdate.Invoke();
    }
}
