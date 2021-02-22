using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftPanel : MonoBehaviour
{
    public List<RecipeObject> recipes;
    public ItemSlot slot1, slot2, output;
    [SerializeField]
    ItemSlot[] slot;
    PlayerInventory inventory;
    [SerializeField]
    ItemStack input1, input2;
    ItemStack outputItem;
    int index1 = -1, index2 = -1, recipeIndex = -1;
    bool craftable = false;
    private void Awake()
    {

        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();
    }

    private void OnEnable()
    {
        ClearCraftPanel();
        updateSlot();
    }
    private void OnDisable()
    {
        TooltipsScreenSpace.HideTooltip();
    }
    // 0-8 inventory 9-10 input 11 output
    public void SlotClick(int slot)
    {
        craftable = false;
        if (slot < 9)
        {
            if (input1 == null || input1.Item == null)
            {
                if (index2 == slot)
                    return;
                input1.Item = inventory.itemList[slot].Item;
                input1.count = inventory.itemList[slot].count;
                slot1.UpdateSlot(input1);
                index1 = slot;
            }
            else if (input2 == null || input2.Item == null)
            {
                if (index1 == slot)
                    return;
                input2.Item = inventory.itemList[slot].Item;
                input2.count = inventory.itemList[slot].count;
                slot2.UpdateSlot(input2);
                index2 = slot;
            }
        }
        else if (slot == 9)
        {
            index1 = -1;
            input1.clear();
            slot1.Clear();
        }
        else if (slot == 10)
        {
            index2 = -1;
            input2.clear();
            slot2.Clear();
        }

        foreach (var (recipe, index) in recipes.WithIndex())
        {
            if (recipe == null)
                return;
            if (input1.Item == recipe.Input[0].Item && input2.Item == recipe.Input[1].Item)
            {
                if (input1.count >= recipe.Input[0].count && input2.count >= recipe.Input[1].count)
                {
                    recipeIndex = index;
                    output.UpdateSlot(recipe.Output);
                    craftable = true;
                }
                else
                {
                    output.Clear();
                    craftable = false;
                }
            }
        }
        if (!craftable)
        {
            output.Clear();
        }
    }

    public void Craft()
    {
        if (craftable)
        {
            if (inventory.RemoveItem(recipes[recipeIndex].Input[0]) && inventory.RemoveItem(recipes[recipeIndex].Input[1]))
            {
                inventory.AddItem(recipes[recipeIndex].Output);
                ClearCraftPanel();
                updateSlot();
            }
        }
    }
    void ClearCraftPanel()
    {
        output.Clear();
        slot1.Clear();
        slot2.Clear();
        input1.clear();
        input2.clear();
        craftable = false;
    }
    // Update is called once per frame
    void updateSlot()
    {

        foreach (var (item, index) in inventory.itemList.WithIndex())
        {
            if (item.Item != null)
            {
                slot[index].UpdateSlot(item);
            }
            else
            {
                slot[index].Clear();
            }
        }
    }
}
