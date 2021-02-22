using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
public class ItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    Image image;
    [SerializeField]
    TextMeshProUGUI text;
    ItemObject currentItem;
    public void UpdateSlot(ItemStack item)
    {
        image.color = new Color(255, 255, 255, 255);
        image.sprite = item.sprite;
        text.text = item.count.ToString();
        currentItem = item.Item;
    }
    public void Clear()
    {
        image.color = new Color(255, 255, 255, 0);
        image.sprite = null;
        text.text = "";
        currentItem = null;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (currentItem != null)
        {
            TooltipsScreenSpace.ShowTooltip(currentItem.itemName);
        }
        else
        {
            TooltipsScreenSpace.HideTooltip();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipsScreenSpace.HideTooltip();
    }
}
