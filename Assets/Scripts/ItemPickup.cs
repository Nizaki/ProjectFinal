using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ItemStack Item;
    PlayerInventory inventory;
    private void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //add item to inventory
        inventory.AddItem(Item);
        Destroy(this.gameObject);
    }
}
