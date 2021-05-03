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
                return true;
            }
            else if (t.name == item.name)
            {
                t.count += 1;
                onInvUpdate.Invoke();
                onInventoryUpdate.Invoke(itemList);
                return true;
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

        selectedSlot = slot;
        onSelectSlot.Invoke(slot);
        currentItem = itemList[selectedSlot].Item;
        handRender.sprite = currentItem.sprite;
        Debug.Log(slot);
        ////if useable use it instead of select
        //switch (itemList[slot].type)
        //{
        //    case ItemType.Food:
        //        player.Eat(Mathf.FloorToInt(itemList[slot].data));
        //        Debug.Log("Eat " + itemList[slot].name);
        //        RemoveItem(itemList[slot].Item, 1);
        //        break;
        //    case ItemType.Water:
        //        player.Drink(Mathf.FloorToInt(itemList[slot].data));
        //        Debug.Log("Drink " + itemList[slot].name);
        //        RemoveItem(itemList[slot].Item, 1);
        //        break;
        //    case ItemType.Health:
        //        player.Heal(Mathf.FloorToInt(itemList[slot].data));
        //        Debug.Log("Use " + itemList[slot].name);
        //        RemoveItem(itemList[slot].Item, 1);
        //        break;
        //    case ItemType.Equip:
        //        handRender.sprite = itemList[SelectedSlot].Item.sprite;
        //        break;
        //    case ItemType.Build:
        //        handRender.sprite = null;
        //        break;
        //    case ItemType.None:
        //        break;
        //    default:
        //        break;
        //}
    }

    private void Update()
    {
        if (currentItem == null) return;
        if (currentItem.type != ItemType.Build) return;
        if (Input.GetMouseButtonDown(0))
        {
            var worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldPos.z = 0;
            if (currentItem.prefab != null)
            {
                Instantiate(currentItem.prefab, worldPos, Quaternion.identity);
                RemoveItem(currentItem, 1);
            }
        }
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
                    Debug.Log($"Inventory : Remove {item.name} {count} ea");
                    if (stack.count <= 0)
                    {
                        itemList[index].Item = noneItem;
                        itemList[index].count = 0;
                        onInvUpdate.Invoke();
                        onInventoryUpdate.Invoke(itemList);
                        return true;
                    }

                    onInventoryUpdate.Invoke(itemList);
                    return true;
                }
                else
                {
                    Debug.Log($"Inventory : {item.name} not enough");
                    return false;
                }
            }

        Debug.Log($"Inventory : {item.name} not founded");
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