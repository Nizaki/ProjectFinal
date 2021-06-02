using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInventory : MonoBehaviour
{
    public List<ItemStack> itemList = new List<ItemStack>(8);
    public int selectedSlot = 0; //select item rage 0 - 9
    public ItemObject currentItem;
    [HideInInspector] public UnityEvent<int> onSelectSlot;
    [HideInInspector] public UnityEvent onInvUpdate;
    [HideInInspector] public UnityEvent<List<ItemStack>> onInventoryUpdate;
    public SpriteRenderer handRender;
    private Player player;
    [SerializeField] private ItemObject noneItem;

    private void Awake()
    {
        onSelectSlot ??= new UnityEvent<int>();
        onInvUpdate ??= new UnityEvent();
        onInventoryUpdate ??= new UnityEvent<List<ItemStack>>();
        player = GetComponent<Player>();
    }

    private void Start()
    {
        onInventoryUpdate.Invoke(itemList);
        SelectSlot(0);
    }

    private void LateUpdate()
    {
        foreach (var item in itemList.Where(item => item.spoilable).Where(item => item.remainTime < Time.time))
        {
            if (item.Item.spoiledItem)
            {
                var spoil = item.Item.spoiledItem;
                var count = item.count;
                RemoveItem(item);
                AddItem(spoil,count);
            }
            else
            {
                item.remainTime = Time.time + item.Item.spoilTime;
                Debug.LogError("no spoiled item found in spoiable item");
            }
        }
    }

    public bool AddItem(ItemObject item, int count = 1)
    {
        var temp = new ItemStack {Item = item, count = count};
        return AddItem(temp);
    }

    public bool AddItem(ItemStack item)
    {
        if (itemList.Exists(i => i.Item.name == item.Item.name))
        {
            itemList.Find(i => i.Item.name == item.Item.name).count += item.count;
            onInvUpdate.Invoke();
            onInventoryUpdate.Invoke(itemList);
            SelectSlot(selectedSlot);
            return true;
        }
        foreach (var cItem in itemList.Where(cItem => cItem.Item == noneItem))
        {
            cItem.Item = item.Item;
            cItem.count = item.count;
            if (item.spoilable)
                cItem.remainTime = Time.time + item.Item.spoilTime;
            onInvUpdate.Invoke();
            onInventoryUpdate.Invoke(itemList);
            SelectSlot(selectedSlot);
            return true;
        }

        SelectSlot(selectedSlot);
        return false;
    }

    public void SelectSlot(int slot)
    {
        if (slot < 0 || slot > 9)
        {
            Debug.LogError("Out of range");
            return;
        }

        selectedSlot = slot;
        onSelectSlot.Invoke(slot);
        currentItem = itemList[selectedSlot].Item;
    }

    public void RemoveItemAtSlot(int slot)
    {
        RemoveItem(itemList[slot]);
    }

    public bool RemoveItem(ItemStack itemStack)
    {
        return RemoveItem(itemStack.Item, itemStack.count);
    }

    public bool RemoveItem(ItemObject item, int count)
    {
        foreach (var (stack, index) in itemList.WithIndex())
            if (stack.Item == item) // item founded
            {
                if (stack.count >= count) // check item stock
                {
                    stack.count -= count;
                    if (stack.count <= 0)
                    {
                        itemList[index].Item = noneItem;
                        itemList[index].count = 0;
                        onInvUpdate.Invoke();
                        onInventoryUpdate.Invoke(itemList);
                        SelectSlot(selectedSlot);
                        return true;
                    }
                    SelectSlot(selectedSlot);
                    onInventoryUpdate.Invoke(itemList);
                    return true;
                }
                Debug.LogError($"Inventory : {item.name} not enough");
                return false;
            }

        Debug.LogError($"Inventory : {item.name} not founded");
        return false;
    }

    public void CleanItemAt(int slot)
    {
        itemList[slot].Item = noneItem;
        itemList[slot].count = 0;
        onInvUpdate.Invoke();
        onInventoryUpdate.Invoke(itemList);
    }
}