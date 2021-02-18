using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiHotbar : MonoBehaviour
{
    [SerializeField]
    Image[] slot;
    PlayerInventory inventory;
    // Start is called before the first frame update
    void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();
        inventory.onInvUpdate.AddListener(updateSlot);
    }

    private void updateSlot()
    {
        foreach (var (item, index) in inventory.itemList.WithIndex())
        {
            if (item.Item != null)
            {
                slot[index].color = new Color(255, 255, 255);
                slot[index].sprite = item.Item.sprite;
            }
            else
            {
                slot[index].color = new Color(255, 255, 255, 0);
                slot[index].sprite = null;
            }
        }
    }
}

