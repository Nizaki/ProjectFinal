using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiHotbar : MonoBehaviour
{
    [SerializeField] private HotbarSlot[] slots;

    private PlayerInventory inventory;

    // Start is called before the first frame update
    private void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();
        inventory.onInvUpdate.AddListener(updateSlot);
        inventory.onSelectSlot.AddListener(selectSlot);
        updateSlot();
    }


    private void updateSlot()
    {
        foreach (var (item, index) in inventory.itemList.WithIndex())
        {
            var cslot = slots[index];
            if (item.Item.itemName != "")
            {
                cslot.itemImage.color = new Color(255, 255, 255);
                cslot.itemImage.sprite = item.Item.sprite;
                cslot.countText.text = item.count.ToString();
            }
            else
            {
                cslot.itemImage.color = new Color(255, 255, 255, 0);
                cslot.itemImage.sprite = null;
                cslot.countText.text = "";
            }
        }
    }

    private void selectSlot(int slot)
    {
        for (var i = 0; i < slots.Length; i++) slots[i].activePanel.SetActive(i == slot);
    }

    private void OnDestroy()
    {
        inventory.onInvUpdate.RemoveListener(updateSlot);
        inventory.onSelectSlot.RemoveListener(selectSlot);
    }
}