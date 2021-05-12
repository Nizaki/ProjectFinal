using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class RecipeSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public RecipeObject recipe;
    [SerializeField] private Image picture;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private Button button;
    private bool craftable = false;
    private PlayerInventory player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();
        player.onInventoryUpdate.AddListener(CheckCraftable);
    }

    // Start is called before the first frame update
    private void Start()
    {
        picture.sprite = recipe.Output.Item.sprite;
        itemName.text = recipe.Output.Item.itemName;
    }

    private void CheckCraftable(List<ItemStack> inventory)
    {
        if (inventory == null) return;
        var itemCount = 0;
        foreach (var req in recipe.Input)
            if (inventory.Exists(item => item.Item.itemName == req.Item.itemName))
                if (inventory.Find(item => item.Item.itemName == req.Item.itemName).count >= req.count)
                    itemCount++;
        if (itemCount == recipe.Input.Length)
        {
            craftable = true;
            button.interactable = true;
        }
        else
        {
            button.interactable = false;
        }

        // if(itemCount == recipe.Input.Length){
        // }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        var temp = recipe.Input.Aggregate("", (current, rec) => current + $"{rec.Item.name} - {rec.count} \n");
        // TooltipsScreenSpace.ShowTooltip($"{recipe.Output.Item.name} \n {temp}");
        CraftingWindow.ShowText($"{recipe.Output.Item.name} \n {temp}");
        Debug.Log("Pointer Enter");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // TooltipsScreenSpace.HideTooltip();
        CraftingWindow.Hide();
    }
    
    
    public void Craft()
    {
        if (craftable)
        {
            foreach (var req in recipe.Input) player.RemoveItem(req);
            player.AddItem(recipe.Output);
        }
    }
}