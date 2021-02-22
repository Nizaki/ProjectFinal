using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CampfirePanel : MonoBehaviour
{
    public static CampfirePanel Instance;

    CampFire currentCamp;
    [SerializeField]
    TextMeshProUGUI statusText;
    [SerializeField]
    ItemSlot[] slot;
    PlayerInventory inventory;
    private void Awake()
    {
        Instance = this;
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();
        gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        updateSlot();
    }
    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeSelf)
        {
            statusText.SetText($"Remaining fuel : {currentCamp.fuel.ToString("0")}%<br>" +
                $"Drain Rate : {currentCamp.fuelDrain}% per second");
        }
    }

    public void SlotClick(int slot)
    {
        if (inventory.itemList[slot].Item.fuelable)
        {
            ItemStack temp = inventory.itemList[slot];
            //add this item to fuel list
            inventory.RemoveItem(temp.Item, 1);
            currentCamp.fuel += temp.Item.fuelAmount;
            updateSlot();
        }
    }

    void Show(CampFire campFire)
    {
        gameObject.SetActive(true);
        currentCamp = campFire;
        // do some logic with campfire
    }

    public void Hide()
    {
        currentCamp = null;
        gameObject.SetActive(false);
    }

    public static void ShowPanel(CampFire campFire)
    {
        Instance.Show(campFire);
    }

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
