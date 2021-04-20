using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class RecipeSlot : MonoBehaviour
{
  public RecipeObject recipe;
  [SerializeField]
  Image picture;
  [SerializeField]
  TextMeshProUGUI itemName;
  [SerializeField]
  Button button;
  bool Craftable = false;
  PlayerInventory player;
  private void Awake()
  {
    player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();
    player.onInventoryUpdate.AddListener(CheckCraftable);
  }
  // Start is called before the first frame update
  void Start()
  {
    picture.sprite = recipe.Output.Item.sprite;
    itemName.text = recipe.Output.Item.itemName;
  }

  void CheckCraftable(List<ItemStack> inventory)
  {
    if (inventory == null) return;
    int itemCount = 0;
    foreach (var req in recipe.Input)
    {
      if (inventory.Exists(item => item.Item.itemName == req.Item.itemName))
        if (inventory.Find(item => item.Item.itemName == req.Item.itemName).count >= req.count)
          itemCount++;
    }
    if (itemCount == recipe.Input.Length)
    {
      Craftable = true;
      button.interactable = true;
    }
    else
    {
      button.interactable = false;
    }
    Debug.Log($"has {itemCount}/{recipe.Input.Length} : {Craftable}");
    // if(itemCount == recipe.Input.Length){
    // }
  }

  public void Craft()
  {
    if (Craftable)
    {
      foreach (var req in recipe.Input)
      {
        player.RemoveItem(req);
      }
      player.AddItem(recipe.Output);
    }
  }
}
