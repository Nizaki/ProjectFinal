using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInventory : MonoBehaviour
{
    public List<ItemStack> itemList = new List<ItemStack>(8);
    public int SelectedSlot = 0; //select item rage 0 - 9
    public ItemObject currentItem;
    public UnityEvent<int> onSelectSlot;
    public UnityEvent onInvUpdate;
    public SpriteRenderer handRender;
    Player player;
    private void Start()
    {
        if (onSelectSlot == null)
            onSelectSlot = new UnityEvent<int>();
        if (onInvUpdate == null)
            onInvUpdate = new UnityEvent();
        player = GetComponent<Player>();
    }
    public bool AddItem(ItemStack item)
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            if (itemList[i].Item == null)
            {
                itemList[i].Item = item.Item;
                itemList[i].count = item.count;
                onInvUpdate.Invoke();
                return true;
            }
            else if (itemList[i].name == item.name)
            {
                itemList[i].count += 1;
                onInvUpdate.Invoke();
                return true;
            }
        }
        return false;
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
                CleanItemAt(slot);
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
                SelectedSlot = slot;
                handRender.sprite = itemList[SelectedSlot].Item.sprite;
                currentItem = itemList[SelectedSlot].Item;
                onSelectSlot.Invoke(slot);
                break;
            case ItemType.Build:
            case ItemType.None:
                break;
            default:
                break;
        }
    }

    public void Use()
    {

    }

    public bool RemoveItem(ItemStack itemStack)
    {
        return RemoveItem(itemStack.Item, itemStack.count);
    }

    public bool RemoveItem(ItemObject item, int count)
    {
        foreach (var (stack, index) in itemList.WithIndex())
        {
            if (stack.Item == item) // item founded
            {
                if (stack.count >= count) // check item stock
                {
                    stack.count -= count;
                    Debug.Log($"Inventory : Remove {item.name} {count} ea");
                    if (stack.count <= 0)
                    {
                        itemList[index] = new ItemStack();
                        onInvUpdate.Invoke();
                        return true;
                    }
                    return true;
                }
                else
                {
                    Debug.Log($"Inventory : {item.name} not enough");
                    return false;
                }
            }
        }
        Debug.Log($"Inventory : {item.name} not founded");
        return false;
    }

    public void CleanItemAt(int slot)
    {
        ItemStack empty = new ItemStack();
        itemList[slot] = empty;
        onInvUpdate.Invoke();
    }
}
