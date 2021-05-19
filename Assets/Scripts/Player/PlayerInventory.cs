using System.Collections;
using System.Collections.Generic;
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
    }

    public bool AddItem(ItemObject item, int count = 1)
    {
        var temp = new ItemStack {Item = item, count = count};
        return AddItem(temp);
    }

    public bool AddItem(ItemStack item)
    {
        foreach (var t in itemList)
            if (t.Item == noneItem)
            {
                t.Item = item.Item;
                t.count = item.count;
                onInvUpdate.Invoke();
                onInventoryUpdate.Invoke(itemList);
                SelectSlot(selectedSlot);
                return true;
            }
            else if (t.name == item.name)
            {
                t.count += 1;
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

    //invoke when not clicking ui
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