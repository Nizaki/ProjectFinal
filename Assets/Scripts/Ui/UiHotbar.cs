﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiHotbar : MonoBehaviour
{
    [SerializeField] private Image[] slot;
    [SerializeField] private GameObject[] selectedImage;

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

    private void selectSlot(int slot)
    {
        for (var i = 0; i < selectedImage.Length; i++)
        {
            selectedImage[i].SetActive(i == slot);
            ;
            if (i == slot)
                Debug.Log($"active {slot}");
        }
    }

    private void OnDestroy()
    {
        inventory.onInvUpdate.RemoveListener(updateSlot);
        inventory.onSelectSlot.RemoveListener(selectSlot);
    }
}